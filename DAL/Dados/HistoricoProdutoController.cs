using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class HistoricoProdutoController
    {
        BaseController baseController = new BaseController();

        public List<LineReportColecao> BuscaMovimentoColecao(string categoria, string colecao, string anoSemana, string griffe, string colecaoAnoAnterior)
        {
            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);

            string ano = buscaData.ANO454;
            string semana = anoSemana.Substring(4, 2);

            string dataInicioSemana = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(baseController.BuscaDataFim(anoSemana));

            string dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMes = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            string dataInicioMesFechado = "";
            string dataFimMesFechado = "";

            if (mes < 4)
            {
                if (mes == 1)
                {
                    dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(12, (Convert.ToInt32(ano) - 2).ToString()));
                    dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(12, (Convert.ToInt32(ano) - 2).ToString()));
                }
                else
                {
                    dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(mes - 1, (Convert.ToInt32(ano) - 1).ToString()));
                    dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(mes - 1, (Convert.ToInt32(ano) - 1).ToString()));
                }
            }
            else
            {
                dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(mes - 1, ano));
                dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(mes - 1, ano));
            }

            string dataInicioSemanaAnoAnt = AjustaData(baseController.BuscaDataInicio((Convert.ToInt32(ano) - 1).ToString() + semana));
            string dataFimSemanaAnoAnt = AjustaData(baseController.BuscaDataFim((Convert.ToInt32(ano) - 1).ToString() + semana));

            string dataInicioMesAnoAnt = AjustaData(baseController.BuscaDataInicioMes(mes, (Convert.ToInt32(ano) - 1).ToString()));
            string dataFimMesAnoAnt = AjustaData(baseController.BuscaDataFimMes(mes, (Convert.ToInt32(ano) - 1).ToString()));

            decimal total_vl_venda_semana = 0;
            decimal total_vl_venda_mes = 0;
            decimal total_vl_venda_semana_ano_ant = 0;
            decimal total_vl_venda_mes_ano_ant = 0;
            decimal total_var_markup_liquido = 0;
            decimal total_var_markup_original = 0;
            decimal total_var_markup_liquido_antigo = 0;
            decimal total_var_markup_original_antigo = 0;
            decimal total_smu = 0;
            decimal total_vl_remarcacao = 0;
            decimal total_vl_estoque_semana = 0;
            decimal total_vl_estoque_fabrica = 0;
            decimal total_pc_qt_giro = 0;
            decimal total_pc_qt_giro_antigo = 0;

            int count_smu = 0;
            int count_remarcacao = 0;
            int count_giro = 0;
            int count_markup = 0;

            int total_qt_venda_semana = 0;
            int total_qt_venda_mes = 0;
            int total_qt_venda_semana_ano_ant = 0;
            int total_qt_venda_mes_ano_ant = 0;
            int total_qt_estoque_semana = 0;
            int total_qt_estoque_ini_semana = 0;
            int total_qt_estoque_fabrica = 0;

            int total_qt_cota_semana = 0;
            decimal total_vl_cota_semana = 0;
            int total_qt_cota_mes = 0;
            decimal total_vl_cota_mes = 0;

            List<LineReportColecao> lineReport = new List<LineReportColecao>();

            List<Sp_Line_Report_ColecaoResult> colecaoProduto = null;
            List<Sp_Line_Report_Colecao_SubResult> colecaoProdutoSub = null;

            if (categoria.Equals("02"))
                colecaoProdutoSub = baseController.BuscaVendasColecaoSub(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimSemana, dataInicioSemanaAnoAnt, dataFimSemanaAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior);
            else
                colecaoProduto = baseController.BuscaVendasColecao(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimSemana, dataInicioSemanaAnoAnt, dataFimSemanaAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior);

            if (colecaoProdutoSub == null)
            {
                if (colecaoProduto != null)
                {
                    foreach (Sp_Line_Report_ColecaoResult item in colecaoProduto)
                    {
                        LineReportColecao lr = new LineReportColecao();

                        lr.Grupo = item.grupo;
                        lr.Colecao = colecao;
                        lr.Descricao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO;

                        lr.Qt_venda_semana = (Convert.ToInt32(item.qt_venda_semana) - Convert.ToInt32(item.qt_troca_semana)).ToString("###,###,##0");
                        lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                        total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana) - Convert.ToInt32(item.qt_troca_semana);
                        total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                        lr.Qt_vd_sem_ano_pas = (Convert.ToInt32(item.qt_venda_semana_ano_ant) - Convert.ToInt32(item.qt_troca_semana_ano_ant)).ToString("###,###,##0");
                        lr.Vl_vd_sem_ano_pas = Convert.ToDecimal(item.vl_venda_semana_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_semana_ano_ant += Convert.ToInt32(item.qt_venda_semana_ano_ant) - Convert.ToInt32(item.qt_troca_semana_ano_ant);
                        total_vl_venda_semana_ano_ant += Convert.ToDecimal(item.vl_venda_semana_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_semana) > 0)
                            lr.Pc_medio_semana = ((Convert.ToDecimal(item.pc_medio_venda_semana) - Convert.ToDecimal(item.pc_medio_troca_semana)) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                        // Cotas Semana

                        COTAS_SEMANA cs = baseController.BuscaCotasSemana(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        if (cs != null)
                        {
                            lr.Cota_semana_qtde = Convert.ToInt32(cs.QTDE).ToString("###,###,##0");
                            lr.Cota_semana_valor = Convert.ToDecimal(cs.VALOR).ToString("###,###,##0.00");

                            lr.Dif_semana_qtde = Convert.ToInt32((item.qt_venda_semana - item.qt_troca_semana) - cs.QTDE).ToString("###,###,##0");
                            lr.Dif_semana_valor = Convert.ToDecimal(item.vl_venda_semana - cs.VALOR).ToString("###,###,##0.00");

                            total_qt_cota_semana += Convert.ToInt32(cs.QTDE);
                            total_vl_cota_semana += Convert.ToDecimal(cs.VALOR);
                        }
                        
                        // Cotas Mes

                        int qtdeCotaMes = baseController.BuscaQtdeCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));
                        decimal valorCotaMes = baseController.BuscaValorCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        lr.Cota_mes_qtde = qtdeCotaMes.ToString("###,###,##0");
                        lr.Cota_mes_valor = valorCotaMes.ToString("###,###,##0.00");

                        lr.Dif_mes_qtde = Convert.ToInt32((item.qt_venda_mes - item.qt_troca_mes) - qtdeCotaMes).ToString("###,###,##0");
                        lr.Dif_mes_valor = Convert.ToDecimal(item.vl_venda_semana - valorCotaMes).ToString("###,###,##0.00");

                        total_qt_cota_mes += qtdeCotaMes;
                        total_vl_cota_mes += valorCotaMes;
                        
                        //Markup

                        if (Convert.ToDecimal(item.vl_original_fabrica_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            lr.Var_markup_original = (((Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_original += (Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana);

                            lr.Var_markup_original_antigo = (Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_original_antigo += Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        if (Convert.ToDecimal(item.vl_venda_real_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            count_markup++;

                            lr.Var_markup_liquido = (((Convert.ToDecimal(item.vl_venda_real_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_real_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_liquido += (Convert.ToDecimal(item.vl_venda_real_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_real_semana);

                            lr.Var_markup_liquido_antigo = (Convert.ToDecimal(item.vl_venda_real_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_liquido_antigo += Convert.ToDecimal(item.vl_venda_real_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        lr.Qt_venda_mes = (Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_troca_mes)).ToString("###,###,##0");
                        lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                        count_remarcacao++;

                        if (Convert.ToDecimal(item.vl_original_fabrica_mes) > 0)
                        {
                            lr.Vl_remarcacao = (((Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_real_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes)) * 100).ToString("N2") + "%";

                            total_vl_remarcacao += (Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_real_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes);
                        }

                        total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_troca_mes);
                        total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                        lr.Qt_vd_mes_ano_pas = (Convert.ToInt32(item.qt_venda_mes_ano_ant) - Convert.ToInt32(item.qt_troca_mes_ano_ant)).ToString("###,###,##0");
                        lr.Vl_vd_mes_ano_pas = Convert.ToDecimal(item.vl_venda_mes_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_mes_ano_ant += Convert.ToInt32(item.qt_venda_mes_ano_ant) - Convert.ToInt32(item.qt_troca_mes_ano_ant);
                        total_vl_venda_mes_ano_ant += Convert.ToDecimal(item.vl_venda_mes_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_mes) > 0)
                            lr.Pc_medio_mes = ((Convert.ToDecimal(item.pc_medio_venda_mes) - Convert.ToDecimal(item.pc_medio_troca_mes)) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                        int qt_estoque_1a_semana = 0;
                        int qt_estoque_ultima_semana = 0;

                        //Sp_Line_Report_Estoque_Semana_Fechada_Grupo_ProdutoResult sp_Line_ReportEstoqueSemanaFechadaResult = baseController.BuscaEstoqueSemanaFechada(dataInicioSemana, dataFimSemana, item.grupo, griffe, colecao, categoria);
                        ESTOQUE_DATA sp_Line_ReportEstoqueSemanaFechadaResult = baseController.BuscaEstoqueSemanaFechadaNovo(anoSemana, item.grupo, griffe, colecao, categoria);

                        if (sp_Line_ReportEstoqueSemanaFechadaResult != null)
                        {
                            if (sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL != null)
                            {
                                lr.Qt_estoque_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL).ToString("###,###,##0");
                                total_qt_estoque_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);

                                qt_estoque_ultima_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);

                                lr.Vl_estoque_semana = Convert.ToDecimal(sp_Line_ReportEstoqueSemanaFechadaResult.VALOR_ESTOQUE_FINAL).ToString("###,###,##0.00");
                                total_vl_estoque_semana += Convert.ToDecimal(lr.Vl_estoque_semana);
                            }

                            if (sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL != null)
                            {
                                qt_estoque_1a_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL);

                                lr.Qt_estoque_ini_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL).ToString("###,###,##0");
                                total_qt_estoque_ini_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL);
                            }
                        }

                        Sp_Line_Report_Estoque_Valor_Grupo_ProdutoResult sp_Line_ReportEstoqueValorResult = baseController.BuscaEstoqueValor(item.grupo, griffe, colecao, "Loja");

                        if (sp_Line_ReportEstoqueValorResult != null)
                        {
                            if (sp_Line_ReportEstoqueValorResult != null && sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA > 0)
                            {
                                count_smu++;
                                lr.Smu = (((sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorResult.ESTOQUE_CUSTO) / (sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA)) * 100).ToString("N2") + "%";
                                total_smu += (sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorResult.ESTOQUE_CUSTO) / sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA;
                            }
                        }

                        sp_Line_ReportEstoqueValorResult = baseController.BuscaEstoqueValor(item.grupo, griffe, colecao, "Outras");

                        if (sp_Line_ReportEstoqueValorResult != null)
                        {
                            lr.Qt_estoque_fabrica = Convert.ToInt32(sp_Line_ReportEstoqueValorResult.ESTOQUE_QTDE).ToString("###,###,##0");
                            lr.Vl_estoque_fabrica = Convert.ToDecimal(sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA).ToString("###,###,##0.00");

                            total_qt_estoque_fabrica += Convert.ToInt32(sp_Line_ReportEstoqueValorResult.ESTOQUE_QTDE);
                            total_vl_estoque_fabrica += Convert.ToDecimal(sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA);
                        }

                        if (((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2) > 0 && Convert.ToDecimal(item.qt_venda_semana) > 0)
                        {
                            count_giro++;
                            lr.Pc_qt_giro = (Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2)).ToString("###,###,##0.00");
                            total_pc_qt_giro += Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2);

                            lr.Pc_qt_giro_antigo = (Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana)).ToString("###,###,##0.00");
                            total_pc_qt_giro_antigo += Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana);
                        }

                        lineReport.Add(lr);
                    }
                }
            }
            else
            {
                if (colecaoProdutoSub != null)
                {
                    foreach (Sp_Line_Report_Colecao_SubResult item in colecaoProdutoSub)
                    {
                        LineReportColecao lr = new LineReportColecao();

                        lr.Grupo = item.grupo;
                        lr.Colecao = colecao;
                        lr.Descricao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO;

                        lr.Qt_venda_semana = (Convert.ToInt32(item.qt_venda_semana) - Convert.ToInt32(item.qt_troca_semana)).ToString("###,###,##0");
                        lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                        total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana) - Convert.ToInt32(item.qt_troca_semana);
                        total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                        lr.Qt_vd_sem_ano_pas = (Convert.ToInt32(item.qt_venda_semana_ano_ant) - Convert.ToInt32(item.qt_troca_semana_ano_ant)).ToString("###,###,##0");
                        lr.Vl_vd_sem_ano_pas = Convert.ToDecimal(item.vl_venda_semana_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_semana_ano_ant += Convert.ToInt32(item.qt_venda_semana_ano_ant) - Convert.ToInt32(item.qt_troca_semana_ano_ant);
                        total_vl_venda_semana_ano_ant += Convert.ToDecimal(item.vl_venda_semana_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_semana) > 0)
                            lr.Pc_medio_semana = ((Convert.ToDecimal(item.pc_medio_venda_semana) - Convert.ToDecimal(item.pc_medio_troca_semana)) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                        // Cotas

                        COTAS_SEMANA cs = baseController.BuscaCotasSemana(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        if (cs != null)
                        {
                            lr.Cota_semana_qtde = Convert.ToInt32(cs.QTDE).ToString("###,###,##0");
                            lr.Cota_semana_valor = Convert.ToDecimal(cs.VALOR).ToString("###,###,##0.00");

                            lr.Dif_semana_qtde = Convert.ToInt32((item.qt_venda_semana - item.qt_troca_semana) - cs.QTDE).ToString("###,###,##0");
                            lr.Dif_semana_valor = Convert.ToDecimal(item.vl_venda_semana - cs.VALOR).ToString("###,###,##0.00");

                            total_qt_cota_semana += Convert.ToInt32(cs.QTDE);
                            total_vl_cota_semana += Convert.ToDecimal(cs.VALOR);
                        }

                        // Cotas Mes

                        int qtdeCotaMes = baseController.BuscaQtdeCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));
                        decimal valorCotaMes = baseController.BuscaValorCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        lr.Cota_mes_qtde = qtdeCotaMes.ToString("###,###,##0");
                        lr.Cota_mes_valor = valorCotaMes.ToString("###,###,##0.00");

                        lr.Dif_mes_qtde = Convert.ToInt32((item.qt_venda_mes - item.qt_troca_mes) - qtdeCotaMes).ToString("###,###,##0");
                        lr.Dif_mes_valor = Convert.ToDecimal(item.vl_venda_semana - valorCotaMes).ToString("###,###,##0.00");

                        total_qt_cota_mes += qtdeCotaMes;
                        total_vl_cota_mes += valorCotaMes;

                        //Markup

                        count_markup++;

                        if (Convert.ToDecimal(item.vl_original_fabrica_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            lr.Var_markup_original = (((Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_original += (Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana);

                            lr.Var_markup_original_antigo = (Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_original_antigo += Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        if (Convert.ToDecimal(item.vl_venda_real_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            lr.Var_markup_liquido = (((Convert.ToDecimal(item.vl_venda_real_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_real_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_liquido += (Convert.ToDecimal(item.vl_venda_real_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_real_semana);

                            lr.Var_markup_liquido_antigo = (Convert.ToDecimal(item.vl_venda_real_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_liquido_antigo += Convert.ToDecimal(item.vl_venda_real_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        lr.Qt_venda_mes = (Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_troca_mes)).ToString("###,###,##0");
                        lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                        count_remarcacao++;

                        if (Convert.ToDecimal(item.vl_original_fabrica_mes) > 0)
                        {
                            lr.Vl_remarcacao = (((Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_real_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes)) * 100).ToString("N2") + "%";

                            total_vl_remarcacao += (Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_real_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes);
                        }

                        total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_troca_mes);
                        total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                        lr.Qt_vd_mes_ano_pas = (Convert.ToInt32(item.qt_venda_mes_ano_ant) - Convert.ToInt32(item.qt_troca_mes_ano_ant)).ToString("###,###,##0");
                        lr.Vl_vd_mes_ano_pas = Convert.ToDecimal(item.vl_venda_mes_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_mes_ano_ant += Convert.ToInt32(item.qt_venda_mes_ano_ant) - Convert.ToInt32(item.qt_troca_mes_ano_ant);
                        total_vl_venda_mes_ano_ant += Convert.ToDecimal(item.vl_venda_mes_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_mes) > 0)
                            lr.Pc_medio_mes = ((Convert.ToDecimal(item.pc_medio_venda_mes) - Convert.ToDecimal(item.pc_medio_troca_mes)) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                        int qt_estoque_1a_semana = 0;
                        int qt_estoque_ultima_semana = 0;

                        //Sp_Line_Report_Estoque_Semana_Fechada_Grupo_Produto_SubResult sp_Line_ReportEstoqueSemanaFechadaSubResult = baseController.BuscaEstoqueSemanaFechadaSub(dataInicioSemana, dataFimSemana, item.grupo, griffe, colecao, categoria);

                        ESTOQUE_DATA sp_Line_ReportEstoqueSemanaFechadaSubResult = baseController.BuscaEstoqueSemanaFechadaNovo(anoSemana, item.grupo, griffe, colecao, categoria);

                        if (sp_Line_ReportEstoqueSemanaFechadaSubResult != null)
                        {
                            if (sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL != null)
                            {
                                lr.Qt_estoque_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL).ToString("###,###,##0");
                                total_qt_estoque_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);

                                qt_estoque_ultima_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);

                                lr.Vl_estoque_semana = Convert.ToDecimal(sp_Line_ReportEstoqueSemanaFechadaSubResult.VALOR_ESTOQUE_FINAL).ToString("###,###,##0.00");
                                total_vl_estoque_semana += Convert.ToDecimal(lr.Vl_estoque_semana);
                            }

                            if (sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL != null)
                            {
                                qt_estoque_1a_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL);

                                lr.Qt_estoque_ini_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL).ToString("###,###,##0");
                                total_qt_estoque_ini_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL);
                            }

                        }

                        Sp_Line_Report_Estoque_Valor_Grupo_Produto_SubResult sp_Line_ReportEstoqueValorSubResult = baseController.BuscaEstoqueValorSub(item.grupo, griffe, colecao, "Loja");

                        if (sp_Line_ReportEstoqueValorSubResult != null)
                        {
                            if (sp_Line_ReportEstoqueValorSubResult != null && sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA > 0)
                            {
                                count_smu++;
                                lr.Smu = (((sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorSubResult.ESTOQUE_CUSTO) / sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA) * 100).ToString("N2") + "%";
                                total_smu += (sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorSubResult.ESTOQUE_CUSTO) / sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA;
                            }
                        }

                        sp_Line_ReportEstoqueValorSubResult = baseController.BuscaEstoqueValorSub(item.grupo, griffe, colecao, "Loja");

                        if (sp_Line_ReportEstoqueValorSubResult != null)
                        {
                            lr.Qt_estoque_fabrica = Convert.ToInt32(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_QTDE).ToString("###,###,##0");
                            lr.Vl_estoque_fabrica = Convert.ToDecimal(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA).ToString("###,###,##0.00");

                            total_qt_estoque_fabrica += Convert.ToInt32(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_QTDE);
                            total_vl_estoque_fabrica += Convert.ToDecimal(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA);
                        }

                        if (((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2) > 0 && Convert.ToDecimal(item.qt_venda_semana) > 0)
                        {
                            count_giro++;

                            lr.Pc_qt_giro = (Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2)).ToString("###,###,##0.00");
                            total_pc_qt_giro += Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2);

                            lr.Pc_qt_giro_antigo = (Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana)).ToString("###,###,##0.00");
                            total_pc_qt_giro_antigo += Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana);
                        }

                        lineReport.Add(lr);
                    }
                }
            }

            if (colecaoProduto != null || colecaoProdutoSub != null)
            {
                if (count_smu == 0)
                    count_smu = 1;
                if (count_remarcacao == 0)
                    count_remarcacao = 1;
                if (count_giro == 0)
                    count_giro = 1;
                if (count_markup == 0)
                    count_markup = 1;

                LineReportColecao lr = new LineReportColecao();

                lr.Grupo = "Total";
                lr.Colecao = "Total";
                lr.Descricao = "Total";
                lr.Qt_venda_semana = total_qt_venda_semana.ToString("###,###,##0");
                lr.Vl_venda_semana = total_vl_venda_semana.ToString("###,###,##0.00");

                lr.Cota_semana_qtde = total_qt_cota_semana.ToString("###,###,##0");
                lr.Cota_semana_valor = total_vl_cota_semana.ToString("###,###,##0.00");

                lr.Dif_semana_qtde = Convert.ToInt32(total_qt_venda_semana - total_qt_cota_semana).ToString("###,###,##0");
                lr.Dif_semana_valor = Convert.ToDecimal(total_vl_venda_semana - total_vl_cota_semana).ToString("###,###,##0.00");

                lr.Cota_mes_qtde = total_qt_cota_mes.ToString("###,###,##0");
                lr.Cota_mes_valor = total_vl_cota_mes.ToString("###,###,##0.00");

                lr.Dif_mes_qtde = Convert.ToInt32(total_qt_venda_mes - total_qt_cota_mes).ToString("###,###,##0");
                lr.Dif_mes_valor = Convert.ToDecimal(total_vl_venda_mes - total_vl_cota_mes).ToString("###,###,##0.00");

                lr.Qt_vd_sem_ano_pas = total_qt_venda_semana_ano_ant.ToString("###,###,##0");
                lr.Vl_vd_sem_ano_pas = total_vl_venda_semana_ano_ant.ToString("###,###,##0.00");

                if (total_qt_venda_semana > 0)
                    lr.Pc_medio_semana = (total_vl_venda_semana / total_qt_venda_semana).ToString("###,###,##0.00");

                lr.Qt_venda_mes = total_qt_venda_mes.ToString("###,###,##0");
                lr.Vl_venda_mes = total_vl_venda_mes.ToString("###,###,##0.00");

                lr.Qt_vd_mes_ano_pas = total_qt_venda_mes_ano_ant.ToString("###,###,##0");
                lr.Vl_vd_mes_ano_pas = total_vl_venda_mes_ano_ant.ToString("###,###,##0.00");

                if (total_qt_venda_mes > 0)
                    lr.Pc_medio_mes = (total_vl_venda_mes / total_qt_venda_mes).ToString("###,###,##0.00");

                lr.Qt_estoque_ini_semana = total_qt_estoque_ini_semana.ToString("###,###,##0");
                lr.Qt_estoque_semana = total_qt_estoque_semana.ToString("###,###,##0");
                lr.Vl_estoque_semana = total_vl_estoque_semana.ToString("###,###,##0.00");
                lr.Qt_estoque_fabrica = total_qt_estoque_fabrica.ToString("###,###,##0");
                lr.Vl_estoque_fabrica = total_vl_estoque_fabrica.ToString("###,###,##0.00");

                if (count_markup > 0)
                {
                    lr.Var_markup_original = ((total_var_markup_original / count_markup) * 100).ToString("N2") + "%";
                    lr.Var_markup_liquido = ((total_var_markup_liquido / count_markup) * 100).ToString("N2") + "%";
                    lr.Var_markup_original_antigo = (total_var_markup_original_antigo / count_markup).ToString("N2");
                    lr.Var_markup_liquido_antigo = (total_var_markup_liquido_antigo / count_markup).ToString("N2");
                }

                if (count_smu > 0)
                    lr.Smu = ((total_smu / count_smu) * 100).ToString("N2") + "%";

                if (count_remarcacao > 0)
                    lr.Vl_remarcacao = ((total_vl_remarcacao / count_remarcacao) * 100).ToString("N2") + "%";

                if (count_giro > 0)
                {
                    lr.Pc_qt_giro = (total_pc_qt_giro / count_giro).ToString("###,###,##0.00");
                    lr.Pc_qt_giro_antigo = (total_pc_qt_giro_antigo / count_giro).ToString("###,###,##0.00");
                }

                lineReport.Add(lr);
            }

            return lineReport;
        }

        public List<LineReportColecao> BuscaMovimentoColecaoLiq(string categoria, string colecao, string anoSemana, string griffe, string colecaoAnoAnterior)
        {
            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);

            string ano = buscaData.ANO454;
            string semana = anoSemana.Substring(4, 2);

            string dataInicioSemana = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(baseController.BuscaDataFim(anoSemana));

            string dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMes = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            string dataInicioMesFechado = "";
            string dataFimMesFechado = "";

            if (mes < 4)
            {
                if (mes == 1)
                {
                    dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(12, (Convert.ToInt32(ano) - 2).ToString()));
                    dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(12, (Convert.ToInt32(ano) - 2).ToString()));
                }
                else
                {
                    dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(mes - 1, (Convert.ToInt32(ano) - 1).ToString()));
                    dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(mes - 1, (Convert.ToInt32(ano) - 1).ToString()));
                }
            }
            else
            {
                dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(mes - 1, ano));
                dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(mes - 1, ano));
            }

            string dataInicioSemanaAnoAnt = AjustaData(baseController.BuscaDataInicio((Convert.ToInt32(ano) - 1).ToString() + semana));
            string dataFimSemanaAnoAnt = AjustaData(baseController.BuscaDataFim((Convert.ToInt32(ano) - 1).ToString() + semana));

            string dataInicioMesAnoAnt = AjustaData(baseController.BuscaDataInicioMes(mes, (Convert.ToInt32(ano) - 1).ToString()));
            string dataFimMesAnoAnt = AjustaData(baseController.BuscaDataFimMes(mes, (Convert.ToInt32(ano) - 1).ToString()));

            decimal total_vl_venda_semana = 0;
            decimal total_vl_venda_mes = 0;
            decimal total_vl_venda_semana_ano_ant = 0;
            decimal total_vl_venda_mes_ano_ant = 0;
            decimal total_var_markup_liquido = 0;
            decimal total_var_markup_original = 0;
            decimal total_var_markup_liquido_antigo = 0;
            decimal total_var_markup_original_antigo = 0;
            decimal total_smu = 0;
            decimal total_vl_remarcacao = 0;
            decimal total_vl_estoque_semana = 0;
            decimal total_vl_estoque_fabrica = 0;
            decimal total_pc_qt_giro = 0;
            decimal total_pc_qt_giro_antigo = 0;

            int count_smu = 0;
            int count_remarcacao = 0;
            int count_giro = 0;
            int count_markup = 0;

            int total_qt_venda_semana = 0;
            int total_qt_venda_mes = 0;
            int total_qt_venda_semana_ano_ant = 0;
            int total_qt_venda_mes_ano_ant = 0;
            int total_qt_estoque_semana = 0;
            int total_qt_estoque_ini_semana = 0;
            int total_qt_estoque_fabrica = 0;
            int total_qt_estoque_sobra = 0;
            decimal total_vl_estoque_sobra = 0;

            int total_qt_cota_semana = 0;
            decimal total_vl_cota_semana = 0;

            int total_qt_cota_mes = 0;
            decimal total_vl_cota_mes = 0;

            List<LineReportColecao> lineReport = new List<LineReportColecao>();

            List<Sp_Line_Report_Colecao_AtualResult> colecaoProduto = null;
            List<Sp_Line_Report_Colecao_Sub_AtualResult> colecaoProdutoSub = null;

            if (categoria.Equals("02"))
                colecaoProdutoSub = baseController.BuscaVendasColecaoSubLiq(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimSemana, dataInicioSemanaAnoAnt, dataFimSemanaAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior);
            else
                colecaoProduto = baseController.BuscaVendasColecaoLiq(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimSemana, dataInicioSemanaAnoAnt, dataFimSemanaAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior);

            if (colecaoProdutoSub == null)
            {
                if (colecaoProduto != null)
                {
                    foreach (Sp_Line_Report_Colecao_AtualResult item in colecaoProduto)
                    {
                        LineReportColecao lr = new LineReportColecao();

                        lr.Grupo = item.grupo;
                        lr.Colecao = colecao;
                        lr.Descricao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO;

                        lr.Qt_venda_semana = Convert.ToInt32(item.qt_venda_semana).ToString("###,###,##0");
                        lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                        total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana);
                        total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                        lr.Qt_vd_sem_ano_pas = Convert.ToInt32(item.qt_venda_semana_ano_ant).ToString("###,###,##0");
                        lr.Vl_vd_sem_ano_pas = Convert.ToDecimal(item.vl_venda_semana_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_semana_ano_ant += Convert.ToInt32(item.qt_venda_semana_ano_ant);
                        total_vl_venda_semana_ano_ant += Convert.ToDecimal(item.vl_venda_semana_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_semana) > 0)
                            lr.Pc_medio_semana = ((Convert.ToDecimal(item.pc_medio_venda_semana) - Convert.ToDecimal(item.pc_medio_troca_semana)) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                        // Cotas

                        COTAS_SEMANA cs = baseController.BuscaCotasSemana(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        if (cs != null)
                        {
                            lr.Cota_semana_qtde = Convert.ToInt32(cs.QTDE).ToString("###,###,##0");
                            lr.Cota_semana_valor = Convert.ToDecimal(cs.VALOR).ToString("###,###,##0.00");

                            lr.Dif_semana_qtde = Convert.ToInt32(item.qt_venda_semana - cs.QTDE).ToString("###,###,##0");
                            lr.Dif_semana_valor = Convert.ToDecimal(item.vl_venda_semana - cs.VALOR).ToString("###,###,##0.00");

                            total_qt_cota_semana += Convert.ToInt32(cs.QTDE);
                            total_vl_cota_semana += Convert.ToDecimal(cs.VALOR);
                        }

                        // Qtde Sobra e  //Valor Sobra
                        ESTOQUE_SOBRA estoqueSobra = baseController.BuscaEstoqueSobra(categoria, colecao, item.grupo, griffe); //Busca Saldo da Sobra

                        if (estoqueSobra != null)
                        {
                            int qtdeCotasPeriodo = baseController.BuscaCotasAcumuladaQtde(categoria, colecao, item.grupo, griffe, Convert.ToInt32(estoqueSobra.ANO_SEMANA), Convert.ToInt32(anoSemana));
                            decimal valorCotasPeriodo = baseController.BuscaCotasAcumuladaValor(categoria, colecao, item.grupo, griffe, Convert.ToInt32(estoqueSobra.ANO_SEMANA), Convert.ToInt32(anoSemana));

                            Sp_Busca_Vendas_PeriodoResult vendasPeriodo = baseController.BuscaVendasPeriodo(categoria, colecao, griffe, item.grupo, estoqueSobra.ANO_SEMANA.ToString(), anoSemana);

                            if (vendasPeriodo != null)
                            {
                                lr.Qt_estoque_sobra = ((Convert.ToInt32(estoqueSobra.QTDE) + qtdeCotasPeriodo) - Convert.ToInt32(vendasPeriodo.qt_venda_semana_1)).ToString("###,###,##0");
                                total_qt_estoque_sobra += ((Convert.ToInt32(estoqueSobra.QTDE) + qtdeCotasPeriodo) - Convert.ToInt32(vendasPeriodo.qt_venda_semana_1));
                            }

                            SP_BUSCA_VALOR_VENDAS_PERIODOResult valorVendasPeriodo = baseController.BuscarValorVendasPeriodo(categoria, colecao, griffe, item.grupo, estoqueSobra.ANO_SEMANA.ToString(), anoSemana);

                            if (valorVendasPeriodo != null)
                            {
                                lr.Vl_estoque_sobra = ((Convert.ToDecimal(estoqueSobra.VALOR) + valorCotasPeriodo) - Convert.ToInt32(valorVendasPeriodo.vl_venda_semana)).ToString("###,###,##0.00");
                                total_vl_estoque_sobra += ((Convert.ToDecimal(estoqueSobra.VALOR) + valorCotasPeriodo) - Convert.ToInt32(valorVendasPeriodo.vl_venda_semana));
                            }

                        }

                        // Cotas Mes

                        int qtdeCotaMes = baseController.BuscaQtdeCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));
                        decimal valorCotaMes = baseController.BuscaValorCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        lr.Cota_mes_qtde = qtdeCotaMes.ToString("###,###,##0");
                        lr.Cota_mes_valor = valorCotaMes.ToString("###,###,##0.00");

                        lr.Dif_mes_qtde = Convert.ToInt32(item.qt_venda_mes - qtdeCotaMes).ToString("###,###,##0");
                        lr.Dif_mes_valor = Convert.ToDecimal(item.vl_venda_semana - valorCotaMes).ToString("###,###,##0.00");

                        total_qt_cota_mes += qtdeCotaMes;
                        total_vl_cota_mes += valorCotaMes;

                        //Markup

                        if (Convert.ToDecimal(item.vl_original_fabrica_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            lr.Var_markup_original = (((Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_original += (Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana);

                            lr.Var_markup_original_antigo = (Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_original_antigo += Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        if (Convert.ToDecimal(item.vl_venda_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            count_markup++;

                            lr.Var_markup_liquido = (((Convert.ToDecimal(item.vl_venda_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_liquido += (Convert.ToDecimal(item.vl_venda_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_semana);

                            lr.Var_markup_liquido_antigo = (Convert.ToDecimal(item.vl_venda_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_liquido_antigo += Convert.ToDecimal(item.vl_venda_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        lr.Qt_venda_mes = Convert.ToInt32(item.qt_venda_mes).ToString("###,###,##0");
                        lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                        count_remarcacao++;

                        if (Convert.ToDecimal(item.vl_original_fabrica_mes) > 0)
                        {
                            lr.Vl_remarcacao = (((Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes)) * 100).ToString("N2") + "%";

                            total_vl_remarcacao += (Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes);
                        }

                        total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes);
                        total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                        lr.Qt_vd_mes_ano_pas = Convert.ToInt32(item.qt_venda_mes_ano_ant).ToString("###,###,##0");
                        lr.Vl_vd_mes_ano_pas = Convert.ToDecimal(item.vl_venda_mes_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_mes_ano_ant += Convert.ToInt32(item.qt_venda_mes_ano_ant);
                        total_vl_venda_mes_ano_ant += Convert.ToDecimal(item.vl_venda_mes_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_mes) > 0)
                            lr.Pc_medio_mes = ((Convert.ToDecimal(item.pc_medio_venda_mes) - Convert.ToDecimal(item.pc_medio_troca_mes)) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                        int qt_estoque_1a_semana = 0;
                        int qt_estoque_ultima_semana = 0;

                        ESTOQUE_DATA sp_Line_ReportEstoqueSemanaFechadaResult = baseController.BuscaEstoqueSemanaFechadaNovo(anoSemana, item.grupo, griffe, colecao, categoria);

                        if (sp_Line_ReportEstoqueSemanaFechadaResult != null)
                        {
                            if (sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL != null)
                            {
                                lr.Qt_estoque_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL).ToString("###,###,##0");
                                total_qt_estoque_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);

                                qt_estoque_ultima_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);

                                lr.Vl_estoque_semana = Convert.ToDecimal(sp_Line_ReportEstoqueSemanaFechadaResult.VALOR_ESTOQUE_FINAL).ToString("###,###,##0.00");
                                total_vl_estoque_semana += Convert.ToDecimal(lr.Vl_estoque_semana);
                            }

                            if (sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL != null)
                            {
                                qt_estoque_1a_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL);

                                lr.Qt_estoque_ini_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL).ToString("###,###,##0");
                                total_qt_estoque_ini_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL);
                            }
                        }

                        Sp_Line_Report_Estoque_Valor_Grupo_ProdutoResult sp_Line_ReportEstoqueValorResult = baseController.BuscaEstoqueValor(item.grupo, griffe, colecao, "Loja");

                        if (sp_Line_ReportEstoqueValorResult != null)
                        {
                            if (sp_Line_ReportEstoqueValorResult != null && sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA > 0)
                            {
                                count_smu++;
                                lr.Smu = (((sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorResult.ESTOQUE_CUSTO) / (sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA)) * 100).ToString("N2") + "%";
                                total_smu += (sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorResult.ESTOQUE_CUSTO) / sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA;
                            }
                        }

                        sp_Line_ReportEstoqueValorResult = baseController.BuscaEstoqueValor(item.grupo, griffe, colecao, "Outras");

                        if (sp_Line_ReportEstoqueValorResult != null)
                        {
                            lr.Qt_estoque_fabrica = Convert.ToInt32(sp_Line_ReportEstoqueValorResult.ESTOQUE_QTDE).ToString("###,###,##0");
                            lr.Vl_estoque_fabrica = Convert.ToDecimal(sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA).ToString("###,###,##0.00");

                            total_qt_estoque_fabrica += Convert.ToInt32(sp_Line_ReportEstoqueValorResult.ESTOQUE_QTDE);
                            total_vl_estoque_fabrica += Convert.ToDecimal(sp_Line_ReportEstoqueValorResult.ESTOQUE_VENDA);
                        }

                        if (((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2) > 0 && Convert.ToDecimal(item.qt_venda_semana) > 0)
                        {
                            count_giro++;
                            lr.Pc_qt_giro = (Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2)).ToString("###,###,##0.00");
                            total_pc_qt_giro += Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2);

                            lr.Pc_qt_giro_antigo = (Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana)).ToString("###,###,##0.00");
                            total_pc_qt_giro_antigo += Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana);
                        }

                        lineReport.Add(lr);
                    }
                }
            }
            else
            {
                if (colecaoProdutoSub != null)
                {
                    foreach (Sp_Line_Report_Colecao_Sub_AtualResult item in colecaoProdutoSub)
                    {
                        LineReportColecao lr = new LineReportColecao();

                        lr.Grupo = item.grupo;
                        lr.Colecao = colecao;
                        lr.Descricao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO;

                        lr.Qt_venda_semana = Convert.ToInt32(item.qt_venda_semana).ToString("###,###,##0");
                        lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                        total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana);
                        total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                        lr.Qt_vd_sem_ano_pas = Convert.ToInt32(item.qt_venda_semana_ano_ant).ToString("###,###,##0");
                        lr.Vl_vd_sem_ano_pas = Convert.ToDecimal(item.vl_venda_semana_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_semana_ano_ant += Convert.ToInt32(item.qt_venda_semana_ano_ant);
                        total_vl_venda_semana_ano_ant += Convert.ToDecimal(item.vl_venda_semana_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_semana) > 0)
                            lr.Pc_medio_semana = ((Convert.ToDecimal(item.pc_medio_venda_semana) - Convert.ToDecimal(item.pc_medio_troca_semana)) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                        // Cotas

                        COTAS_SEMANA cs = baseController.BuscaCotasSemana(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        if (cs != null)
                        {
                            lr.Cota_semana_qtde = Convert.ToInt32(cs.QTDE).ToString("###,###,##0");
                            lr.Cota_semana_valor = Convert.ToDecimal(cs.VALOR).ToString("###,###,##0.00");

                            lr.Dif_semana_qtde = Convert.ToInt32((item.qt_venda_semana) - cs.QTDE).ToString("###,###,##0");
                            lr.Dif_semana_valor = Convert.ToDecimal(item.vl_venda_semana - cs.VALOR).ToString("###,###,##0.00");

                            total_qt_cota_semana += Convert.ToInt32(cs.QTDE);
                            total_vl_cota_semana += Convert.ToDecimal(cs.VALOR);
                        }

                        // Cotas Mes

                        int qtdeCotaMes = baseController.BuscaQtdeCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));
                        decimal valorCotaMes = baseController.BuscaValorCotasMes(Convert.ToInt32(colecao), Convert.ToInt32(categoria), griffe, item.grupo, Convert.ToInt32(anoSemana));

                        lr.Cota_mes_qtde = qtdeCotaMes.ToString("###,###,##0");
                        lr.Cota_mes_valor = valorCotaMes.ToString("###,###,##0.00");

                        lr.Dif_mes_qtde = Convert.ToInt32(item.qt_venda_mes - qtdeCotaMes).ToString("###,###,##0");
                        lr.Dif_mes_valor = Convert.ToDecimal(item.vl_venda_semana - valorCotaMes).ToString("###,###,##0.00");

                        total_qt_cota_mes += qtdeCotaMes;
                        total_vl_cota_mes += valorCotaMes;

                        //Markup

                        count_markup++;

                        if (Convert.ToDecimal(item.vl_original_fabrica_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            lr.Var_markup_original = (((Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_original += (Convert.ToDecimal(item.vl_original_fabrica_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_original_fabrica_semana);

                            lr.Var_markup_original_antigo = (Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_original_antigo += Convert.ToDecimal(item.vl_original_fabrica_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        if (Convert.ToDecimal(item.vl_venda_semana) > 0 && Convert.ToDecimal(item.vl_custo_semana) > 0)
                        {
                            lr.Var_markup_liquido = (((Convert.ToDecimal(item.vl_venda_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_semana)) * 100).ToString("N2") + "%";

                            total_var_markup_liquido += (Convert.ToDecimal(item.vl_venda_semana) - Convert.ToDecimal(item.vl_custo_semana)) / Convert.ToDecimal(item.vl_venda_semana);

                            lr.Var_markup_liquido_antigo = (Convert.ToDecimal(item.vl_venda_semana) / Convert.ToDecimal(item.vl_custo_semana)).ToString("N2");

                            total_var_markup_liquido_antigo += Convert.ToDecimal(item.vl_venda_semana) / Convert.ToDecimal(item.vl_custo_semana);
                        }

                        lr.Qt_venda_mes = Convert.ToInt32(item.qt_venda_mes).ToString("###,###,##0");
                        lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                        count_remarcacao++;

                        if (Convert.ToDecimal(item.vl_original_fabrica_mes) > 0)
                        {
                            lr.Vl_remarcacao = (((Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes)) * 100).ToString("N2") + "%";

                            total_vl_remarcacao += (Convert.ToDecimal(item.vl_original_fabrica_mes) - Convert.ToDecimal(item.vl_venda_mes)) / Convert.ToDecimal(item.vl_original_fabrica_mes);
                        }

                        total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes);
                        total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                        lr.Qt_vd_mes_ano_pas = Convert.ToInt32(item.qt_venda_mes_ano_ant).ToString("###,###,##0");
                        lr.Vl_vd_mes_ano_pas = Convert.ToDecimal(item.vl_venda_mes_ano_ant).ToString("###,###,##0.00");

                        total_qt_venda_mes_ano_ant += Convert.ToInt32(item.qt_venda_mes_ano_ant);
                        total_vl_venda_mes_ano_ant += Convert.ToDecimal(item.vl_venda_mes_ano_ant);

                        if (Convert.ToInt32(item.qt_venda_mes) > 0)
                            lr.Pc_medio_mes = ((Convert.ToDecimal(item.pc_medio_venda_mes) - Convert.ToDecimal(item.pc_medio_troca_mes)) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                        int qt_estoque_1a_semana = 0;
                        int qt_estoque_ultima_semana = 0;

                        ESTOQUE_DATA sp_Line_ReportEstoqueSemanaFechadaSubResult = baseController.BuscaEstoqueSemanaFechadaNovo(anoSemana, item.grupo, griffe, colecao, categoria);

                        if (sp_Line_ReportEstoqueSemanaFechadaSubResult != null)
                        {
                            if (sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL != null)
                            {
                                lr.Qt_estoque_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL).ToString("###,###,##0");
                                total_qt_estoque_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);

                                qt_estoque_ultima_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);

                                lr.Vl_estoque_semana = Convert.ToDecimal(sp_Line_ReportEstoqueSemanaFechadaSubResult.VALOR_ESTOQUE_FINAL).ToString("###,###,##0.00");
                                total_vl_estoque_semana += Convert.ToDecimal(lr.Vl_estoque_semana);
                            }

                            if (sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL != null)
                            {
                                qt_estoque_1a_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL);

                                lr.Qt_estoque_ini_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL).ToString("###,###,##0");
                                total_qt_estoque_ini_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL);
                            }
                        }

                        Sp_Line_Report_Estoque_Valor_Grupo_Produto_SubResult sp_Line_ReportEstoqueValorSubResult = baseController.BuscaEstoqueValorSub(item.grupo, griffe, colecao, "Loja");

                        if (sp_Line_ReportEstoqueValorSubResult != null)
                        {
                            if (sp_Line_ReportEstoqueValorSubResult != null && sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA > 0)
                            {
                                count_smu++;
                                lr.Smu = (((sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorSubResult.ESTOQUE_CUSTO) / sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA) * 100).ToString("N2") + "%";
                                total_smu += (sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA - sp_Line_ReportEstoqueValorSubResult.ESTOQUE_CUSTO) / sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA;
                            }
                        }

                        sp_Line_ReportEstoqueValorSubResult = baseController.BuscaEstoqueValorSub(item.grupo, griffe, colecao, "Loja");

                        if (sp_Line_ReportEstoqueValorSubResult != null)
                        {
                            lr.Qt_estoque_fabrica = Convert.ToInt32(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_QTDE).ToString("###,###,##0");
                            lr.Vl_estoque_fabrica = Convert.ToDecimal(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA).ToString("###,###,##0.00");

                            total_qt_estoque_fabrica += Convert.ToInt32(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_QTDE);
                            total_vl_estoque_fabrica += Convert.ToDecimal(sp_Line_ReportEstoqueValorSubResult.ESTOQUE_VENDA);
                        }

                        if (((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2) > 0 && Convert.ToDecimal(item.qt_venda_semana) > 0)
                        {
                            count_giro++;

                            lr.Pc_qt_giro = (Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2)).ToString("###,###,##0.00");
                            total_pc_qt_giro += Convert.ToDecimal(item.qt_venda_semana) / ((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2);

                            lr.Pc_qt_giro_antigo = (Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana)).ToString("###,###,##0.00");
                            total_pc_qt_giro_antigo += Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana);
                        }

                        lineReport.Add(lr);
                    }
                }
            }

            if (colecaoProduto != null || colecaoProdutoSub != null)
            {
                if (count_smu == 0)
                    count_smu = 1;
                if (count_remarcacao == 0)
                    count_remarcacao = 1;
                if (count_giro == 0)
                    count_giro = 1;
                if (count_markup == 0)
                    count_markup = 1;

                LineReportColecao lr = new LineReportColecao();

                lr.Grupo = "Total";
                lr.Colecao = "";
                //lr.Descricao = "Total";

                if (total_qt_estoque_sobra > 0)
                    lr.Qt_estoque_sobra = total_qt_estoque_sobra.ToString("###,###,##0");
                else
                    lr.Qt_estoque_sobra = "";

                if (total_vl_estoque_sobra != Convert.ToDecimal("0.00"))
                    lr.Vl_estoque_sobra = total_vl_estoque_sobra.ToString("###,###,##0.00");
                else
                    lr.Vl_estoque_sobra = "";

                lr.Qt_venda_semana = total_qt_venda_semana.ToString("###,###,##0");
                lr.Vl_venda_semana = total_vl_venda_semana.ToString("###,###,##0.00");

                lr.Cota_semana_qtde = total_qt_cota_semana.ToString("###,###,##0");
                lr.Cota_semana_valor = total_vl_cota_semana.ToString("###,###,##0.00");

                lr.Dif_semana_qtde = Convert.ToInt32(total_qt_venda_semana - total_qt_cota_semana).ToString("###,###,##0");
                lr.Dif_semana_valor = Convert.ToDecimal(total_vl_venda_semana - total_vl_cota_semana).ToString("###,###,##0.00");

                lr.Cota_mes_qtde = total_qt_cota_mes.ToString("###,###,##0");
                lr.Cota_mes_valor = total_vl_cota_mes.ToString("###,###,##0.00");

                lr.Dif_mes_qtde = Convert.ToInt32(total_qt_venda_semana - total_qt_cota_mes).ToString("###,###,##0");
                lr.Dif_mes_valor = Convert.ToDecimal(total_vl_venda_semana - total_vl_cota_mes).ToString("###,###,##0.00");

                lr.Qt_vd_sem_ano_pas = total_qt_venda_semana_ano_ant.ToString("###,###,##0");
                lr.Vl_vd_sem_ano_pas = total_vl_venda_semana_ano_ant.ToString("###,###,##0.00");

                if (total_qt_venda_semana > 0)
                    lr.Pc_medio_semana = (total_vl_venda_semana / total_qt_venda_semana).ToString("###,###,##0.00");

                lr.Qt_venda_mes = total_qt_venda_mes.ToString("###,###,##0");
                lr.Vl_venda_mes = total_vl_venda_mes.ToString("###,###,##0.00");

                lr.Qt_vd_mes_ano_pas = total_qt_venda_mes_ano_ant.ToString("###,###,##0");
                lr.Vl_vd_mes_ano_pas = total_vl_venda_mes_ano_ant.ToString("###,###,##0.00");

                if (total_qt_venda_mes > 0)
                    lr.Pc_medio_mes = (total_vl_venda_mes / total_qt_venda_mes).ToString("###,###,##0.00");

                lr.Qt_estoque_ini_semana = total_qt_estoque_ini_semana.ToString("###,###,##0");
                lr.Qt_estoque_semana = total_qt_estoque_semana.ToString("###,###,##0");
                lr.Vl_estoque_semana = total_vl_estoque_semana.ToString("###,###,##0.00");
                lr.Qt_estoque_fabrica = total_qt_estoque_fabrica.ToString("###,###,##0");
                lr.Vl_estoque_fabrica = total_vl_estoque_fabrica.ToString("###,###,##0.00");

                if (count_markup > 0)
                {
                    lr.Var_markup_original = ((total_var_markup_original / count_markup) * 100).ToString("N2") + "%";
                    lr.Var_markup_liquido = ((total_var_markup_liquido / count_markup) * 100).ToString("N2") + "%";
                    lr.Var_markup_original_antigo = (total_var_markup_original_antigo / count_markup).ToString("N2");
                    lr.Var_markup_liquido_antigo = (total_var_markup_liquido_antigo / count_markup).ToString("N2");
                }

                if (count_smu > 0)
                    lr.Smu = ((total_smu / count_smu) * 100).ToString("N2") + "%";

                if (count_remarcacao > 0)
                    lr.Vl_remarcacao = ((total_vl_remarcacao / count_remarcacao) * 100).ToString("N2") + "%";

                if (count_giro > 0)
                {
                    lr.Pc_qt_giro = (total_pc_qt_giro / count_giro).ToString("###,###,##0.00");
                    lr.Pc_qt_giro_antigo = (total_pc_qt_giro_antigo / count_giro).ToString("###,###,##0.00");
                }

                lineReport.Add(lr);
            }

            return lineReport;
        }

        public List<LineReportColecao> BuscaMovimentoLojaColecao(string categoria, string colecao, string filial, string griffe)
        {
            FILIAI f = baseController.BuscaFilialCodigo(Convert.ToInt32(filial));
            
            string descricaoFilial = f.FILIAL.Trim()+"%";

            string wdata = DateTime.Now.ToString();

            Sp_Busca_Semana_454Result wsemana = baseController.BuscaSemana454(wdata);

            string anoSemana = (wsemana.ano_semana-1).ToString();

            string dataInicioSemana = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(baseController.BuscaDataFim(anoSemana));

            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);
            string ano = buscaData.ANO454;
            string anoAnterior = (Convert.ToInt32(buscaData.ANO454) - 1).ToString();
            string semana = anoSemana.Substring(4, 2);

            string dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMes = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            if (mes == 1)
            {
                mes = 12;
                ano = (Convert.ToInt32(ano) - 1).ToString();
            }
            else
                mes -= 1;

            string dataInicioSemanaAnoAnterior = AjustaData(baseController.BuscaDataInicio(anoAnterior+semana));
            string dataFimSemanaAnoAnterior = AjustaData(baseController.BuscaDataFim(anoAnterior+semana));

            string dataInicioMesAnoAnterior = AjustaData(baseController.BuscaDataInicioMes(mes, anoAnterior));
            string dataFimMesAnoAnterior = AjustaData(baseController.BuscaDataFimMes(mes, anoAnterior));

            decimal total_vl_dif = 0;
            decimal total_vl_venda_semana_ano_anterior = 0;
            decimal total_vl_venda_mes_ano_anterior = 0;
            decimal total_vl_venda_semana = 0;
            decimal total_vl_venda_mes = 0;
            decimal total_vl_estoque_semana = 0;
            decimal total_pc_qt_giro_antigo = 0;

            int count_giro = 0;

            int total_qt_dif = 0;
            int total_qt_venda_semana = 0;
            int total_qt_venda_mes = 0;
            int total_qt_venda_semana_ano_anterior = 0;
            int total_qt_venda_mes_ano_anterior = 0;
            int total_qt_estoque_semana = 0;

            List<LineReportColecao> lineReport = new List<LineReportColecao>();

            List<Sp_Line_Report_Colecao_LojaResult> colecaoProduto = null;
            List<Sp_Line_Report_Colecao_Sub_LojaResult> colecaoProdutoSub = null;

            if (categoria.Equals("02"))
                colecaoProdutoSub = baseController.BuscaVendasColecaoSubLoja(dataInicioSemana, 
                                                                             dataFimSemana, 
                                                                             categoria, 
                                                                             colecao, 
                                                                             griffe, 
                                                                             dataInicioMes, 
                                                                             dataFimSemana, 
                                                                             filial, 
                                                                             dataInicioSemanaAnoAnterior,
                                                                             dataFimSemanaAnoAnterior,
                                                                             dataInicioMesAnoAnterior,
                                                                             dataFimMesAnoAnterior);
            else
                colecaoProduto = baseController.BuscaVendasColecaoLoja(dataInicioSemana, 
                                                                       dataFimSemana, 
                                                                       categoria, 
                                                                       colecao, 
                                                                       griffe, 
                                                                       dataInicioMes, 
                                                                       dataFimSemana, 
                                                                       filial,
                                                                       dataInicioSemanaAnoAnterior,
                                                                       dataFimSemanaAnoAnterior,
                                                                       dataInicioMesAnoAnterior,
                                                                       dataFimMesAnoAnterior);

            if (colecaoProdutoSub == null)
            {
                foreach (Sp_Line_Report_Colecao_LojaResult item in colecaoProduto)
                {
                    LineReportColecao lr = new LineReportColecao();

                    lr.Grupo = item.grupo;
                    lr.Colecao = colecao.ToString();
                    lr.Descricao = baseController.BuscaColecaoAtual(colecao.ToString()).DESC_COLECAO;

                    lr.Qt_venda_semana = Convert.ToInt32(item.qt_venda_semana).ToString("###,###,##0");
                    lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                    total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana);
                    total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                    if (Convert.ToInt32(item.qt_venda_semana) > 0)
                        lr.Pc_medio_semana = (Convert.ToDecimal(item.vl_venda_semana) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                    lr.Qt_vd_sem_ano_pas = Convert.ToInt32(item.qt_venda_semana_ano_anterior).ToString("###,###,##0");
                    lr.Vl_vd_sem_ano_pas = Convert.ToDecimal(item.vl_venda_semana_ano_anterior).ToString("###,###,##0.00");

                    total_qt_venda_semana_ano_anterior += Convert.ToInt32(item.qt_venda_semana_ano_anterior);
                    total_vl_venda_semana_ano_anterior += Convert.ToDecimal(item.vl_venda_semana_ano_anterior);

                    lr.Qt_venda_mes = Convert.ToInt32(item.qt_venda_mes).ToString("###,###,##0");
                    lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                    total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes);
                    total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                    if (Convert.ToInt32(item.qt_venda_mes) > 0)
                        lr.Pc_medio_mes = (Convert.ToDecimal(item.vl_venda_mes) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                    lr.Qt_vd_mes_ano_pas = Convert.ToInt32(item.qt_venda_mes_ano_anterior).ToString("###,###,##0");
                    lr.Vl_vd_mes_ano_pas = Convert.ToDecimal(item.vl_venda_mes_ano_anterior).ToString("###,###,##0.00");

                    total_qt_venda_mes_ano_anterior += Convert.ToInt32(item.qt_venda_mes_ano_anterior);
                    total_vl_venda_mes_ano_anterior += Convert.ToDecimal(item.vl_venda_mes_ano_anterior);

                    lr.Dif_mes_qtde = (Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_venda_mes_ano_anterior)).ToString("###,###,##0");
                    lr.Dif_mes_valor = (Convert.ToInt32(item.vl_venda_mes) - Convert.ToInt32(item.vl_venda_mes_ano_anterior)).ToString("###,###,##0.00");
                    lr.Dif_2011_2012 = (Convert.ToInt32(item.vl_venda_mes) - Convert.ToInt32(item.vl_venda_mes_ano_anterior)).ToString("###,###,##0.00");

                    total_qt_dif += Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_venda_mes_ano_anterior);
                    total_vl_dif += Convert.ToInt32(item.vl_venda_mes) - Convert.ToInt32(item.vl_venda_mes_ano_anterior);

                    //int qt_estoque_1a_semana = 0;
                    //int qt_estoque_ultima_semana = 0;
                    /*
                    Sp_Line_Report_Estoque_Semana_Fechada_LojaResult sp_Line_ReportEstoqueSemanaFechadaResult = baseController.BuscaEstoqueSemanaFechadaLoja(dataInicioSemana, dataFimSemana, item.grupo, griffe, colecao.ToString(), categoria, descricaoFilial);

                    if (sp_Line_ReportEstoqueSemanaFechadaResult != null)
                    {
                        if (sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL != null)
                        {
                            lr.Qt_estoque_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL).ToString("###,###,##0");
                            total_qt_estoque_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);

                            qt_estoque_ultima_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_FINAL);

                            lr.Vl_estoque_semana = Convert.ToDecimal(sp_Line_ReportEstoqueSemanaFechadaResult.VALOR_ESTOQUE_FINAL).ToString("###,###,##0.00");
                            total_vl_estoque_semana += Convert.ToDecimal(lr.Vl_estoque_semana);
                        }

                        if (sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL != null)
                            qt_estoque_1a_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaResult.ESTOQUE_INICIAL);
                    }

                    if (((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2) > 0 && Convert.ToDecimal(item.qt_venda_semana) > 0)
                    {
                        count_giro++;

                        lr.Pc_qt_giro_antigo = (Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana)).ToString("###,###,##0.00");
                        total_pc_qt_giro_antigo += Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana);
                    }
                    */
                    lineReport.Add(lr);
                }
            }
            else
            {
                foreach (Sp_Line_Report_Colecao_Sub_LojaResult item in colecaoProdutoSub)
                {
                    LineReportColecao lr = new LineReportColecao();

                    lr.Grupo = item.grupo;
                    lr.Colecao = colecao.ToString();
                    lr.Descricao = baseController.BuscaColecaoAtual(colecao.ToString()).DESC_COLECAO;

                    lr.Qt_venda_semana = Convert.ToInt32(item.qt_venda_semana).ToString("###,###,##0");
                    lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                    total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana);
                    total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                    if (Convert.ToInt32(item.qt_venda_semana) > 0)
                        lr.Pc_medio_semana = (Convert.ToDecimal(item.vl_venda_semana) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                    lr.Qt_vd_sem_ano_pas = Convert.ToInt32(item.qt_venda_semana_ano_anterior).ToString("###,###,##0");
                    lr.Vl_vd_sem_ano_pas = Convert.ToDecimal(item.vl_venda_semana_ano_anterior).ToString("###,###,##0.00");

                    total_qt_venda_semana_ano_anterior += Convert.ToInt32(item.qt_venda_semana_ano_anterior);
                    total_vl_venda_semana_ano_anterior += Convert.ToDecimal(item.vl_venda_semana_ano_anterior);

                    lr.Qt_venda_mes = Convert.ToInt32(item.qt_venda_mes).ToString("###,###,##0");
                    lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                    total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes);
                    total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                    if (Convert.ToInt32(item.qt_venda_mes) > 0)
                        lr.Pc_medio_mes = (Convert.ToDecimal(item.vl_venda_mes) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                    lr.Qt_vd_mes_ano_pas = Convert.ToInt32(item.qt_venda_mes_ano_anterior).ToString("###,###,##0");
                    lr.Vl_vd_mes_ano_pas = Convert.ToDecimal(item.vl_venda_mes_ano_anterior).ToString("###,###,##0.00");

                    total_qt_venda_mes_ano_anterior += Convert.ToInt32(item.qt_venda_mes_ano_anterior);
                    total_vl_venda_mes_ano_anterior += Convert.ToDecimal(item.vl_venda_mes_ano_anterior);

                    lr.Dif_mes_qtde = (Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_venda_mes_ano_anterior)).ToString("###,###,##0");
                    lr.Dif_mes_valor = (Convert.ToInt32(item.vl_venda_mes) - Convert.ToInt32(item.vl_venda_mes_ano_anterior)).ToString("###,###,##0.00");
                    lr.Dif_2011_2012 = (Convert.ToInt32(item.vl_venda_mes) - Convert.ToInt32(item.vl_venda_mes_ano_anterior)).ToString("###,###,##0.00");

                    total_qt_dif += Convert.ToInt32(item.qt_venda_mes) - Convert.ToInt32(item.qt_venda_mes_ano_anterior);
                    total_vl_dif += Convert.ToInt32(item.vl_venda_mes) - Convert.ToInt32(item.vl_venda_mes_ano_anterior);

                    //int qt_estoque_1a_semana = 0;
                    //int qt_estoque_ultima_semana = 0;
                    /*
                    Sp_Line_Report_Estoque_Semana_Fechada_Loja_SubResult sp_Line_ReportEstoqueSemanaFechadaSubResult = baseController.BuscaEstoqueSemanaFechadaLojaSub(dataInicioSemana, dataFimSemana, item.grupo, griffe, colecao.ToString(), categoria, descricaoFilial);

                    if (sp_Line_ReportEstoqueSemanaFechadaSubResult != null)
                    {
                        if (sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL != null)
                        {
                            lr.Qt_estoque_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL).ToString("###,###,##0");
                            total_qt_estoque_semana += Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);

                            qt_estoque_ultima_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_FINAL);

                            lr.Vl_estoque_semana = Convert.ToDecimal(sp_Line_ReportEstoqueSemanaFechadaSubResult.VALOR_ESTOQUE_FINAL).ToString("###,###,##0.00");
                            total_vl_estoque_semana += Convert.ToDecimal(lr.Vl_estoque_semana);
                        }

                        if (sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL != null)
                            qt_estoque_1a_semana = Convert.ToInt32(sp_Line_ReportEstoqueSemanaFechadaSubResult.ESTOQUE_INICIAL);
                    }

                    if (((Convert.ToDecimal(qt_estoque_ultima_semana) + Convert.ToDecimal(qt_estoque_1a_semana)) / 2) > 0 && Convert.ToDecimal(item.qt_venda_semana) > 0)
                    {
                        count_giro++;

                        lr.Pc_qt_giro_antigo = (Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana)).ToString("###,###,##0.00");
                        total_pc_qt_giro_antigo += Convert.ToDecimal(qt_estoque_ultima_semana) / Convert.ToDecimal(item.qt_venda_semana);
                    }
                    */
                    lineReport.Add(lr);
                }
            }

            if (colecaoProduto != null || colecaoProdutoSub != null)
            {
                if (count_giro == 0)
                    count_giro = 1;

                LineReportColecao lr = new LineReportColecao();

                lr.Grupo = "Total";
                lr.Colecao = "Total";
                lr.Descricao = "Total";
                lr.Qt_venda_semana = total_qt_venda_semana.ToString("###,###,##0");
                lr.Vl_venda_semana = total_vl_venda_semana.ToString("###,###,##0.00");

                if (total_qt_venda_semana > 0)
                    lr.Pc_medio_semana = (total_vl_venda_semana / total_qt_venda_semana).ToString("###,###,##0.00");

                lr.Qt_vd_sem_ano_pas = total_qt_venda_semana_ano_anterior.ToString("###,###,##0");
                lr.Vl_vd_sem_ano_pas = total_qt_venda_semana_ano_anterior.ToString("###,###,##0.00");

                lr.Qt_venda_mes = total_qt_venda_mes.ToString("###,###,##0");
                lr.Vl_venda_mes = total_vl_venda_mes.ToString("###,###,##0.00");

                if (total_qt_venda_mes > 0)
                    lr.Pc_medio_mes = (total_vl_venda_mes / total_qt_venda_mes).ToString("###,###,##0.00");

                lr.Qt_vd_mes_ano_pas = total_qt_venda_mes_ano_anterior.ToString("###,###,##0");
                lr.Vl_vd_mes_ano_pas = total_qt_venda_mes_ano_anterior.ToString("###,###,##0.00");

                lr.Dif_mes_qtde = total_qt_dif.ToString("###,###,##0");
                lr.Dif_mes_valor = total_vl_dif.ToString("###,###,##0.00");
                lr.Dif_2011_2012 = total_vl_dif.ToString("###,###,##0.00");

                lr.Qt_estoque_semana = total_qt_estoque_semana.ToString("###,###,##0");
                lr.Vl_estoque_semana = total_vl_estoque_semana.ToString("###,###,##0.00");

                if (count_giro > 0)
                    lr.Pc_qt_giro_antigo = (total_pc_qt_giro_antigo / count_giro).ToString("###,###,##0.00");

                lineReport.Add(lr);
            }

            return lineReport;
        }

        public List<LineReportColecao> BuscaFranquiaMovimentoColecao(string categoria, string colecao, string anoSemana, string griffe)
        {
            string dataInicioSemana = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(baseController.BuscaDataFim(anoSemana));

            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);
            string ano = buscaData.ANO454;
            string anoAnterior = (Convert.ToInt32(buscaData.ANO454) - 1).ToString();
            string semana = anoSemana.Substring(4, 2);

            string dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMes = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            if (mes == 1)
            {
                mes = 12;
                ano = (Convert.ToInt32(ano) - 1).ToString();
            }
            else
                mes -= 1;

            string dataInicioMesFechado = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMesFechado = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            decimal total_vl_venda_semana = 0;
            decimal total_vl_venda_mes = 0;

            int total_qt_venda_semana = 0;
            int total_qt_venda_mes = 0;

            decimal total_vl_faturado_semana = 0;
            decimal total_vl_faturado_mes = 0;

            int total_qt_faturado_semana = 0;
            int total_qt_faturado_mes = 0;

            List<LineReportColecao> lineReport = new List<LineReportColecao>();

            List<Sp_Line_Report_Colecao_FranquiaResult> colecaoProdutoFranquia = null;
            List<Sp_Line_Report_Colecao_Sub_FranquiaResult> colecaoProdutoSubFranquia = null;

            List<Sp_Line_Report_Franquia_ColecaoResult> colecaoProduto = null;
            List<Sp_Line_Report_Franquia_Colecao_SubResult> colecaoProdutoSub = null;

            if (categoria.Equals("02"))
                colecaoProdutoSubFranquia = baseController.BuscaVendasColecaoSubFranquia(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimSemana);
            else
                colecaoProdutoFranquia = baseController.BuscaVendasColecaoFranquia(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimSemana);

            if (colecaoProdutoSubFranquia == null)
            {
                foreach (Sp_Line_Report_Colecao_FranquiaResult item in colecaoProdutoFranquia)
                {
                    LineReportColecao lr = new LineReportColecao();

                    lr.Grupo = item.grupo;
                    lr.Colecao = colecao;
                    lr.Descricao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO;

                    lr.Qt_venda_semana = Convert.ToInt32(item.qt_venda_semana).ToString("###,###,##0");
                    lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                    total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana);
                    total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                    if (Convert.ToInt32(item.qt_venda_semana) > 0)
                        lr.Pc_medio_semana = (Convert.ToDecimal(item.vl_venda_semana) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                    lr.Qt_venda_mes = Convert.ToInt32(item.qt_venda_mes).ToString("###,###,##0");
                    lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                    total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes);
                    total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                    if (Convert.ToInt32(item.qt_venda_mes) > 0)
                        lr.Pc_medio_mes = (Convert.ToDecimal(item.vl_venda_mes) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                    colecaoProduto = baseController.BuscaFranquiaVendasColecao(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, item.grupo, dataInicioMes, dataFimSemana, dataInicioMesFechado, dataFimMesFechado);

                    if (colecaoProduto != null)
                    {
                        lr.Qt_faturado_semana = Convert.ToInt32(item.qt_venda_semana);
                        lr.Vl_faturado_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                        total_qt_faturado_semana += lr.Qt_faturado_semana;
                        total_vl_faturado_semana += Convert.ToDecimal(item.vl_venda_semana);

                        if (lr.Qt_faturado_semana > 0)
                            lr.Pc_med_faturado_semana = (Convert.ToDecimal(item.vl_venda_semana) / lr.Qt_faturado_semana).ToString("###,###,##0.00");

                        lr.Qt_faturado_mes = Convert.ToInt32(item.qt_venda_mes);
                        lr.Vl_faturado_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                        total_qt_faturado_mes += lr.Qt_faturado_mes;
                        total_vl_faturado_mes += Convert.ToDecimal(item.vl_venda_mes);

                        if (lr.Qt_faturado_mes > 0)
                            lr.Pc_med_faturado_mes = (Convert.ToDecimal(item.vl_venda_mes) / lr.Qt_faturado_mes).ToString("###,###,##0.00");
                    }
                    
                    lineReport.Add(lr);
                }
            }
            else
            {
                foreach (Sp_Line_Report_Colecao_Sub_FranquiaResult item in colecaoProdutoSubFranquia)
                {
                    LineReportColecao lr = new LineReportColecao();

                    lr.Grupo = item.grupo;
                    lr.Colecao = colecao;
                    lr.Descricao = baseController.BuscaColecaoAtual(colecao).DESC_COLECAO;

                    lr.Qt_venda_semana = Convert.ToInt32(item.qt_venda_semana).ToString("###,###,##0");
                    lr.Vl_venda_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                    total_qt_venda_semana += Convert.ToInt32(item.qt_venda_semana);
                    total_vl_venda_semana += Convert.ToDecimal(item.vl_venda_semana);

                    if (Convert.ToInt32(item.qt_venda_semana) > 0)
                        lr.Pc_medio_semana = (Convert.ToDecimal(item.vl_venda_semana) / Convert.ToInt32(item.qt_venda_semana)).ToString("###,###,##0.00");

                    lr.Qt_venda_mes = Convert.ToInt32(item.qt_venda_mes).ToString("###,###,##0");
                    lr.Vl_venda_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                    total_qt_venda_mes += Convert.ToInt32(item.qt_venda_mes);
                    total_vl_venda_mes += Convert.ToDecimal(item.vl_venda_mes);

                    if (Convert.ToInt32(item.qt_venda_mes) > 0)
                        lr.Pc_medio_mes = (Convert.ToDecimal(item.vl_venda_mes) / Convert.ToInt32(item.qt_venda_mes)).ToString("###,###,##0.00");

                    colecaoProdutoSub = baseController.BuscaFranquiaVendasColecaoSub(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, item.grupo, dataInicioMes, dataFimSemana, dataInicioMesFechado, dataFimMesFechado);

                    if (colecaoProdutoSub != null)
                    {
                        lr.Qt_faturado_semana = Convert.ToInt32(item.qt_venda_semana);
                        lr.Vl_faturado_semana = Convert.ToDecimal(item.vl_venda_semana).ToString("###,###,##0.00");

                        total_qt_faturado_semana += lr.Qt_faturado_semana;
                        total_vl_faturado_semana += Convert.ToDecimal(item.vl_venda_semana);

                        if (lr.Qt_faturado_semana > 0)
                            lr.Pc_med_faturado_semana = (Convert.ToDecimal(item.vl_venda_semana) / lr.Qt_faturado_semana).ToString("###,###,##0.00");

                        lr.Qt_faturado_mes = Convert.ToInt32(item.qt_venda_mes);
                        lr.Vl_faturado_mes = Convert.ToDecimal(item.vl_venda_mes).ToString("###,###,##0.00");

                        total_qt_faturado_mes += lr.Qt_faturado_mes;
                        total_vl_faturado_mes += Convert.ToDecimal(item.vl_venda_mes);

                        if (lr.Qt_faturado_mes > 0)
                            lr.Pc_med_faturado_mes = (Convert.ToDecimal(item.vl_venda_mes) / lr.Qt_faturado_mes).ToString("###,###,##0.00");
                    }

                    lineReport.Add(lr);
                }
            }

            if (colecaoProdutoFranquia != null || colecaoProdutoSubFranquia != null)
            {
                LineReportColecao lr = new LineReportColecao();

                lr.Grupo = "Total";
                lr.Colecao = "Total";
                lr.Descricao = "Total";
                lr.Qt_venda_semana = total_qt_venda_semana.ToString("###,###,##0");
                lr.Vl_venda_semana = total_vl_venda_semana.ToString("###,###,##0.00");

                if (total_qt_venda_semana > 0)
                    lr.Pc_medio_semana = (total_vl_venda_semana / total_qt_venda_semana).ToString("###,###,##0.00");

                lr.Qt_venda_mes = total_qt_venda_mes.ToString("###,###,##0");
                lr.Vl_venda_mes = total_vl_venda_mes.ToString("###,###,##0.00");

                if (total_qt_venda_mes > 0)
                    lr.Pc_medio_mes = (total_vl_venda_mes / total_qt_venda_mes).ToString("###,###,##0.00");

                lr.Qt_faturado_semana = total_qt_faturado_semana;
                lr.Vl_faturado_semana = total_vl_faturado_semana.ToString("###,###,##0.00");

                if (total_qt_faturado_semana > 0)
                    lr.Pc_med_faturado_semana = (total_vl_faturado_semana / total_qt_faturado_semana).ToString("###,###,##0.00");

                lr.Qt_faturado_mes = total_qt_faturado_mes;
                lr.Vl_faturado_mes = total_vl_faturado_mes.ToString("###,###,##0.00");

                if (total_qt_faturado_mes > 0)
                    lr.Pc_med_faturado_mes = (total_vl_faturado_mes / total_qt_faturado_mes).ToString("###,###,##0.00");

                lineReport.Add(lr);
            }

            return lineReport;
        }

        public List<BranchReport> BuscaVendasLoja(string anoSemana, USUARIO usuario)
        {
            string dataInicioSemana = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(baseController.BuscaDataFim(anoSemana));

            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);
            string ano = buscaData.ANO454;
            string anoAnterior = (Convert.ToInt32(buscaData.ANO454) - 1).ToString();
            string semana = anoSemana.Substring(4, 2);

            string dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMes = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            decimal total_vl_venda_semana = 0;
            decimal total_vl_venda_mes = 0;
            decimal total_cota_mes = 0;
            int total_qt_venda_semana = 0;
            int total_qt_venda_mes = 0;
            int total_qt_estoque_semana = 0;

            List<BranchReport> branchReport = new List<BranchReport>();

            List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

            foreach (FILIAI item in filiais)
            {
                Sp_Branch_Report_Cota_LojaResult lojaCota = baseController.BuscaVendasLoja(item.COD_FILIAL, dataInicioSemana, dataFimSemana, dataInicioMes, dataFimSemana);

                if (lojaCota.Venda_Mes > 0)
                {
                    BranchReport br = new BranchReport();

                    br.Descricao = lojaCota.Filial;

                    br.Vl_venda_semana = Convert.ToDecimal(lojaCota.Venda_Semana).ToString("###,###,##0.00");
                    br.Qt_pecas_semana = Convert.ToInt32(lojaCota.Qtde_Semana);

                    total_vl_venda_semana += Convert.ToDecimal(lojaCota.Venda_Semana);
                    total_qt_venda_semana += Convert.ToInt32(lojaCota.Qtde_Semana);

                    if (Convert.ToInt32(lojaCota.Qtde_Semana) > 0)
                        br.Vl_medio_semana = (Convert.ToDecimal(lojaCota.Venda_Semana) / Convert.ToInt32(lojaCota.Qtde_Semana)).ToString("###,###,##0.00");

                    br.Qt_pecas_estoque_semana = Convert.ToInt32(lojaCota.Estoque_Atual);
                    total_qt_estoque_semana += Convert.ToInt32(lojaCota.Estoque_Atual);

                    br.Vl_venda_mes = Convert.ToDecimal(lojaCota.Venda_Mes).ToString("###,###,##0.00");
                    br.Qt_pecas_mes = Convert.ToInt32(lojaCota.Qtde_Mes);

                    total_vl_venda_mes += Convert.ToDecimal(lojaCota.Venda_Mes);
                    total_qt_venda_mes += Convert.ToInt32(lojaCota.Qtde_Mes);

                    if (Convert.ToInt32(lojaCota.Qtde_Mes) > 0)
                        br.Vl_medio_mes = (Convert.ToDecimal(lojaCota.Venda_Mes) / Convert.ToInt32(lojaCota.Qtde_Mes)).ToString("###,###,##0.00");

                    br.Vl_cota = Convert.ToDecimal(500000).ToString("###,###,##0.00");

                    total_cota_mes += Convert.ToDecimal(500000);

                    br.Perc_cota = ((Convert.ToDecimal(lojaCota.Venda_Mes) / Convert.ToDecimal(500000)) * 100).ToString("N2") + "%";

                    branchReport.Add(br);
                }
            }

            if (filiais.Count() > 0)
            {
                BranchReport br = new BranchReport();

                br.Descricao = usuario.NOME_USUARIO;
                br.Qt_pecas_semana = total_qt_venda_semana;
                br.Vl_venda_semana = total_vl_venda_semana.ToString("###,###,##0.00");
                br.Vl_medio_semana = (total_vl_venda_semana / total_qt_venda_semana).ToString("###,###,##0.00");
                br.Qt_pecas_estoque_semana = total_qt_estoque_semana;
                br.Qt_pecas_mes = total_qt_venda_mes;
                br.Vl_venda_mes = total_vl_venda_mes.ToString("###,###,##0.00");
                br.Vl_medio_mes = (total_vl_venda_mes / total_qt_venda_mes).ToString("###,###,##0.00");
                br.Vl_cota = total_cota_mes.ToString("###,###,##0.00");
                br.Perc_cota = ((Convert.ToDecimal(total_vl_venda_mes) / total_cota_mes) * 100).ToString("N2") + "%";

                branchReport.Add(br);
            }

            foreach (BranchReport item in branchReport)
            {
                item.Perc_venda_semana = ((Convert.ToDecimal(item.Vl_venda_semana) / total_vl_venda_semana) * 100).ToString("N2") + "%";
                item.Perc_estoque_atual = ((Convert.ToDecimal(item.Qt_pecas_estoque_semana) / total_qt_estoque_semana) * 100).ToString("N2") + "%";
            }

            return branchReport;
        }

        public List<BranchReport> BuscaEstoqueLoja(string anoSemana, string filial)
        {
            FILIAI f = baseController.BuscaFilial(filial);

            string dataInicioSemana = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(baseController.BuscaDataFim(anoSemana));

            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);
            string ano = buscaData.ANO454;
            string anoAnterior = (Convert.ToInt32(buscaData.ANO454) - 1).ToString();
            string semana = anoSemana.Substring(4, 2);

            string dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(mes, ano));
            string dataFimMes = AjustaData(baseController.BuscaDataFimMes(mes, ano));

            decimal total_vl_venda_semana = 0;
            decimal total_vl_venda_mes = 0;
            decimal total_cota_mes = 0;
            int total_qt_venda_semana = 0;
            int total_qt_venda_mes = 0;
            int total_qt_estoque_semana = 0;

            List<BranchReport> branchReport = new List<BranchReport>();

            List<Sp_Branch_Report_Cota_Loja_GrupoResult> lojaGrupoProduto = baseController.BuscaEstoqueLoja(f.COD_FILIAL, dataInicioSemana, dataFimSemana, dataInicioMes, dataFimSemana);

            foreach (Sp_Branch_Report_Cota_Loja_GrupoResult item in lojaGrupoProduto)
            {
                if (item.Venda_Mes > 0)
                {
                    BranchReport br = new BranchReport();

                    br.Descricao = item.Grupo;

                    br.Vl_venda_semana = Convert.ToDecimal(item.Venda_Semana).ToString("###,###,##0.00");
                    br.Qt_pecas_semana = Convert.ToInt32(item.Qtde_Semana);

                    total_vl_venda_semana += Convert.ToDecimal(item.Venda_Semana);
                    total_qt_venda_semana += Convert.ToInt32(item.Qtde_Semana);

                    if (Convert.ToInt32(item.Qtde_Semana) > 0)
                        br.Vl_medio_semana = (Convert.ToDecimal(item.Venda_Semana) / Convert.ToInt32(item.Qtde_Semana)).ToString("###,###,##0.00");

                    br.Qt_pecas_estoque_semana = Convert.ToInt32(item.Estoque_Atual);
                    total_qt_estoque_semana += Convert.ToInt32(item.Estoque_Atual);

                    br.Vl_venda_mes = Convert.ToDecimal(item.Venda_Mes).ToString("###,###,##0.00");
                    br.Qt_pecas_mes = Convert.ToInt32(item.Qtde_Mes);

                    total_vl_venda_mes += Convert.ToDecimal(item.Venda_Mes);
                    total_qt_venda_mes += Convert.ToInt32(item.Qtde_Mes);

                    if (Convert.ToInt32(item.Qtde_Mes) > 0)
                        br.Vl_medio_mes = (Convert.ToDecimal(item.Venda_Mes) / Convert.ToInt32(item.Qtde_Mes)).ToString("###,###,##0.00");

                    br.Vl_cota = Convert.ToDecimal(500000).ToString("###,###,##0.00");

                    total_cota_mes += Convert.ToDecimal(500000);

                    br.Perc_cota = ((Convert.ToDecimal(item.Venda_Mes) / Convert.ToDecimal(500000)) * 100).ToString("N2") + "%";

                    branchReport.Add(br);
                }
            }

            if (lojaGrupoProduto.Count() > 0)
            {
                BranchReport br = new BranchReport();

                br.Descricao = "Total";
                br.Qt_pecas_semana = total_qt_venda_semana;
                br.Vl_venda_semana = total_vl_venda_semana.ToString("###,###,##0.00");
                br.Vl_medio_semana = (total_vl_venda_semana / total_qt_venda_semana).ToString("###,###,##0.00");
                br.Qt_pecas_estoque_semana = total_qt_estoque_semana;
                br.Qt_pecas_mes = total_qt_venda_mes;
                br.Vl_venda_mes = total_vl_venda_mes.ToString("###,###,##0.00");
                br.Vl_medio_mes = (total_vl_venda_mes / total_qt_venda_mes).ToString("###,###,##0.00");
                br.Vl_cota = total_cota_mes.ToString("###,###,##0.00");
                br.Perc_cota = ((Convert.ToDecimal(total_vl_venda_mes) / total_cota_mes) * 100).ToString("N2") + "%";

                branchReport.Add(br);
            }

            foreach (BranchReport item in branchReport)
            {
                item.Perc_venda_semana = ((Convert.ToDecimal(item.Vl_venda_semana) / total_vl_venda_semana) * 100).ToString("N2") + "%";
                item.Perc_estoque_atual = ((Convert.ToDecimal(item.Qt_pecas_estoque_semana) / total_qt_estoque_semana) * 100).ToString("N2") + "%";
            }

            return branchReport;
        }

        public List<ReportVendas> BuscaVendasPeriodo(string anoSemana, USUARIO usuario)
        {
            string dataInicioAno = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimAno = AjustaData(baseController.BuscaDataFim(anoSemana));

            DATA buscaData = baseController.BuscaDatas(anoSemana);

            int mes = Convert.ToInt32(buscaData.MES454);
            string ano = buscaData.ANO454;
            string anoAnterior = (Convert.ToInt32(buscaData.ANO454) - 1).ToString();
            string semana = anoSemana.Substring(4, 2);

            anoSemana = anoAnterior + semana;

            string dataInicioAnoAnterior = AjustaData(baseController.BuscaDataInicio(anoSemana));
            string dataFimAnoAnterior = AjustaData(baseController.BuscaDataFim(anoSemana));

            List<ReportVendas> vendaReport = new List<ReportVendas>();

            List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

            foreach (FILIAI item in filiais)
            {
                Sp_Analise_Venda_PeriodoResult analiseAtende = baseController.BuscaVendasPeriodo(item.COD_FILIAL, dataInicioAnoAnterior, dataFimAnoAnterior, dataInicioAno, dataFimAno);

                ReportVendas rv = new ReportVendas();

                rv.Item = baseController.BuscaDescricaoFilial(item.COD_FILIAL).FILIAL;

                rv.Ticket = Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior).ToString("#,###,##0");
                rv.Valor = Convert.ToDecimal(analiseAtende.Valor_Ano_Anterior).ToString("###,###,##0.00");
                rv.Peca = Convert.ToInt32(analiseAtende.Peca_Ano_Anterior).ToString("#,###,##0");

                rv.Ticket_2 = Convert.ToInt32(analiseAtende.Ticket_Ano).ToString("#,###,##0");
                rv.Valor_2 = Convert.ToDecimal(analiseAtende.Valor_Ano).ToString("###,###,##0.00");
                rv.Peca_2 = Convert.ToInt32(analiseAtende.Peca_Ano).ToString("#,###,##0");

                if (analiseAtende.Ticket_Ano_Anterior > 0)
                    rv.Perc_ticket = (1-(Convert.ToDecimal(analiseAtende.Ticket_Ano) / Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior))).ToString("###,###,##0.00");

                if (analiseAtende.Valor_Ano_Anterior > 0)
                    rv.Perc_valor = (1-(Convert.ToDecimal(analiseAtende.Valor_Ano) / Convert.ToInt32(analiseAtende.Valor_Ano_Anterior))).ToString("###,###,##0.00");

                rv.Difer_ticket = (Convert.ToInt32(analiseAtende.Ticket_Ano) - Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior)).ToString("#,###,##0");

                if (analiseAtende.Ticket_Ano > 0)
                {
                    decimal perda = (Convert.ToDecimal(analiseAtende.Valor_Ano) / Convert.ToInt32(analiseAtende.Ticket_Ano)) * (Convert.ToInt32(analiseAtende.Ticket_Ano) - Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior));

                    if (perda < 0)
                        perda = perda * -1;

                    rv.Media_ticket = perda.ToString("###,###,##0.00");
                    rv.Futuro = ((perda + Convert.ToDecimal(analiseAtende.Valor_Ano)) - Convert.ToDecimal(analiseAtende.Valor_Ano_Anterior)).ToString("###,###,##0.00");
                }

                if (analiseAtende.Ticket_Ano_Anterior > 0)
                    rv.Pa = (Convert.ToDecimal(analiseAtende.Peca_Ano_Anterior) / Convert.ToDecimal(analiseAtende.Ticket_Ano_Anterior)).ToString("###,###,##0.00");
                if (analiseAtende.Ticket_Ano > 0)
                    rv.Pa_2 = (Convert.ToDecimal(analiseAtende.Peca_Ano) / Convert.ToDecimal(analiseAtende.Ticket_Ano)).ToString("###,###,##0.00");

                vendaReport.Add(rv);
            }

            return vendaReport;
        }

        public List<ReportVendas> BuscaVendasPeriodoLoja(string loja)
        {
            List<int> list = new List<int>();

            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Add(7);
            list.Add(8);
            list.Add(9);
            list.Add(10);
            list.Add(11);
            list.Add(12);

            int ano = DateTime.Now.Year;
            int anoAnterior = DateTime.Now.Year - 1;

            string dataInicioMes = null;
            string dataFimMes = null;
            string dataInicioMesAnoAnterior = null;
            string dataFimMesAnoAnterior = null;

            List<ReportVendas> vendaReport = new List<ReportVendas>();

            foreach (int item in list)
            {
                dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(item, ano.ToString()));
                dataFimMes = AjustaData(baseController.BuscaDataFimMes(item, ano.ToString()));

                dataInicioMesAnoAnterior = AjustaData(baseController.BuscaDataInicioMes(item, anoAnterior.ToString()));
                dataFimMesAnoAnterior = AjustaData(baseController.BuscaDataFimMes(item, anoAnterior.ToString()));

                Sp_Analise_Venda_PeriodoResult analiseAtende = baseController.BuscaVendasPeriodo(loja, dataInicioMesAnoAnterior, dataFimMesAnoAnterior, dataInicioMes, dataFimMes);

                ReportVendas rv = new ReportVendas();

                rv.Item = baseController.BuscaDescricaoMes(item);

                rv.Ticket = Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior).ToString("#,###,##0");
                rv.Valor = Convert.ToDecimal(analiseAtende.Valor_Ano_Anterior).ToString("###,###,##0.00");
                rv.Peca = Convert.ToInt32(analiseAtende.Peca_Ano_Anterior).ToString("#,###,##0");

                rv.Ticket_2 = Convert.ToInt32(analiseAtende.Ticket_Ano).ToString("#,###,##0");
                rv.Valor_2 = Convert.ToDecimal(analiseAtende.Valor_Ano).ToString("###,###,##0.00");
                rv.Peca_2 = Convert.ToInt32(analiseAtende.Peca_Ano).ToString("#,###,##0");

                if (analiseAtende.Ticket_Ano_Anterior > 0)
                    rv.Perc_ticket = (1-(Convert.ToDecimal(analiseAtende.Ticket_Ano) / Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior))).ToString("###,###,##0.00");

                if (analiseAtende.Valor_Ano_Anterior > 0)
                    rv.Perc_valor = (1-(Convert.ToDecimal(analiseAtende.Valor_Ano) / Convert.ToInt32(analiseAtende.Valor_Ano_Anterior))).ToString("###,###,##0.00");

                rv.Difer_ticket = (Convert.ToInt32(analiseAtende.Ticket_Ano) - Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior)).ToString("#,###,##0");

                if (analiseAtende.Ticket_Ano > 0)
                {
                    decimal perda = (Convert.ToDecimal(analiseAtende.Valor_Ano) / Convert.ToInt32(analiseAtende.Ticket_Ano)) * (Convert.ToInt32(analiseAtende.Ticket_Ano) - Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior));

                    if (perda < 0)
                        perda = perda * -1;

                    rv.Media_ticket = perda.ToString("###,###,##0.00");
                    rv.Futuro = ((perda + Convert.ToDecimal(analiseAtende.Valor_Ano)) - Convert.ToDecimal(analiseAtende.Valor_Ano_Anterior)).ToString("###,###,##0.00");
                }

                if (analiseAtende.Ticket_Ano_Anterior > 0)
                    rv.Pa = (Convert.ToDecimal(analiseAtende.Peca_Ano_Anterior) / Convert.ToDecimal(analiseAtende.Ticket_Ano_Anterior)).ToString("###,###,##0.00");
                if (analiseAtende.Ticket_Ano > 0)
                    rv.Pa_2 = (Convert.ToDecimal(analiseAtende.Peca_Ano) / Convert.ToDecimal(analiseAtende.Ticket_Ano)).ToString("###,###,##0.00");

                vendaReport.Add(rv);
            }

            return vendaReport;
        }

        public List<ReportVendas> BuscaVendasPeriodoGrupo(USUARIO usuario)
        {
            List<int> list = new List<int>();

            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Add(7);
            list.Add(8);
            list.Add(9);
            list.Add(10);
            list.Add(11);
            list.Add(12);

            int ano = DateTime.Now.Year;
            int anoAnterior = DateTime.Now.Year - 1;

            string dataInicioMes = null;
            string dataFimMes = null;
            string dataInicioMesAnoAnterior = null;
            string dataFimMesAnoAnterior = null;

            List<ReportVendas> vendaReport = new List<ReportVendas>();

            foreach (int item in list)
            {
                dataInicioMes = AjustaData(baseController.BuscaDataInicioMes(item, ano.ToString()));
                dataFimMes = AjustaData(baseController.BuscaDataFimMes(item, ano.ToString()));

                dataInicioMesAnoAnterior = AjustaData(baseController.BuscaDataInicioMes(item, anoAnterior.ToString()));
                dataFimMesAnoAnterior = AjustaData(baseController.BuscaDataFimMes(item, anoAnterior.ToString()));

                List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                int total_ticket = 0;
                decimal total_valor = 0;
                int total_pecas = 0;

                int total_ticket_ano_anterior = 0;
                decimal total_valor_ano_anterior = 0;
                int total_pecas_ano_anterior = 0;

                foreach (FILIAI itemFilial in filiais)
                {
                    Sp_Analise_Venda_PeriodoResult analiseAtende = baseController.BuscaVendasPeriodo(itemFilial.COD_FILIAL, dataInicioMesAnoAnterior, dataFimMesAnoAnterior, dataInicioMes, dataFimMes);

                    total_ticket += Convert.ToInt32(analiseAtende.Ticket_Ano);
                    total_valor += Convert.ToDecimal(analiseAtende.Valor_Ano);
                    total_pecas += Convert.ToInt32(analiseAtende.Peca_Ano);

                    total_ticket_ano_anterior += Convert.ToInt32(analiseAtende.Ticket_Ano_Anterior);
                    total_valor_ano_anterior += Convert.ToDecimal(analiseAtende.Valor_Ano_Anterior);
                    total_pecas_ano_anterior += Convert.ToInt32(analiseAtende.Peca_Ano_Anterior);
                }

                ReportVendas rv = new ReportVendas();

                rv.Item = baseController.BuscaDescricaoMes(item);

                rv.Ticket = total_ticket_ano_anterior.ToString("#,###,##0");
                rv.Valor = total_valor_ano_anterior.ToString("###,###,##0.00");
                rv.Peca = total_pecas_ano_anterior.ToString("#,###,##0");

                rv.Ticket_2 = total_ticket.ToString("#,###,##0");
                rv.Valor_2 = total_valor.ToString("###,###,##0.00");
                rv.Peca_2 = total_pecas.ToString("#,###,##0");

                if (total_ticket_ano_anterior > 0)
                    rv.Perc_ticket = (1-(Convert.ToDecimal(total_ticket) / Convert.ToDecimal(total_ticket_ano_anterior))).ToString("###,###,##0.00");

                if (total_valor_ano_anterior > 0)
                    rv.Perc_valor = (1-(Convert.ToDecimal(total_valor) / Convert.ToDecimal(total_valor_ano_anterior))).ToString("###,###,##0.00");

                rv.Difer_ticket = (total_ticket - total_ticket_ano_anterior).ToString("#,###,##0");

                if (total_ticket > 0)
                {
                    decimal perda = (total_valor / total_ticket) * (total_ticket - total_ticket_ano_anterior);

                    if (perda < 0)
                        perda = perda * -1;

                    rv.Media_ticket = perda.ToString("###,###,##0.00");
                    rv.Futuro = ((perda + total_valor) - total_valor_ano_anterior).ToString("###,###,##0.00");
                }

                if (total_ticket_ano_anterior > 0)
                    rv.Pa = (Convert.ToDecimal(total_pecas_ano_anterior) / Convert.ToDecimal(total_ticket_ano_anterior)).ToString("###,###,##0.00");
                if (total_ticket > 0)
                    rv.Pa_2 = (Convert.ToDecimal(total_pecas) / Convert.ToDecimal(total_ticket)).ToString("###,###,##0.00");

                vendaReport.Add(rv);
            }

            return vendaReport;
        }

        public string AjustaData(string data)
        {
            string mes = "";
            string dia = "";
            string dataAjustada = "";

            if (data.Substring(data.Length - 16, 1).Equals("/") || data.Substring(data.Length - 16, 1).Equals("-"))
                mes = "0" + data.Substring(data.Length - 15, 1);

            if (data.Substring(1, 1).Equals("/") || data.Substring(1, 1).Equals("-"))
                dia = "0" + data.Substring(0, 1);

            if (mes.Equals("") && dia.Equals(""))
                dataAjustada = data.Substring(6, 4) + data.Substring(3, 2) + data.Substring(0, 2);

            if (mes.Equals("") && !dia.Equals(""))
                dataAjustada = data.Substring(5, 4) + data.Substring(2, 2) + dia;

            if (!mes.Equals("") && dia.Equals(""))
                dataAjustada = data.Substring(5, 4) + mes + data.Substring(0, 2);

            if (!mes.Equals("") && !dia.Equals(""))
                dataAjustada = data.Substring(4, 4) + mes + dia;

            return dataAjustada;
        }
    }
}
