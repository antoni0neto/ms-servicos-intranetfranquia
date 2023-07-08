using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class LineReportColecao
{
    private string grupo;

    public string Grupo
    {
        get { return grupo; }
        set { grupo = value; }
    }

    private string colecao;

    public string Colecao
    {
        get { return colecao; }
        set { colecao = value; }
    }

    private string descricao;

    public string Descricao
    {
        get { return descricao; }
        set { descricao = value; }
    }

    private string qt_venda_semana;

    public string Qt_venda_semana
    {
        get { return qt_venda_semana; }
        set { qt_venda_semana = value; }
    }

    private string vl_venda_semana;

    public string Vl_venda_semana
    {
        get { return vl_venda_semana; }
        set { vl_venda_semana = value; }
    }

    private string pc_medio_semana;

    public string Pc_medio_semana
    {
        get { return pc_medio_semana; }
        set { pc_medio_semana = value; }
    }

    private string var_vl_plano_semana;

    public string Var_vl_plano_semana
    {
        get { return var_vl_plano_semana; }
        set { var_vl_plano_semana = value; }
    }
    
    private string qt_venda_mes;

    public string Qt_venda_mes
    {
        get { return qt_venda_mes; }
        set { qt_venda_mes = value; }
    }

    private string vl_venda_mes;

    public string Vl_venda_mes
    {
        get { return vl_venda_mes; }
        set { vl_venda_mes = value; }
    }

    private string pc_medio_mes;

    public string Pc_medio_mes
    {
        get { return pc_medio_mes; }
        set { pc_medio_mes = value; }
    }

    private string var_vl_plano_mes;

    public string Var_vl_plano_mes
    {
        get { return var_vl_plano_mes; }
        set { var_vl_plano_mes = value; }
    }

    private string var_markup_original;

    public string Var_markup_original
    {
        get { return var_markup_original; }
        set { var_markup_original = value; }
    }

    private string var_markup_original_antigo;

    public string Var_markup_original_antigo
    {
        get { return var_markup_original_antigo; }
        set { var_markup_original_antigo = value; }
    }

    private string var_markup_liquido;

    public string Var_markup_liquido
    {
        get { return var_markup_liquido; }
        set { var_markup_liquido = value; }
    }

    private string var_markup_liquido_antigo;

    public string Var_markup_liquido_antigo
    {
        get { return var_markup_liquido_antigo; }
        set { var_markup_liquido_antigo = value; }
    }

    private string qt_estoque_semana;

    public string Qt_estoque_semana
    {
        get { return qt_estoque_semana; }
        set { qt_estoque_semana = value; }
    }

    private string qt_estoque_ini_semana;

    public string Qt_estoque_ini_semana
    {
        get { return qt_estoque_ini_semana; }
        set { qt_estoque_ini_semana = value; }
    }

    private string vl_estoque_semana;

    public string Vl_estoque_semana
    {
        get { return vl_estoque_semana; }
        set { vl_estoque_semana = value; }
    }

    private string smu;

    public string Smu
    {
        get { return smu; }
        set { smu = value; }
    }

    private string vl_remarcacao;

    public string Vl_remarcacao
    {
        get { return vl_remarcacao; }
        set { vl_remarcacao = value; }
    }

    private string pc_qt_giro;

    public string Pc_qt_giro
    {
        get { return pc_qt_giro; }
        set { pc_qt_giro = value; }
    }

    private string pc_qt_giro_antigo;

    public string Pc_qt_giro_antigo
    {
        get { return pc_qt_giro_antigo; }
        set { pc_qt_giro_antigo = value; }
    }

    private string qt_estoque_sobra;

    public string Qt_estoque_sobra
    {
        get { return qt_estoque_sobra; }
        set { qt_estoque_sobra = value; }
    }

    private string vl_estoque_sobra;

    public string Vl_estoque_sobra
    {
        get { return vl_estoque_sobra; }
        set { vl_estoque_sobra = value; }
    }

    private string qt_estoque_fabrica;

    public string Qt_estoque_fabrica
    {
        get { return qt_estoque_fabrica; }
        set { qt_estoque_fabrica = value; }
    }

    private string vl_estoque_fabrica;

    public string Vl_estoque_fabrica
    {
        get { return vl_estoque_fabrica; }
        set { vl_estoque_fabrica = value; }
    }

    private string qt_vd_sem_ano_pas;

    public string Qt_vd_sem_ano_pas
    {
        get { return qt_vd_sem_ano_pas; }
        set { qt_vd_sem_ano_pas = value; }
    }

    private string vl_vd_sem_ano_pas;

    public string Vl_vd_sem_ano_pas
    {
        get { return vl_vd_sem_ano_pas; }
        set { vl_vd_sem_ano_pas = value; }
    }

    private string cota_semana_qtde;

    public string Cota_semana_qtde
    {
        get { return cota_semana_qtde; }
        set { cota_semana_qtde = value; }
    }

    private string cota_semana_valor;

    public string Cota_semana_valor
    {
        get { return cota_semana_valor; }
        set { cota_semana_valor = value; }
    }

    private string dif_semana_qtde;

    public string Dif_semana_qtde
    {
        get { return dif_semana_qtde; }
        set { dif_semana_qtde = value; }
    }

    private string dif_semana_valor;

    public string Dif_semana_valor
    {
        get { return dif_semana_valor; }
        set { dif_semana_valor = value; }
    }
    private string qt_vd_mes_ano_pas;

    public string Qt_vd_mes_ano_pas
    {
        get { return qt_vd_mes_ano_pas; }
        set { qt_vd_mes_ano_pas = value; }
    }

    private string vl_vd_mes_ano_pas;

    public string Vl_vd_mes_ano_pas
    {
        get { return vl_vd_mes_ano_pas; }
        set { vl_vd_mes_ano_pas = value; }
    }

    private string cota_mes_qtde;

    public string Cota_mes_qtde
    {
        get { return cota_mes_qtde; }
        set { cota_mes_qtde = value; }
    }

    private string cota_mes_valor;

    public string Cota_mes_valor
    {
        get { return cota_mes_valor; }
        set { cota_mes_valor = value; }
    }

    private string dif_mes_qtde;

    public string Dif_mes_qtde
    {
        get { return dif_mes_qtde; }
        set { dif_mes_qtde = value; }
    }

    private string dif_mes_valor;

    public string Dif_mes_valor
    {
        get { return dif_mes_valor; }
        set { dif_mes_valor = value; }
    }

    private string dif_2011_2012;

    public string Dif_2011_2012
    {
        get { return dif_2011_2012; }
        set { dif_2011_2012 = value; }
    }

    private int qt_faturado_semana;

    public int Qt_faturado_semana
    {
        get { return qt_faturado_semana; }
        set { qt_faturado_semana = value; }
    }

    private string vl_faturado_semana;

    public string Vl_faturado_semana
    {
        get { return vl_faturado_semana; }
        set { vl_faturado_semana = value; }
    }

    private string pc_med_faturado_semana;

    public string Pc_med_faturado_semana
    {
        get { return pc_med_faturado_semana; }
        set { pc_med_faturado_semana = value; }
    }

    private int qt_faturado_mes;

    public int Qt_faturado_mes
    {
        get { return qt_faturado_mes; }
        set { qt_faturado_mes = value; }
    }

    private string vl_faturado_mes;

    public string Vl_faturado_mes
    {
        get { return vl_faturado_mes; }
        set { vl_faturado_mes = value; }
    }

    private string pc_med_faturado_mes;

    public string Pc_med_faturado_mes
    {
        get { return pc_med_faturado_mes; }
        set { pc_med_faturado_mes = value; }
    }
}