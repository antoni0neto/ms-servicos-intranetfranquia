using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Relatorios
{
    public partial class gerloja_analise_atendimento : System.Web.UI.Page
    {
        SaidaProdutoController saidaProdutoController = new SaidaProdutoController();
        BaseController baseController = new BaseController();

        decimal totalValor = 0;
        int totalTicket = 0;

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

                var filiais = baseController.BuscaFiliais(usuario);

                filiais.Insert(0, new FILIAI { COD_FILIAL = "0", FILIAL = "Selecione" });
                ddlFilial.DataSource = filiais;
                ddlFilial.DataBind();

            }
        }

        private List<SP_Atendimento_LojaResult> ObterAtendimentoLoja()
        {
            var ate = saidaProdutoController.BuscaAtendimentosLoja(TextBoxDataInicio.Text, TextBoxDataFim.Text, ddlFilial.SelectedValue.ToString());

            return ate;
        }

        private void CarregaGridViewVendas()
        {
            gvVendasHora.DataSource = ObterAtendimentoLoja();
            gvVendasHora.DataBind();
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btPesquisarVendas_Click(object sender, EventArgs e)
        {

            try
            {

                if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                    return;

                if (ddlFilial.SelectedValue.ToString().Equals("0") || ddlFilial.SelectedValue.ToString().Equals(""))
                    return;

                CarregaGridViewVendas();
                GerarGrafico();

            }
            catch (Exception ex)
            {

            }
        }
        private void GerarGrafico()
        {
            var tb_vendas = ObterAtendimentoLoja();

            int qtde_linhas = tb_vendas.Count;
            //cria os arrays para alimentar o gráfico 
            string[] valoresx = new string[qtde_linhas];
            int[] valoresy = new int[qtde_linhas];
            for (int vx = 0; qtde_linhas > vx; vx++)
            {
                valoresx[vx] = Convert.ToString(tb_vendas[vx].HORA);
                valoresy[vx] = Convert.ToInt32(tb_vendas[vx].VALOR);

            }
            //fim do For 
            //insere os valores dos array no gráficos 

            chartHora.Series["serieHora"].Points.DataBindXY(valoresx, valoresy);

            chartHora.DataBind();

            //LimpaTela();
        }

        protected void gvVendasHora_DataBound(object sender, EventArgs e)
        {
            GridViewRow footer = gvVendasHora.FooterRow;

            foreach (GridViewRow item in gvVendasHora.Rows)
            {
                if (item.Cells[2].Text.Equals(""))
                    item.Cells[2].Text = "0";

                if (item.Cells[4].Text.Equals(""))
                    item.Cells[4].Text = "0";

                totalValor += Convert.ToDecimal(item.Cells[2].Text);
                totalTicket += Convert.ToInt32(item.Cells[4].Text);
            }

            if (footer != null)
            {
                footer.Cells[1].Text = "Total";
                footer.Cells[2].Text = totalValor.ToString();
                footer.Cells[4].Text = totalTicket.ToString();
            }
        }

    }
}
