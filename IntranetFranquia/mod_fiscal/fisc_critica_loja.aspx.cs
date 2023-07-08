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
    public partial class fisc_critica_loja : System.Web.UI.Page
    {
        CriticaController criticaController = new CriticaController();
        BaseController baseController = new BaseController();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void CalendarDataInicio_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataInicio.Text = CalendarDataInicio.SelectedDate.ToString("dd/MM/yyyy");
        }
        protected void CalendarDataFim_SelectionChanged(object sender, EventArgs e)
        {
            TextBoxDataFim.Text = CalendarDataFim.SelectedDate.ToString("dd/MM/yyyy");
        }

        protected void btPesquisar_Click(object sender, EventArgs e)
        {
            if (TextBoxDataInicio.Text.Equals("") || TextBoxDataInicio.Text == null || TextBoxDataFim.Text.Equals("") || TextBoxDataFim.Text == null)
                return;

            List<ReportCriticaLoja> listReportCriticaLoja = new List<ReportCriticaLoja>();

            // Diversas compras no dia, na mesma loja

            DateTime dataInicio = CalendarDataInicio.SelectedDate;
            DateTime dataFim = CalendarDataFim.SelectedDate;

            do
            {
                List<Sp_Critica_Loja_DiaResult> listCriticaLojaDia = criticaController.BuscaCriticaLojaDia(baseController.AjustaData(dataInicio.ToString("dd/MM/yyyy")));

                CLIENTES_VAREJO cliente = null;
                FILIAI filial = null;

                foreach (Sp_Critica_Loja_DiaResult cld in listCriticaLojaDia)
                {
                    List<Sp_Busca_Vendas_DiaResult> listaVendas = criticaController.BuscaVendasDia(cld.cpf_cgc_ecf, baseController.AjustaData(dataInicio.ToString("dd/MM/yyyy")));

                    foreach (Sp_Busca_Vendas_DiaResult lv in listaVendas)
                    {
                        ReportCriticaLoja cl = new ReportCriticaLoja();

                        if (!lv.ticket.Substring(0, 1).Equals("P"))
                        {
                            if (lv.valor_pago > 0)
                            {
                                cliente = baseController.BuscaClienteCPF(cld.cpf_cgc_ecf);

                                if (cliente == null || cliente.CPF_CGC.Trim().Equals(""))
                                {
                                    cl.Nome_cliente = "Sem Identificação";
                                    cl.Cpf_cliente = "0";
                                    cl.Numero_nf = lv.ticket;
                                    cl.Data_nf = dataInicio.ToString();
                                    cl.Valor_nf = lv.valor_pago.ToString();
                                    cl.Codigo_loja = lv.codigo_filial;

                                    filial = baseController.BuscaFilialCodigo(Convert.ToInt32(lv.codigo_filial));

                                    if (filial == null)
                                        cl.Descricao_loja = "Sem Identificação";
                                    else
                                        cl.Descricao_loja = filial.FILIAL;

                                    cl.Cor = "1";

                                    listReportCriticaLoja.Add(cl);
                                }
                                else
                                {
                                    if (!cliente.CLIENTE_VAREJO.Trim().Equals("FUNCIONARIO") &&
                                        !cliente.CLIENTE_VAREJO.Trim().Equals("CONSUMIDOR"))
                                    {
                                        cl.Nome_cliente = cliente.CLIENTE_VAREJO;
                                        cl.Cpf_cliente = cliente.CPF_CGC;
                                        cl.Numero_nf = lv.ticket;
                                        cl.Data_nf = dataInicio.ToString();
                                        cl.Valor_nf = lv.valor_pago.ToString();
                                        cl.Codigo_loja = lv.codigo_filial;

                                        filial = baseController.BuscaFilialCodigo(Convert.ToInt32(lv.codigo_filial));

                                        if (filial == null)
                                            cl.Descricao_loja = "Sem Identificação";
                                        else
                                            cl.Descricao_loja = filial.FILIAL;

                                        cl.Cor = "1";

                                        listReportCriticaLoja.Add(cl);
                                    }
                                }
                            }
                        }
                    }
                }

                dataInicio = dataInicio.AddDays(1);
            }

            while (dataInicio <= dataFim);

            CarregaGridViewCritica(listReportCriticaLoja);
        }
        private void CarregaGridViewCritica(List<ReportCriticaLoja> cl)
        {
            GridViewCritica.DataSource = cl;
            GridViewCritica.DataBind();
        }

        protected void GridViewCritica_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.BackColor = System.Drawing.Color.CadetBlue;

            ReportCriticaLoja cld = e.Row.DataItem as ReportCriticaLoja;

            if (cld != null)
            {
                Literal literalNumeroNF = e.Row.FindControl("LiteralNumeroNF") as Literal;

                if (literalNumeroNF != null)
                    literalNumeroNF.Text = cld.Numero_nf;

                Literal literalDataNF = e.Row.FindControl("LiteralDataNF") as Literal;

                if (literalDataNF != null)
                    literalDataNF.Text = cld.Data_nf;

                Literal literalValorNF = e.Row.FindControl("LiteralValorNF") as Literal;

                if (literalValorNF != null)
                    literalValorNF.Text = cld.Valor_nf;

                Literal literalNomeCliente = e.Row.FindControl("LiteralNomeCliente") as Literal;

                if (literalNomeCliente != null)
                    literalNomeCliente.Text = cld.Nome_cliente;

                Literal literalCpf = e.Row.FindControl("LiteralCpf") as Literal;

                if (literalCpf != null)
                    literalCpf.Text = cld.Cpf_cliente;

                Literal literalCodigoLoja = e.Row.FindControl("LiteralCodigoLoja") as Literal;

                if (literalCodigoLoja != null)
                    literalCodigoLoja.Text = cld.Codigo_loja;

                Literal literalDescricaoLoja = e.Row.FindControl("LiteralDescricaoLoja") as Literal;

                if (literalDescricaoLoja != null)
                    literalDescricaoLoja.Text = cld.Descricao_loja;

                if (cld.Cor.Equals("1"))
                    e.Row.BackColor = System.Drawing.Color.Pink;

                if (cld.Cor.Equals("2"))
                    e.Row.BackColor = System.Drawing.Color.Moccasin;
            }
        }
    }
}
