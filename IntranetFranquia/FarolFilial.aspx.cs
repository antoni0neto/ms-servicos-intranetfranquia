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
    public partial class FarolFilial : System.Web.UI.Page
    {
        BaseController baseController = new BaseController();
        DesempenhoController desempenhoController = new DesempenhoController();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void CarregaGridViewMovimentoLoja()
        {
            List<FILIAI> filiais = null;

            if (Session["USUARIO"] == null)
                Response.Redirect("~/Login.aspx");
            else
            {
                USUARIO usuario = (USUARIO)Session["USUARIO"];

                filiais = baseController.BuscaFiliais(usuario);
            }

            if (filiais != null)
            {
                List<DAL.Farol> farol = baseController.BuscaMovimentosLoja(filiais, txtData.Text);

                Repeater1.DataSource = farol;
                Repeater1.DataBind();
            }
        }

        protected void btBuscarMovimento_Click(object sender, EventArgs e)
        {
            if (txtData.Text.Equals(""))
                return;

            CarregaGridViewMovimentoLoja();
        }

        protected void CalendarData_SelectionChanged(object sender, EventArgs e)
        {
            txtData.Text = CalendarData.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DAL.Farol farol = e.Item.DataItem as DAL.Farol;

            if (farol != null)
            {
                Literal literalFilial = e.Item.FindControl("LiteralFilial") as Literal;

                if (literalFilial != null)
                    literalFilial.Text = farol.Loja;

                Literal literalVlCotaDia = e.Item.FindControl("LiteralVlCotaDia") as Literal;

                if (literalVlCotaDia != null)
                    literalVlCotaDia.Text = farol.Vl_cota_dia;

                Literal literalVlVendaDia = e.Item.FindControl("LiteralVlVendaDia") as Literal;

                if (literalVlVendaDia != null)
                    literalVlVendaDia.Text = farol.Vl_venda_dia;

                Literal literalPercAtendidoDia = e.Item.FindControl("LiteralPercAtendidoDia") as Literal;

                if (literalPercAtendidoDia != null)
                    literalPercAtendidoDia.Text = farol.Perc_at_dia;

                Literal literalVlCotaDiaAnoAnt = e.Item.FindControl("LiteralVlCotaDiaAnoAnt") as Literal;

                if (literalVlCotaDiaAnoAnt != null)
                    literalVlCotaDiaAnoAnt.Text = farol.Vl_cota_dia_ano_ant;

                Literal literalVlVendaDiaAnoAnt = e.Item.FindControl("LiteralVlVendaDiaAnoAnt") as Literal;

                if (literalVlVendaDiaAnoAnt != null)
                    literalVlVendaDiaAnoAnt.Text = farol.Vl_venda_dia_ano_ant;

                Literal literalPercAtendidoDiaAnoAnt = e.Item.FindControl("LiteralPercAtendidoDiaAnoAnt") as Literal;

                if (literalPercAtendidoDiaAnoAnt != null)
                    literalPercAtendidoDiaAnoAnt.Text = farol.Perc_at_dia_ano_ant;

                Literal literalPercAtendidoDiaCota = e.Item.FindControl("LiteralPercAtendidoDiaCota") as Literal;

                if (literalPercAtendidoDiaCota != null)
                    literalPercAtendidoDiaCota.Text = farol.Perc_at_dia_cota;

                Literal literalPercAtendidoDiaVenda = e.Item.FindControl("LiteralPercAtendidoDiaVenda") as Literal;

                if (literalPercAtendidoDiaVenda != null)
                    literalPercAtendidoDiaVenda.Text = farol.Perc_at_dia_venda;

                Literal literalVlCotaSem = e.Item.FindControl("LiteralVlCotaSem") as Literal;

                if (literalVlCotaSem != null)
                    literalVlCotaSem.Text = farol.Vl_cota_sem;

                Literal literalVlVendaSem = e.Item.FindControl("LiteralVlVendaSem") as Literal;

                if (literalVlVendaSem != null)
                    literalVlVendaSem.Text = farol.Vl_venda_sem;

                Literal literalPercAtendidoSem = e.Item.FindControl("LiteralPercAtendidoSem") as Literal;

                if (literalPercAtendidoSem != null)
                    literalPercAtendidoSem.Text = farol.Perc_at_sem;

                Literal literalVlCotaSemAnoAnt = e.Item.FindControl("LiteralVlCotaSemAnoAnt") as Literal;

                if (literalVlCotaSemAnoAnt != null)
                    literalVlCotaSemAnoAnt.Text = farol.Vl_cota_sem_ano_ant;

                Literal literalVlVendaSemAnoAnt = e.Item.FindControl("LiteralVlVendaSemAnoAnt") as Literal;

                if (literalVlVendaSemAnoAnt != null)
                    literalVlVendaSemAnoAnt.Text = farol.Vl_venda_sem_ano_ant;

                Literal literalPercAtendidoSemAnoAnt = e.Item.FindControl("LiteralPercAtendidoSemAnoAnt") as Literal;

                if (literalPercAtendidoSemAnoAnt != null)
                    literalPercAtendidoSemAnoAnt.Text = farol.Perc_at_sem_ano_ant;

                Literal literalPercAtendidoSemCota = e.Item.FindControl("LiteralPercAtendidoSemCota") as Literal;

                if (literalPercAtendidoSemCota != null)
                    literalPercAtendidoSemCota.Text = farol.Perc_at_sem_cota;
                
                Literal literalPercAtendidoSemVenda = e.Item.FindControl("LiteralPercAtendidoSemVenda") as Literal;

                if (literalPercAtendidoSemVenda != null)
                    literalPercAtendidoSemVenda.Text = farol.Perc_at_sem_venda;

                Literal literalVlCotaMes = e.Item.FindControl("LiteralVlCotaMes") as Literal;

                if (literalVlCotaMes != null)
                    literalVlCotaMes.Text = farol.Vl_cota_mes;

                Literal literalVlVendaMes = e.Item.FindControl("LiteralVlVendaMes") as Literal;

                if (literalVlVendaMes != null)
                    literalVlVendaMes.Text = farol.Vl_venda_mes;

                Literal literalPercAtendidoMes = e.Item.FindControl("LiteralPercAtendidoMes") as Literal;

                if (literalPercAtendidoMes != null)
                    literalPercAtendidoMes.Text = farol.Perc_at_mes;

                Literal literalVlCotaMesAnoAnt = e.Item.FindControl("LiteralVlCotaMesAnoAnt") as Literal;

                if (literalVlCotaMesAnoAnt != null)
                    literalVlCotaMesAnoAnt.Text = farol.Vl_cota_mes_ano_ant;

                Literal literalVlVendaMesAnoAnt = e.Item.FindControl("LiteralVlVendaMesAnoAnt") as Literal;

                if (literalVlVendaMesAnoAnt != null)
                    literalVlVendaMesAnoAnt.Text = farol.Vl_venda_mes_ano_ant;

                Literal literalPercAtendidoMesAnoAnt = e.Item.FindControl("LiteralPercAtendidoMesAnoAnt") as Literal;

                if (literalPercAtendidoMesAnoAnt != null)
                    literalPercAtendidoMesAnoAnt.Text = farol.Perc_at_mes_ano_ant;

                Literal literalPercAtendidoMesCota = e.Item.FindControl("LiteralPercAtendidoMesCota") as Literal;

                if (literalPercAtendidoMesCota != null)
                    literalPercAtendidoMesCota.Text = farol.Perc_at_mes_cota;

                Literal literalPercAtendidoMesVenda = e.Item.FindControl("LiteralPercAtendidoMesVenda") as Literal;

                if (literalPercAtendidoMesVenda != null)
                    literalPercAtendidoMesVenda.Text = farol.Perc_at_mes_venda;

                Literal literalVlVendaTri = e.Item.FindControl("LiteralVlVendaTri") as Literal;

                if (literalVlVendaTri != null)
                    literalVlVendaTri.Text = farol.Vl_venda_tri;

                Literal literalVlVendaTriAnoAnt = e.Item.FindControl("LiteralVlVendaTriAnoAnt") as Literal;

                if (literalVlVendaTriAnoAnt != null)
                    literalVlVendaTriAnoAnt.Text = farol.Vl_venda_tri_ano_ant;

                Literal literalPercVendaAtingidoTri = e.Item.FindControl("LiteralPercVendaAtingidoTri") as Literal;

                if (literalPercVendaAtingidoTri != null)
                    literalPercVendaAtingidoTri.Text = farol.Perc_venda_tri_at_ano;

                Literal literalQtCotaDia = e.Item.FindControl("LiteralQtCotaDia") as Literal;

                if (literalQtCotaDia != null)
                    literalQtCotaDia.Text = farol.Qt_cota_dia.ToString("###,###");

                Literal literalQtVendaDia = e.Item.FindControl("LiteralQtVendaDia") as Literal;

                if (literalQtVendaDia != null)
                    literalQtVendaDia.Text = farol.Qt_venda_dia.ToString("###,###");

                Literal literalQtCotaDiaAnoAnt = e.Item.FindControl("LiteralQtCotaDiaAnoAnt") as Literal;

                if (literalQtCotaDiaAnoAnt != null)
                    literalQtCotaDiaAnoAnt.Text = farol.Qt_cota_dia_ano_ant.ToString("###,###");

                Literal literalQtVendaDiaAnoAnt = e.Item.FindControl("LiteralQtVendaDiaAnoAnt") as Literal;

                if (literalQtVendaDiaAnoAnt != null)
                    literalQtVendaDiaAnoAnt.Text = farol.Qt_venda_dia_ano_ant.ToString("###,###");
                
                Literal literalPercQtAtendidoDia = e.Item.FindControl("LiteralPercQtAtendidoDia") as Literal;

                if (literalPercQtAtendidoDia != null)
                    literalPercQtAtendidoDia.Text = farol.Perc_qt_atendido_dia;
                
                Literal literalQtCotaSem = e.Item.FindControl("LiteralQtCotaSem") as Literal;

                if (literalQtCotaSem != null)
                    literalQtCotaSem.Text = farol.Qt_cota_sem.ToString("###,###");

                Literal literalQtVendaSem = e.Item.FindControl("LiteralQtVendaSem") as Literal;

                if (literalQtVendaSem != null)
                    literalQtVendaSem.Text = farol.Qt_venda_sem.ToString("###,###");

                Literal literalQtCotaSemAnoAnt = e.Item.FindControl("LiteralQtCotaSemAnoAnt") as Literal;

                if (literalQtCotaSemAnoAnt != null)
                    literalQtCotaSemAnoAnt.Text = farol.Qt_cota_sem_ano_ant.ToString("###,###");

                Literal literalQtVendaSemAnoAnt = e.Item.FindControl("LiteralQtVendaSemAnoAnt") as Literal;

                if (literalQtVendaSemAnoAnt != null)
                    literalQtVendaSemAnoAnt.Text = farol.Qt_venda_sem_ano_ant.ToString("###,###");
                
                Literal literalPercQtAtendidoSem = e.Item.FindControl("LiteralPercQtAtendidoSem") as Literal;

                if (literalPercQtAtendidoSem != null)
                    literalPercQtAtendidoSem.Text = farol.Perc_qt_atendido_sem;

                Literal literalQtCotaMes = e.Item.FindControl("LiteralQtCotaMes") as Literal;

                if (literalQtCotaMes != null)
                    literalQtCotaMes.Text = farol.Qt_cota_mes.ToString("###,###");

                Literal literalQtVendaMes = e.Item.FindControl("LiteralQtVendaMes") as Literal;

                if (literalQtVendaMes != null)
                    literalQtVendaMes.Text = farol.Qt_venda_mes.ToString("###,###");

                Literal literalQtCotaMesAnoAnt = e.Item.FindControl("LiteralQtCotaMesAnoAnt") as Literal;

                if (literalQtCotaMesAnoAnt != null)
                    literalQtCotaMesAnoAnt.Text = farol.Qt_cota_mes_ano_ant.ToString("###,###");

                Literal literalQtVendaMesAnoAnt = e.Item.FindControl("LiteralQtVendaMesAnoAnt") as Literal;

                if (literalQtVendaMesAnoAnt != null)
                    literalQtVendaMesAnoAnt.Text = farol.Qt_venda_mes_ano_ant.ToString("###,###");
                
                Literal literalPercQtAtendidoMes = e.Item.FindControl("LiteralPercQtAtendidoMes") as Literal;

                if (literalPercQtAtendidoMes != null)
                    literalPercQtAtendidoMes.Text = farol.Perc_qt_atendido_mes;

                Literal literalVlCotaTri = e.Item.FindControl("LiteralVlCotaTri") as Literal;

                if (literalVlCotaTri != null)
                    literalVlCotaTri.Text = farol.Vl_cota_tri;

                Literal literalVlCotaTriAnoAnt = e.Item.FindControl("LiteralVlCotaTriAnoAnt") as Literal;

                if (literalVlCotaTriAnoAnt != null)
                    literalVlCotaTriAnoAnt.Text = farol.Vl_cota_tri_ano_ant;

                Literal literalPercCotaAtingidoTri = e.Item.FindControl("LiteralPercCotaAtingidoTri") as Literal;

                if (literalPercCotaAtingidoTri != null)
                    literalPercCotaAtingidoTri.Text = farol.Perc_cota_tri_at_ano;

                Literal literalVlMedioDia = e.Item.FindControl("LiteralVlMedioDia") as Literal;

                if (literalVlMedioDia != null)
                    literalVlMedioDia.Text = farol.Vl_medio_dia;

                Literal literalVlMedioDiaAnoAnt = e.Item.FindControl("LiteralVlMedioDiaAnoAnt") as Literal;

                if (literalVlMedioDiaAnoAnt != null)
                    literalVlMedioDiaAnoAnt.Text = farol.Vl_medio_dia_ano_ant;
                
                Literal literalVlMedioDiaAtingido = e.Item.FindControl("LiteralVlMedioDiaAtingido") as Literal;

                if (literalVlMedioDiaAtingido != null)
                    literalVlMedioDiaAtingido.Text = farol.Perc_vl_medio_atendido_dia;

                Literal literalVlMedioSem = e.Item.FindControl("LiteralVlMedioSem") as Literal;

                if (literalVlMedioSem != null)
                    literalVlMedioSem.Text = farol.Vl_medio_sem;

                Literal literalVlMedioSemAnoAnt = e.Item.FindControl("LiteralVlMedioSemAnoAnt") as Literal;

                if (literalVlMedioSemAnoAnt != null)
                    literalVlMedioSemAnoAnt.Text = farol.Vl_medio_sem_ano_ant;
                
                Literal literalVlMedioSemAtingido = e.Item.FindControl("LiteralVlMedioSemAtingido") as Literal;

                if (literalVlMedioSemAtingido != null)
                    literalVlMedioSemAtingido.Text = farol.Perc_vl_medio_atendido_sem;
                
                Literal literalVlMedioMes = e.Item.FindControl("LiteralVlMedioMes") as Literal;

                if (literalVlMedioMes != null)
                    literalVlMedioMes.Text = farol.Vl_medio_mes;

                Literal literalVlMedioMesAnoAnt = e.Item.FindControl("LiteralVlMedioMesAnoAnt") as Literal;

                if (literalVlMedioMesAnoAnt != null)
                    literalVlMedioMesAnoAnt.Text = farol.Vl_medio_mes_ano_ant;

                Literal literalVlMedioMesAtingido = e.Item.FindControl("LiteralVlMedioMesAtingido") as Literal;

                if (literalVlMedioMesAtingido != null)
                    literalVlMedioMesAtingido.Text = farol.Perc_vl_medio_atendido_mes;
                

                Literal literalPercAtingidoTri = e.Item.FindControl("LiteralPercAtingidoTri") as Literal;

                if (literalPercAtingidoTri != null)
                    literalPercAtingidoTri.Text = farol.Perc_venda_cota_tri;

                Literal literalPercAtingidoTriAnoAnt = e.Item.FindControl("LiteralPercAtingidoTriAnoAnt") as Literal;

                if (literalPercAtingidoTriAnoAnt != null)
                    literalPercAtingidoTriAnoAnt.Text = farol.Perc_venda_cota_tri_ano_ant;
                
            }
        }
    }
}
