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
    public partial class DesempenhoVendedor : System.Web.UI.Page
    {
        DesempenhoController desempenhoController = new DesempenhoController();
        BaseController baseController = new BaseController();

        public double totalPerc0 = 0;
        public double totalCont0 = 0;
        public double totalPerc1 = 0;
        public double totalCont1 = 0;
        public double totalPerc11 = 0;
        public double totalCont11 = 0;
        public double totalPerc2 = 0;
        public double totalCont2 = 0;
        public double totalPerc3 = 0;
        public double totalCont3 = 0;
        public double totalPerc4 = 0;
        public double totalCont4 = 0;
        public double totalPerc5 = 0;
        public double totalCont5 = 0;
        public double totalPerc6 = 0;
        public double totalCont6 = 0;
        public double totalZeroPeca = 0;
        public double totalUmaPeca = 0;
        public double totalDuasPecas = 0;
        public double totalTresPecas = 0;
        public double totalQuatroPecas = 0;
        public double totalCincoPecas = 0;
        public double totalPA = 0;
        public double totalContPA = 0;
        public double totalMedAte = 0;
        public double totalContMedAte = 0;
        public double totalMedVl = 0;
        public double totalContMedVl = 0;
        public double totalVl = 0;
        public double totalContVl = 0;
        public double totalComLink = 0;
        public double totalSemLink = 0;
        public int? totalPcs = 0;
        public double totalAte = 0;
        public double totalNotaFinal = 0;
        public double totalNumeroDias = 0;
        public double totalContNotaFinal = 0;
        public double totalContDias = 0;
        public int nota = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CarregaDropDownListFilial();
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

        private void CarregaGridViewDesempenho()
        {
            GridViewDesempenho.DataSource = desempenhoController.BuscaDesempenhoVenda(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            GridViewDesempenho.DataBind();

            GridViewGrupo.DataSource = desempenhoController.BuscaDesempenhoVendaGrupo(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            GridViewGrupo.DataBind();

            GridViewRede.DataSource = desempenhoController.BuscaDesempenhoVendaRede(TextBoxDataInicio.Text, TextBoxDataFim.Text);
            GridViewRede.DataBind();
        }

        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Add(new ListItem("Selecione", "0"));
            ddlFilial.SelectedValue = "0";
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void ButtonPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                return;

            if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            CarregaGridViewDesempenho();
        }

        protected void GridViewDesempenho_DataBound(object sender, EventArgs e)
        {
            int i = 0;

            GridViewRow footer = GridViewDesempenho.FooterRow;

            foreach (GridViewRow item in GridViewDesempenho.Rows)
            {
                i = 3;

                Literal literalPA = item.FindControl("LiteralPA") as Literal;

                if (Convert.ToDouble(literalPA.Text) > (totalPA / totalContPA))
                    item.Cells[3].BackColor = System.Drawing.Color.Green;
                else
                {
                    i--;
                    item.Cells[3].BackColor = System.Drawing.Color.Red;
                }

                Literal literalMedAte = item.FindControl("LiteralMediaAte") as Literal;

                if (Convert.ToDouble(literalMedAte.Text) > (totalMedAte / totalContMedAte))
                    item.Cells[4].BackColor = System.Drawing.Color.Green;
                else
                {
                    i--;
                    item.Cells[4].BackColor = System.Drawing.Color.Red;
                }

                Literal literalMedVl = item.FindControl("LiteralMediaVl") as Literal;

                if (Convert.ToDouble(literalMedVl.Text) > (totalMedVl / totalContMedVl))
                    item.Cells[5].BackColor = System.Drawing.Color.Green;
                else
                {
                    i--;
                    item.Cells[5].BackColor = System.Drawing.Color.Red;
                }

                Literal literalPerc1 = item.FindControl("LiteralPerc1") as Literal;

                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) > ((totalPerc1 * 100) / totalCont1))
                    item.Cells[7].BackColor = System.Drawing.Color.Red;
                else
                    item.Cells[7].BackColor = System.Drawing.Color.Green;

                Literal literalPerc5 = item.FindControl("LiteralPerc5") as Literal;

                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) > ((totalPerc5 * 100) / totalCont5))
                    item.Cells[8].BackColor = System.Drawing.Color.Green;
                else
                    item.Cells[8].BackColor = System.Drawing.Color.Red;

                Literal literalNota = item.FindControl("LiteralNota") as Literal;

                if (literalNota != null)
                {
                    if (i == 3)
                        literalNota.Text = "A";
                    if (i == 2)
                        literalNota.Text = "B";
                    if (i < 2)
                        literalNota.Text = "C";
                }
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Loja";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;

                Sp_Desempenho_Venda_LojaResult dvl = desempenhoController.BuscaDesempenhoVendaLoja(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
                
                if (dvl != null)
                    footer.Cells[3].Text = (Convert.ToDouble(dvl.Vendas_qt_total+dvl.Vendas_N) / Convert.ToDouble(dvl.Vendas_1+dvl.Vendas_2+dvl.Vendas_3+dvl.Vendas_4+dvl.Vendas_5)).ToString("N2");
                else
                    footer.Cells[3].Text = "0";

                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = (totalMedAte / totalContMedAte).ToString("N2");
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[5].Text = (totalMedVl / totalContMedVl).ToString("N2");
                footer.Cells[5].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].Text = ((totalPerc11 * 100) / totalCont11).ToString("N0") + "%";
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[7].Text = ((totalPerc1 * 100) / totalCont1).ToString("N0") + "%";
                footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[8].Text = ((totalPerc5 * 100) / totalCont5).ToString("N0") + "%";
                footer.Cells[8].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[9].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[10].Text = (desempenhoController.BuscaTotalVendedoresAtivo().Count()).ToString("N0");
                footer.Cells[10].BackColor = System.Drawing.Color.PeachPuff;
            }
        }

        protected void GridViewDesempenho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Desempenho_VendaResult desempenho = e.Row.DataItem as Sp_Desempenho_VendaResult;

            if (desempenho != null)
            {
                int? total = desempenho.Vendas_0 + desempenho.Vendas_1 + desempenho.Vendas_2 + desempenho.Vendas_3 + desempenho.Vendas_4 + desempenho.Vendas_5;

                if (total == null || total == 0)
                    total = 1;

                if (total > 0)
                {
                    double subTotalPerc11 = Convert.ToDouble(desempenho.Vendas_1) / Convert.ToDouble(total);

                    Literal literalPerc11 = e.Row.FindControl("LiteralPerc11") as Literal;

                    if (literalPerc11 != null)
                    {
                        literalPerc11.Text = (subTotalPerc11 * 100).ToString("N0") + "%";
                        totalPerc11 += subTotalPerc11;
                        totalCont11++;
                    }

                    double subTotalPerc1 = (Convert.ToDouble(desempenho.Vendas_1) + Convert.ToDouble(desempenho.Vendas_2)) / Convert.ToDouble(total);

                    Literal literalPerc1 = e.Row.FindControl("LiteralPerc1") as Literal;

                    if (literalPerc1 != null)
                    {
                        literalPerc1.Text = (subTotalPerc1 * 100).ToString("N0") + "%";
                        totalPerc1 += subTotalPerc1;
                        totalCont1++;
                    }

                    double subTotalPerc5 = (Convert.ToDouble(desempenho.Vendas_4) + Convert.ToDouble(desempenho.Vendas_5)) / Convert.ToDouble(total);

                    Literal literalPerc5 = e.Row.FindControl("LiteralPerc5") as Literal;

                    if (literalPerc5 != null)
                    {
                        literalPerc5.Text = (subTotalPerc5 * 100).ToString("N0") + "%";
                        totalPerc5 += subTotalPerc5;
                        totalCont5++;
                    }
                }

                if ((total - desempenho.Vendas_0) > 0)
                {
                    double subTotalPA = Convert.ToDouble(desempenho.Vendas_qt_total) / Convert.ToDouble(total - desempenho.Vendas_0);

                    Literal literalPA = e.Row.FindControl("LiteralPA") as Literal;

                    if (literalPA != null)
                    {
                        literalPA.Text = (subTotalPA).ToString("N2");
                        totalPA += subTotalPA;
                        totalContPA++;
                    }

                    double subTotalMedVl = Convert.ToDouble(desempenho.Valor_Pago) / Convert.ToDouble(total - desempenho.Vendas_0);

                    Literal literalMediaVl = e.Row.FindControl("LiteralMediaVl") as Literal;

                    if (literalMediaVl != null)
                    {
                        literalMediaVl.Text = (subTotalMedVl).ToString("N2");
                        totalMedVl += subTotalMedVl;
                        totalContMedVl++;
                    }
                }
                else
                {
                    Literal literalPA = e.Row.FindControl("LiteralPA") as Literal;
                    literalPA.Text = "0";
                    Literal literalMediaVl = e.Row.FindControl("LiteralMediaVl") as Literal;
                    literalMediaVl.Text = "0";
                }

                if (desempenho.Numero_Dias > 0)
                {
                    double subTotalMedAte = Convert.ToDouble(total) / Convert.ToDouble(desempenho.Numero_Dias);

                    Literal literalMediaAte = e.Row.FindControl("LiteralMediaAte") as Literal;

                    if (literalMediaAte != null)
                    {
                        literalMediaAte.Text = (subTotalMedAte).ToString("N2");
                        totalMedAte += subTotalMedAte;
                        totalContMedAte++;
                    }
                }

                Literal literalRanking = e.Row.FindControl("LiteralRanking") as Literal;

                if (literalRanking != null)
                {
                    List<Sp_Ranking_VendaResult> sp_Ranking_VendaResult = desempenhoController.BuscaDesempenhoVendedor(TextBoxDataInicio.Text, TextBoxDataFim.Text);

                    if (sp_Ranking_VendaResult != null)
                    {
                        int i = 0;

                        foreach (Sp_Ranking_VendaResult ranking in sp_Ranking_VendaResult)
                        {
                            i++;

                            if (ranking.Nome_Vendedor.Equals(desempenho.Nome_Vendedor))
                            {
                                literalRanking.Text = i.ToString();

                                break;
                            }
                        }
                    }
                }
            }
        }

        protected void GridViewGrupo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Desempenho_Venda_LojaResult desempenho = e.Row.DataItem as Sp_Desempenho_Venda_LojaResult;

            if (desempenho != null)
            {
                int? total = desempenho.Vendas_0 + desempenho.Vendas_1 + desempenho.Vendas_2 + desempenho.Vendas_3 + desempenho.Vendas_4 + desempenho.Vendas_5;

                if (total == null || total == 0)
                    total = 1;

                if (total > 0)
                {
                    double subTotalPerc1 = (Convert.ToDouble(desempenho.Vendas_1) + Convert.ToDouble(desempenho.Vendas_2)) / Convert.ToDouble(total);

                    Literal literalPerc1 = e.Row.FindControl("LiteralPerc1") as Literal;

                    if (literalPerc1 != null)
                    {
                        literalPerc1.Text = (subTotalPerc1 * 100).ToString("N0") + "%";
                        totalPerc1 += subTotalPerc1;
                        totalCont1++;
                    }

                    double subTotalPerc11 = Convert.ToDouble(desempenho.Vendas_1) / Convert.ToDouble(total);

                    Literal literalPerc11 = e.Row.FindControl("LiteralPerc11") as Literal;

                    if (literalPerc11 != null)
                    {
                        literalPerc11.Text = (subTotalPerc11 * 100).ToString("N0") + "%";
                        totalPerc11 += subTotalPerc11;
                        totalCont11++;
                    }

                    double subTotalPerc5 = (Convert.ToDouble(desempenho.Vendas_4) + Convert.ToDouble(desempenho.Vendas_5)) / Convert.ToDouble(total);

                    Literal literalPerc5 = e.Row.FindControl("LiteralPerc5") as Literal;

                    if (literalPerc5 != null)
                    {
                        literalPerc5.Text = (subTotalPerc5 * 100).ToString("N0") + "%";
                        totalPerc5 += subTotalPerc5;
                        totalCont5++;
                    }
                }

                if ((total - desempenho.Vendas_0) > 0)
                {
                    double subTotalPA = Convert.ToDouble(desempenho.Vendas_qt_total) / Convert.ToDouble(total - desempenho.Vendas_0);

                    Literal literalPA = e.Row.FindControl("LiteralPA") as Literal;

                    if (literalPA != null)
                    {
                        literalPA.Text = (subTotalPA).ToString("N2");
                        totalPA += subTotalPA;
                        totalContPA++;
                    }

                    double subTotalMedVl = Convert.ToDouble(desempenho.Valor_Pago) / Convert.ToDouble(total - desempenho.Vendas_0);

                    Literal literalMediaVl = e.Row.FindControl("LiteralMediaVl") as Literal;

                    if (literalMediaVl != null)
                    {
                        literalMediaVl.Text = (subTotalMedVl).ToString("N2");
                        totalMedVl += subTotalMedVl;
                        totalContMedVl++;
                    }
                }
                else
                {
                    Literal literalPA = e.Row.FindControl("LiteralPA") as Literal;
                    literalPA.Text = "0";
                    Literal literalMediaVl = e.Row.FindControl("LiteralMediaVl") as Literal;
                    literalMediaVl.Text = "0";
                }

                if (desempenho.Numero_Dias > 0)
                {
                    double subTotalMedAte = Convert.ToDouble(desempenho.Numero_Vendedor)/100;

                    Literal literalMediaAte = e.Row.FindControl("LiteralMediaAte") as Literal;

                    if (literalMediaAte != null)
                    {
                        literalMediaAte.Text = (subTotalMedAte).ToString("N2");
                        totalMedAte += subTotalMedAte;
                        totalContMedAte++;
                    }
                }
            }
        }

        protected void GridViewRede_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Desempenho_Venda_LojaResult desempenho = e.Row.DataItem as Sp_Desempenho_Venda_LojaResult;

            if (desempenho != null)
            {
                int? total = desempenho.Vendas_0 + desempenho.Vendas_1 + desempenho.Vendas_2 + desempenho.Vendas_3 + desempenho.Vendas_4 + desempenho.Vendas_5;

                if (total == null || total == 0)
                    total = 1;

                if (total > 0)
                {
                    double subTotalPerc1 = (Convert.ToDouble(desempenho.Vendas_1) + Convert.ToDouble(desempenho.Vendas_2)) / Convert.ToDouble(total);

                    Literal literalPerc1 = e.Row.FindControl("LiteralPerc1") as Literal;

                    if (literalPerc1 != null)
                    {
                        literalPerc1.Text = (subTotalPerc1 * 100).ToString("N0") + "%";
                        totalPerc1 += subTotalPerc1;
                        totalCont1++;
                    }

                    double subTotalPerc11 = Convert.ToDouble(desempenho.Vendas_1) / Convert.ToDouble(total);

                    Literal literalPerc11 = e.Row.FindControl("LiteralPerc11") as Literal;

                    if (literalPerc11 != null)
                    {
                        literalPerc11.Text = (subTotalPerc11 * 100).ToString("N0") + "%";
                        totalPerc11 += subTotalPerc11;
                        totalCont11++;
                    }

                    double subTotalPerc5 = (Convert.ToDouble(desempenho.Vendas_4) + Convert.ToDouble(desempenho.Vendas_5)) / Convert.ToDouble(total);

                    Literal literalPerc5 = e.Row.FindControl("LiteralPerc5") as Literal;

                    if (literalPerc5 != null)
                    {
                        literalPerc5.Text = (subTotalPerc5 * 100).ToString("N0") + "%";
                        totalPerc5 += subTotalPerc5;
                        totalCont5++;
                    }
                }

                if ((total - desempenho.Vendas_0) > 0)
                {
                    double subTotalPA = Convert.ToDouble(desempenho.Vendas_qt_total) / Convert.ToDouble(total - desempenho.Vendas_0);

                    Literal literalPA = e.Row.FindControl("LiteralPA") as Literal;

                    if (literalPA != null)
                    {
                        literalPA.Text = (subTotalPA).ToString("N2");
                        totalPA += subTotalPA;
                        totalContPA++;
                    }

                    double subTotalMedVl = Convert.ToDouble(desempenho.Valor_Pago) / Convert.ToDouble(total - desempenho.Vendas_0);

                    Literal literalMediaVl = e.Row.FindControl("LiteralMediaVl") as Literal;

                    if (literalMediaVl != null)
                    {
                        literalMediaVl.Text = (subTotalMedVl).ToString("N2");
                        totalMedVl += subTotalMedVl;
                        totalContMedVl++;
                    }
                }
                else
                {
                    Literal literalPA = e.Row.FindControl("LiteralPA") as Literal;
                    literalPA.Text = "0";
                    Literal literalMediaVl = e.Row.FindControl("LiteralMediaVl") as Literal;
                    literalMediaVl.Text = "0";
                }

                if (desempenho.Numero_Dias > 0)
                {
                    double subTotalMedAte = Convert.ToDouble(desempenho.Numero_Vendedor) / 100;

                    Literal literalMediaAte = e.Row.FindControl("LiteralMediaAte") as Literal;

                    if (literalMediaAte != null)
                    {
                        literalMediaAte.Text = (subTotalMedAte).ToString("N2");
                        totalMedAte += subTotalMedAte;
                        totalContMedAte++;
                    }
                }
            }
        }
    }
}
