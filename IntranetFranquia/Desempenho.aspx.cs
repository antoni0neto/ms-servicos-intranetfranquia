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
    public partial class Desempenho : System.Web.UI.Page
    {
        DesempenhoController desempenhoController = new DesempenhoController();
        BaseController baseController = new BaseController();

        public double totalPerc0 = 0;
        public double totalCont0 = 0;
        public double totalPerc1 = 0;
        public double totalCont1 = 0;
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
        public double totalPerc11 = 0;
        public double totalCont11 = 0;

        //SOMATORIO DO PA
        double _totalVendas = 0;
        double _totalVendasPcs = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregaDropDownListFilial();

                //Esconder paineis
                pnlFilial.Visible = false;
                pnlGeral.Visible = false;
            }
        }

        #region "DADOS INICIAIS"
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
        protected void ddlFilial_DataBound(object sender, EventArgs e)
        {
            ddlFilial.Items.Insert(0, new ListItem("Selecione", "0"));
            ddlFilial.Items.Add(new ListItem("GERAL", "G"));

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
        #endregion

        protected void ButtonPesquisarVendas_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") ||
                TextBoxDataInicio.Text == null ||
                TextBoxDataFim.Text.Equals("") ||
                TextBoxDataFim.Text == null ||
                ddlFilial.SelectedValue.ToString().Equals("0") ||
                ddlFilial.SelectedValue.ToString().Equals(""))
                return;

            pnlFilial.Visible = false;
            pnlGeral.Visible = false;
            if (ddlFilial.SelectedValue == "G") //GERAL
            {
                pnlGeral.Visible = true;
                CarregaGridViewGeral();
            }
            else
            {
                CarregaGridViewDesempenho();
                pnlFilial.Visible = true;
            }
        }

        #region "GRID VIEW DESEMPENHO"
        private void CarregaGridViewDesempenho()
        {
            GridViewDesempenho.DataSource = desempenhoController.BuscaDesempenhoVenda(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());
            GridViewDesempenho.DataBind();

            GridViewRede.DataSource = desempenhoController.BuscaDesempenhoVendaRede(TextBoxDataInicio.Text, TextBoxDataFim.Text);
            GridViewRede.DataBind();
        }
        protected void GridViewDesempenho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Desempenho_VendaResult desempenho = e.Row.DataItem as Sp_Desempenho_VendaResult;

            if (desempenho != null)
            {
                Literal literalNomeFuncionario = e.Row.FindControl("LiteralNomeFuncionario") as Literal;

                if (literalNomeFuncionario != null)
                    literalNomeFuncionario.Text = desempenho.Nome_Vendedor;

                int? total = desempenho.Vendas_0 + desempenho.Vendas_1 + desempenho.Vendas_2 + desempenho.Vendas_3 + desempenho.Vendas_4 + desempenho.Vendas_5;

                if (total == null || total == 0)
                    total = 1;

                if (total > 0)
                {
                    double subTotalPerc0 = Convert.ToDouble(desempenho.Vendas_0) / Convert.ToDouble(total);

                    Literal literalPerc0 = e.Row.FindControl("LiteralPerc0") as Literal;

                    if (literalPerc0 != null)
                    {
                        literalPerc0.Text = (subTotalPerc0 * 100).ToString("N0") + "%";
                        totalPerc0 += subTotalPerc0;
                        //totalCont0++;
                    }

                    double subTotalPerc1 = Convert.ToDouble(desempenho.Vendas_1) / Convert.ToDouble(total);

                    Literal literalPerc1 = e.Row.FindControl("LiteralPerc1") as Literal;

                    if (literalPerc1 != null)
                    {
                        literalPerc1.Text = (subTotalPerc1 * 100).ToString("N0") + "%";
                        totalPerc1 += subTotalPerc1;
                        //totalCont1++;
                    }

                    double subTotalPerc2 = Convert.ToDouble(desempenho.Vendas_2) / Convert.ToDouble(total);

                    Literal literalPerc2 = e.Row.FindControl("LiteralPerc2") as Literal;

                    if (literalPerc2 != null)
                    {
                        literalPerc2.Text = (subTotalPerc2 * 100).ToString("N0") + "%";
                        totalPerc2 += subTotalPerc2;
                        //totalCont2++;
                    }

                    double subTotalPerc3 = Convert.ToDouble(desempenho.Vendas_3) / Convert.ToDouble(total);

                    Literal literalPerc3 = e.Row.FindControl("LiteralPerc3") as Literal;

                    if (literalPerc3 != null)
                    {
                        literalPerc3.Text = (subTotalPerc3 * 100).ToString("N0") + "%";
                        totalPerc3 += subTotalPerc3;
                        //totalCont3++;
                    }

                    double subTotalPerc4 = Convert.ToDouble(desempenho.Vendas_4) / Convert.ToDouble(total);

                    Literal literalPerc4 = e.Row.FindControl("LiteralPerc4") as Literal;

                    if (literalPerc4 != null)
                    {
                        literalPerc4.Text = (subTotalPerc4 * 100).ToString("N0") + "%";
                        totalPerc4 += subTotalPerc4;
                        //totalCont4++;
                    }

                    double subTotalPerc5 = Convert.ToDouble(desempenho.Vendas_5) / Convert.ToDouble(total);

                    Literal literalPerc5 = e.Row.FindControl("LiteralPerc5") as Literal;

                    if (literalPerc5 != null)
                    {
                        literalPerc5.Text = (subTotalPerc5 * 100).ToString("N0") + "%";
                        totalPerc5 += subTotalPerc5;
                        //totalCont5++;
                    }

                    double subTotalVl = Convert.ToDouble(total - desempenho.Vendas_Consumidor) / Convert.ToDouble(total);

                    Literal literalPercVl = e.Row.FindControl("LiteralPercVl") as Literal;

                    if (literalPercVl != null)
                    {
                        literalPercVl.Text = (subTotalVl).ToString("N2");
                        totalVl += subTotalVl;
                        totalContVl++;
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
                        //totalContPA++;
                    }

                    double subTotalMedVl = Convert.ToDouble(desempenho.Valor_Pago) / Convert.ToDouble(total - desempenho.Vendas_0);
                    //leandro
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

                Literal literalComLink = e.Row.FindControl("LiteralComLink") as Literal;

                if (literalComLink != null)
                {
                    literalComLink.Text = (total - desempenho.Vendas_Consumidor).ToString();
                    totalComLink += Convert.ToDouble(total - desempenho.Vendas_Consumidor);
                }

                Literal literalSemLink = e.Row.FindControl("LiteralSemLink") as Literal;

                if (literalSemLink != null)
                {
                    literalSemLink.Text = desempenho.Vendas_Consumidor.ToString();
                    totalSemLink += Convert.ToDouble(desempenho.Vendas_Consumidor);
                }

                int? subTotalPcs = desempenho.Vendas_qt_total;

                Literal literalTotalPcs = e.Row.FindControl("LiteralTotalPcs") as Literal;

                if (literalTotalPcs != null)
                {
                    literalTotalPcs.Text = (subTotalPcs).ToString();
                    totalPcs += subTotalPcs;
                }

                double subTotalAte = Convert.ToDouble(total);

                Literal literalTotalAte = e.Row.FindControl("LiteralTotalAte") as Literal;

                if (literalTotalAte != null)
                {
                    literalTotalAte.Text = (subTotalAte).ToString();
                    totalAte += subTotalAte;
                }

                Literal literalNotaFinal = e.Row.FindControl("LiteralNotaFinal") as Literal;

                if (literalNotaFinal != null)
                {
                    literalNotaFinal.Text = Convert.ToDouble(desempenho.Valor_Pago / 100).ToString("###,###,###.##");
                    totalNotaFinal += Convert.ToDouble(desempenho.Valor_Pago / 100);
                    totalContNotaFinal++;
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
        protected void GridViewDesempenho_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = GridViewDesempenho.FooterRow;

            foreach (GridViewRow item in GridViewDesempenho.Rows)
            {
                totalZeroPeca += Convert.ToDouble(item.Cells[1].Text);
                totalUmaPeca += Convert.ToDouble(item.Cells[3].Text);
                totalDuasPecas += Convert.ToDouble(item.Cells[5].Text);
                totalTresPecas += Convert.ToDouble(item.Cells[7].Text);
                totalQuatroPecas += Convert.ToDouble(item.Cells[9].Text);
                totalCincoPecas += Convert.ToDouble(item.Cells[11].Text);
                totalSemLink += Convert.ToDouble(item.Cells[19].Text);
                //totalNotaFinal += Convert.ToDouble(item.Cells[22].Text);
                totalNumeroDias += Convert.ToDouble(item.Cells[23].Text);
                totalContDias++;
            }

            double total = totalZeroPeca + totalUmaPeca + totalDuasPecas + totalTresPecas + totalQuatroPecas + totalCincoPecas;
            double total_PA = (totalUmaPeca + (totalDuasPecas * 2) + (totalTresPecas * 3) + (totalQuatroPecas * 4) + (totalCincoPecas * 5)) / (total - totalZeroPeca);

            foreach (GridViewRow item in GridViewDesempenho.Rows)
            {
                item.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[1].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[2].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[3].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[4].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[5].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[6].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[7].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[8].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[9].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[10].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[11].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[13].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[14].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[15].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[16].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[17].BackColor = System.Drawing.Color.PeachPuff;

                Literal literalPerc1 = item.FindControl("LiteralPerc1") as Literal;

                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 > (totalUmaPeca / total) * 1.05)
                    item.Cells[4].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 < (totalUmaPeca / total) / 1.05)
                    item.Cells[4].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 > (totalUmaPeca / total) / 1.05 &&
                    Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 < (totalUmaPeca / total) * 1.05)
                    item.Cells[4].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc2 = item.FindControl("LiteralPerc2") as Literal;

                if (Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 > (totalDuasPecas / total) * 1.05)
                    item.Cells[6].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 < (totalDuasPecas / total) / 1.05)
                    item.Cells[6].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 > (totalDuasPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 < (totalDuasPecas / total) * 1.05)
                    item.Cells[6].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc3 = item.FindControl("LiteralPerc3") as Literal;

                if (Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 > (totalTresPecas / total) * 1.05)
                    item.Cells[8].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 < (totalTresPecas / total) / 1.05)
                    item.Cells[8].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 > (totalTresPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 < (totalTresPecas / total) * 1.05)
                    item.Cells[8].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc4 = item.FindControl("LiteralPerc4") as Literal;

                if (Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 > (totalQuatroPecas / total) * 1.05)
                    item.Cells[10].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 < (totalQuatroPecas / total) / 1.05)
                    item.Cells[10].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 > (totalQuatroPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 < (totalQuatroPecas / total) * 1.05)
                    item.Cells[10].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc5 = item.FindControl("LiteralPerc5") as Literal;

                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 > (totalCincoPecas / total) * 1.05)
                    item.Cells[12].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 < (totalCincoPecas / total) / 1.05)
                    item.Cells[12].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 > (totalCincoPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 < (totalCincoPecas / total) * 1.05)
                    item.Cells[12].BackColor = System.Drawing.Color.Yellow;

                nota = 4;

                Literal literalPA = item.FindControl("LiteralPA") as Literal;

                if (Convert.ToDouble(literalPA.Text) > total_PA)
                    item.Cells[13].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[13].BackColor = System.Drawing.Color.Red;
                }

                Literal literalMediaAte = item.FindControl("LiteralMediaAte") as Literal;

                if (Convert.ToDouble(literalMediaAte.Text) > ((total - totalZeroPeca) / totalNumeroDias))
                    item.Cells[14].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[14].BackColor = System.Drawing.Color.Red;
                }
                Literal literalMediaVl = item.FindControl("LiteralMediaVl") as Literal;

                if (Convert.ToDouble(literalMediaVl.Text) > (totalNotaFinal * 100 / (total - totalZeroPeca)))
                    item.Cells[15].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[15].BackColor = System.Drawing.Color.Red;
                }

                Literal literalPercVl = item.FindControl("LiteralPercVl") as Literal;

                if (Convert.ToDouble(literalPercVl.Text) > (total - totalSemLink) / total)
                    item.Cells[16].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[16].BackColor = System.Drawing.Color.Red;
                }

                Literal literalNota = item.FindControl("LiteralNota") as Literal;

                if (literalNota != null)
                {
                    if (nota == 4)
                    {
                        literalNota.Text = "A";
                        item.Cells[17].BackColor = System.Drawing.Color.Green;
                    }
                    if (nota == 3)
                    {
                        literalNota.Text = "B";
                        item.Cells[17].BackColor = System.Drawing.Color.Yellow;
                    }
                    if (nota == 2)
                        literalNota.Text = "B-";
                    if (nota < 2)
                    {
                        literalNota.Text = "C";
                        item.Cells[17].BackColor = System.Drawing.Color.Red;
                    }
                }
            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalZeroPeca.ToString();
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = ((totalZeroPeca / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalUmaPeca.ToString();
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = ((totalUmaPeca / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[5].Text = totalDuasPecas.ToString();
                footer.Cells[5].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].Text = ((totalDuasPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[7].Text = totalTresPecas.ToString();
                footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[8].Text = ((totalTresPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[8].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[9].Text = totalQuatroPecas.ToString();
                footer.Cells[9].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[10].Text = ((totalQuatroPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[10].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[11].Text = totalCincoPecas.ToString();
                footer.Cells[11].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[12].Text = ((totalCincoPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[12].BackColor = System.Drawing.Color.PeachPuff;

                Sp_Desempenho_Venda_LojaResult dvl = desempenhoController.BuscaDesempenhoVendaLoja(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());

                if (dvl != null)
                {
                    footer.Cells[13].Text = (Convert.ToDouble(dvl.Vendas_qt_total + dvl.Vendas_N) / Convert.ToDouble(dvl.Vendas_1 + dvl.Vendas_2 + dvl.Vendas_3 + dvl.Vendas_4 + dvl.Vendas_5)).ToString("N2");
                    //footer.Cells[20].Text = (dvl.Vendas_qt_total + dvl.Vendas_N).ToString();
                    footer.Cells[20].Text = (dvl.Vendas_qt_total + dvl.Vendas_N).ToString();
                }
                else
                {
                    footer.Cells[13].Text = "0";
                    footer.Cells[20].Text = "0";
                }

                //footer.Cells[13].Text = (totalPA / totalContPA).ToString("N2");
                footer.Cells[13].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[14].Text = (totalMedAte / totalContMedAte).ToString("N2");
                footer.Cells[14].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[15].Text = (totalMedVl / totalContMedVl).ToString("N2");
                footer.Cells[15].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[16].Text = (totalVl / totalContVl).ToString("N2");
                footer.Cells[16].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[18].Text = totalComLink.ToString();
                footer.Cells[17].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[18].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[19].Text = totalSemLink.ToString();
                footer.Cells[19].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[20].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[21].Text = totalAte.ToString();
                footer.Cells[21].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[22].Text = totalNotaFinal.ToString("###,###,###.##");
                footer.Cells[22].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[23].Text = (totalNumeroDias / totalContDias).ToString("N2");
                footer.Cells[23].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
        protected void GridViewDesempenho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewDesempenho.PageIndex = e.NewPageIndex;
            CarregaGridViewDesempenho();
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
                        //totalCont1++;
                    }

                    double subTotalPerc11 = Convert.ToDouble(desempenho.Vendas_1) / Convert.ToDouble(total);

                    Literal literalPerc11 = e.Row.FindControl("LiteralPerc11") as Literal;

                    if (literalPerc11 != null)
                    {
                        literalPerc11.Text = (subTotalPerc11 * 100).ToString("N0") + "%";
                        totalPerc11 += subTotalPerc11;
                        //totalCont11++;
                    }

                    double subTotalPerc5 = (Convert.ToDouble(desempenho.Vendas_4) + Convert.ToDouble(desempenho.Vendas_5)) / Convert.ToDouble(total);

                    Literal literalPerc5 = e.Row.FindControl("LiteralPerc5") as Literal;

                    if (literalPerc5 != null)
                    {
                        literalPerc5.Text = (subTotalPerc5 * 100).ToString("N0") + "%";
                        totalPerc5 += subTotalPerc5;
                        //totalCont5++;
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
        #endregion

        #region "GRID VIEW GERAL"
        private void CarregaGridViewGeral()
        {
            USUARIO usuario = (USUARIO)Session["USUARIO"];

            if (usuario != null)
            {
                gvDesempenhoGeral.DataSource = desempenhoController.BuscaDesempenhoFilial(TextBoxDataInicio.Text, TextBoxDataFim.Text, usuario);
                gvDesempenhoGeral.DataBind();
            }
        }
        protected void gvDesempenhoGeral_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Sp_Desempenho_Venda_LojaResult desempenho = e.Row.DataItem as Sp_Desempenho_Venda_LojaResult;
            bool _Zero = false;
            if (desempenho != null)
            {
                Literal literalFilial = e.Row.FindControl("LiteralFilial") as Literal;
                if (literalFilial != null)
                    literalFilial.Text = baseController.BuscaFilialCodigo(Convert.ToInt32(desempenho.Cod_Filial)).FILIAL;

                int? total = desempenho.Vendas_N + desempenho.Vendas_0 + desempenho.Vendas_1 + desempenho.Vendas_2 + desempenho.Vendas_3 + desempenho.Vendas_4 + desempenho.Vendas_5;

                if (total == null || total == 0)
                {
                    total = 1;
                    _Zero = true;
                }

                if (total > 0)
                {
                    _totalVendasPcs += Convert.ToDouble(desempenho.Vendas_qt_total + desempenho.Vendas_N);
                    _totalVendas += Convert.ToDouble(desempenho.Vendas_1 + desempenho.Vendas_2 + desempenho.Vendas_3 + desempenho.Vendas_4 + desempenho.Vendas_5);

                    double subTotalPerc0 = Convert.ToDouble(desempenho.Vendas_0 + desempenho.Vendas_N) / Convert.ToDouble(total);

                    Literal literalPerc0 = e.Row.FindControl("LiteralPerc0") as Literal;

                    if (literalPerc0 != null)
                    {
                        literalPerc0.Text = (subTotalPerc0 * 100).ToString("N0") + "%";
                        totalPerc0 += subTotalPerc0;
                    }

                    double subTotalPerc1 = Convert.ToDouble(desempenho.Vendas_1) / Convert.ToDouble(total);

                    Literal literalPerc1 = e.Row.FindControl("LiteralPerc1") as Literal;

                    if (literalPerc1 != null)
                    {
                        literalPerc1.Text = (subTotalPerc1 * 100).ToString("N0") + "%";
                        totalPerc1 += subTotalPerc1;
                    }

                    double subTotalPerc2 = Convert.ToDouble(desempenho.Vendas_2) / Convert.ToDouble(total);

                    Literal literalPerc2 = e.Row.FindControl("LiteralPerc2") as Literal;

                    if (literalPerc2 != null)
                    {
                        literalPerc2.Text = (subTotalPerc2 * 100).ToString("N0") + "%";
                        totalPerc2 += subTotalPerc2;
                    }

                    double subTotalPerc3 = Convert.ToDouble(desempenho.Vendas_3) / Convert.ToDouble(total);

                    Literal literalPerc3 = e.Row.FindControl("LiteralPerc3") as Literal;

                    if (literalPerc3 != null)
                    {
                        literalPerc3.Text = (subTotalPerc3 * 100).ToString("N0") + "%";
                        totalPerc3 += subTotalPerc3;
                    }

                    double subTotalPerc4 = Convert.ToDouble(desempenho.Vendas_4) / Convert.ToDouble(total);

                    Literal literalPerc4 = e.Row.FindControl("LiteralPerc4") as Literal;

                    if (literalPerc4 != null)
                    {
                        literalPerc4.Text = (subTotalPerc4 * 100).ToString("N0") + "%";
                        totalPerc4 += subTotalPerc4;
                    }

                    double subTotalPerc5 = Convert.ToDouble(desempenho.Vendas_5) / Convert.ToDouble(total);

                    Literal literalPerc5 = e.Row.FindControl("LiteralPerc5") as Literal;

                    if (literalPerc5 != null)
                    {
                        literalPerc5.Text = (subTotalPerc5 * 100).ToString("N0") + "%";
                        totalPerc5 += subTotalPerc5;
                    }

                    double subTotalVl = Convert.ToDouble(total - desempenho.Vendas_Consumidor) / Convert.ToDouble(total);

                    Literal literalPercVl = e.Row.FindControl("LiteralPercVl") as Literal;

                    if (literalPercVl != null)
                    {
                        literalPercVl.Text = (subTotalVl).ToString("N2");
                        totalVl += subTotalVl;
                        totalContVl++;
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

                Literal literalMediaAte = e.Row.FindControl("LiteralMediaAte") as Literal;
                if (literalMediaAte != null)
                {
                    if (desempenho.Numero_Dias > 0)
                    {
                        double subTotalMedAte = Convert.ToDouble(total) / Convert.ToDouble(desempenho.Numero_Dias) / Convert.ToDouble(desempenho.Numero_Vendedor);

                        literalMediaAte.Text = (subTotalMedAte).ToString("N2");
                        totalMedAte += subTotalMedAte;
                        totalContMedAte++;
                    }
                    else
                    {
                        literalMediaAte.Text = "0,00";
                    }
                }

                Literal literalComLink = e.Row.FindControl("LiteralComLink") as Literal;

                if (literalComLink != null)
                {
                    if (!_Zero)
                    {
                        literalComLink.Text = (total - desempenho.Vendas_Consumidor).ToString();
                        totalComLink += Convert.ToDouble(total - desempenho.Vendas_Consumidor);
                    }
                    else
                    {
                        literalComLink.Text = "0";
                    }
                }

                Literal literalSemLink = e.Row.FindControl("LiteralSemLink") as Literal;

                if (literalSemLink != null)
                {
                    literalSemLink.Text = desempenho.Vendas_Consumidor.ToString();
                    totalSemLink += Convert.ToDouble(desempenho.Vendas_Consumidor);
                }

                int? subTotalPcs = desempenho.Vendas_qt_total + desempenho.Vendas_N;
                Literal literalTotalPcs = e.Row.FindControl("LiteralTotalPcs") as Literal;
                if (literalTotalPcs != null)
                {
                    literalTotalPcs.Text = (subTotalPcs).ToString();
                    //SOMATORIO DO TOTAL DE PEÇAS
                    totalPcs += subTotalPcs;
                }

                double subTotalAte = 0;
                if (!_Zero)
                    subTotalAte = Convert.ToDouble(total);

                Literal literalTotalAte = e.Row.FindControl("LiteralTotalAte") as Literal;

                if (literalTotalAte != null)
                {
                    literalTotalAte.Text = (subTotalAte).ToString();
                    totalAte += subTotalAte;
                }

                Literal literalNotaFinal = e.Row.FindControl("LiteralNotaFinal") as Literal;

                if (literalNotaFinal != null)
                {
                    literalNotaFinal.Text = Convert.ToDouble(desempenho.Valor_Pago / 100).ToString("###,###,##0.00");
                    totalNotaFinal += Convert.ToDouble(desempenho.Valor_Pago / 100);
                    totalContNotaFinal++;
                }

                Literal literalRanking = e.Row.FindControl("LiteralRanking") as Literal;

                if (literalRanking != null)
                {
                    List<Sp_Ranking_Venda_LojaResult> _sp_Ranking_VendaLoja = desempenhoController.BuscaDesempenhoFilial(TextBoxDataInicio.Text, TextBoxDataFim.Text);

                    if (_sp_Ranking_VendaLoja != null)
                    {
                        int i = 0;

                        foreach (Sp_Ranking_Venda_LojaResult ranking in _sp_Ranking_VendaLoja)
                        {
                            i++;

                            if (ranking.Filial.ToUpper().Trim().Equals(literalFilial.Text.ToUpper().Trim()))
                            {
                                literalRanking.Text = i.ToString();

                                break;
                            }
                        }
                    }
                }
            }
        }
        protected void gvDesempenhoGeral_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvDesempenhoGeral.FooterRow;

            foreach (GridViewRow item in gvDesempenhoGeral.Rows)
            {
                totalZeroPeca += Convert.ToDouble(item.Cells[1].Text);
                totalUmaPeca += Convert.ToDouble(item.Cells[3].Text);
                totalDuasPecas += Convert.ToDouble(item.Cells[5].Text);
                totalTresPecas += Convert.ToDouble(item.Cells[7].Text);
                totalQuatroPecas += Convert.ToDouble(item.Cells[9].Text);
                totalCincoPecas += Convert.ToDouble(item.Cells[11].Text);
                totalSemLink += Convert.ToDouble(item.Cells[19].Text);
                //totalNotaFinal += Convert.ToDouble(item.Cells[22].Text);
                totalNumeroDias += Convert.ToDouble(item.Cells[23].Text);
                totalContDias++;
            }

            double total = totalZeroPeca + totalUmaPeca + totalDuasPecas + totalTresPecas + totalQuatroPecas + totalCincoPecas;
            double total_PA = (totalUmaPeca + (totalDuasPecas * 2) + (totalTresPecas * 3) + (totalQuatroPecas * 4) + (totalCincoPecas * 5)) / (total - totalZeroPeca);

            foreach (GridViewRow item in gvDesempenhoGeral.Rows)
            {
                item.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[1].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[2].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[3].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[4].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[5].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[6].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[7].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[8].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[9].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[10].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[11].BackColor = System.Drawing.Color.LightSteelBlue;
                item.Cells[13].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[14].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[15].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[16].BackColor = System.Drawing.Color.PeachPuff;
                item.Cells[17].BackColor = System.Drawing.Color.PeachPuff;

                Literal literalPerc1 = item.FindControl("LiteralPerc1") as Literal;

                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 > (totalUmaPeca / total) * 1.05)
                    item.Cells[4].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 < (totalUmaPeca / total) / 1.05)
                    item.Cells[4].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 > (totalUmaPeca / total) / 1.05 &&
                    Convert.ToDouble(literalPerc1.Text.Substring(0, literalPerc1.Text.Length - 1)) / 100 < (totalUmaPeca / total) * 1.05)
                    item.Cells[4].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc2 = item.FindControl("LiteralPerc2") as Literal;

                if (Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 > (totalDuasPecas / total) * 1.05)
                    item.Cells[6].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 < (totalDuasPecas / total) / 1.05)
                    item.Cells[6].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 > (totalDuasPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc2.Text.Substring(0, literalPerc2.Text.Length - 1)) / 100 < (totalDuasPecas / total) * 1.05)
                    item.Cells[6].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc3 = item.FindControl("LiteralPerc3") as Literal;

                if (Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 > (totalTresPecas / total) * 1.05)
                    item.Cells[8].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 < (totalTresPecas / total) / 1.05)
                    item.Cells[8].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 > (totalTresPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc3.Text.Substring(0, literalPerc3.Text.Length - 1)) / 100 < (totalTresPecas / total) * 1.05)
                    item.Cells[8].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc4 = item.FindControl("LiteralPerc4") as Literal;

                if (Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 > (totalQuatroPecas / total) * 1.05)
                    item.Cells[10].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 < (totalQuatroPecas / total) / 1.05)
                    item.Cells[10].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 > (totalQuatroPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc4.Text.Substring(0, literalPerc4.Text.Length - 1)) / 100 < (totalQuatroPecas / total) * 1.05)
                    item.Cells[10].BackColor = System.Drawing.Color.Yellow;

                Literal literalPerc5 = item.FindControl("LiteralPerc5") as Literal;

                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 > (totalCincoPecas / total) * 1.05)
                    item.Cells[12].BackColor = System.Drawing.Color.Green;
                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 < (totalCincoPecas / total) / 1.05)
                    item.Cells[12].BackColor = System.Drawing.Color.Red;
                if (Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 > (totalCincoPecas / total) / 1.05 &&
                    Convert.ToDouble(literalPerc5.Text.Substring(0, literalPerc5.Text.Length - 1)) / 100 < (totalCincoPecas / total) * 1.05)
                    item.Cells[12].BackColor = System.Drawing.Color.Yellow;

                nota = 4;

                Literal literalPA = item.FindControl("LiteralPA") as Literal;

                if (Convert.ToDouble(literalPA.Text) > total_PA)
                    item.Cells[13].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[13].BackColor = System.Drawing.Color.Red;
                }

                Literal literalMediaAte = item.FindControl("LiteralMediaAte") as Literal;

                if (Convert.ToDouble(literalMediaAte.Text) > ((total - totalZeroPeca) / totalNumeroDias))
                    item.Cells[14].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[14].BackColor = System.Drawing.Color.Red;
                }
                Literal literalMediaVl = item.FindControl("LiteralMediaVl") as Literal;

                if (Convert.ToDouble(literalMediaVl.Text) > (totalNotaFinal * 100 / (total - totalZeroPeca)))
                    item.Cells[15].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[15].BackColor = System.Drawing.Color.Red;
                }

                Literal literalPercVl = item.FindControl("LiteralPercVl") as Literal;

                if (Convert.ToDouble(literalPercVl.Text) > (total - totalSemLink) / total)
                    item.Cells[16].BackColor = System.Drawing.Color.Green;
                else
                {
                    nota--;
                    item.Cells[16].BackColor = System.Drawing.Color.Red;
                }

                Literal literalNota = item.FindControl("LiteralNota") as Literal;

                if (literalNota != null)
                {
                    if (nota == 4)
                    {
                        literalNota.Text = "A";
                        item.Cells[17].BackColor = System.Drawing.Color.Green;
                    }
                    if (nota == 3)
                    {
                        literalNota.Text = "B";
                        item.Cells[17].BackColor = System.Drawing.Color.Yellow;
                    }
                    if (nota == 2)
                        literalNota.Text = "B-";
                    if (nota < 2)
                    {
                        literalNota.Text = "C";
                        item.Cells[17].BackColor = System.Drawing.Color.Red;
                    }
                }

            }

            if (footer != null)
            {
                footer.Cells[0].Text = "Totais";
                footer.Cells[0].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[1].Text = totalZeroPeca.ToString();
                footer.Cells[1].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[2].Text = ((totalZeroPeca / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[2].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[3].Text = totalUmaPeca.ToString();
                footer.Cells[3].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[4].Text = ((totalUmaPeca / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[4].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[5].Text = totalDuasPecas.ToString();
                footer.Cells[5].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[6].Text = ((totalDuasPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[6].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[7].Text = totalTresPecas.ToString();
                footer.Cells[7].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[8].Text = ((totalTresPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[8].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[9].Text = totalQuatroPecas.ToString();
                footer.Cells[9].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[10].Text = ((totalQuatroPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[10].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[11].Text = totalCincoPecas.ToString();
                footer.Cells[11].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[12].Text = ((totalCincoPecas / totalAte) * 100).ToString("N0") + "%";
                footer.Cells[12].BackColor = System.Drawing.Color.PeachPuff;

                footer.Cells[13].Text = (_totalVendasPcs / _totalVendas).ToString("N2");
                footer.Cells[20].Text = totalPcs.ToString();

                //footer.Cells[13].Text = (totalPA / totalContPA).ToString("N2");
                footer.Cells[13].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[14].Text = (totalMedAte / totalContMedAte).ToString("N2");
                footer.Cells[14].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[15].Text = (totalMedVl / totalContMedVl).ToString("N2");
                footer.Cells[15].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[16].Text = (totalVl / totalContVl).ToString("N2");
                footer.Cells[16].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[18].Text = totalComLink.ToString();
                footer.Cells[17].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[18].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[19].Text = totalSemLink.ToString();
                footer.Cells[19].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[20].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[21].Text = totalAte.ToString();
                footer.Cells[21].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[22].Text = totalNotaFinal.ToString("###,###,###.##");
                footer.Cells[22].BackColor = System.Drawing.Color.PeachPuff;
                footer.Cells[23].Text = (totalNumeroDias / totalContDias).ToString("N2");
                footer.Cells[23].BackColor = System.Drawing.Color.PeachPuff;
            }
        }
        #endregion

    }
}
