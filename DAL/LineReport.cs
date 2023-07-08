using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class LineReport
{
    private string grupo_produto;

    public string Grupo_produto
    {
        get { return grupo_produto; }
        set { grupo_produto = value; }
    }

    private string subgrupo_produto;

    public string SubGrupo_produto
    {
        get { return subgrupo_produto; }
        set { subgrupo_produto = value; }
    }

    private int qt_venda_semana;

    public int Qt_venda_semana
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

    private string var_vl_semana_ano_anterior;

    public string Var_vl_semana_ano_anterior
    {
        get { return var_vl_semana_ano_anterior; }
        set { var_vl_semana_ano_anterior = value; }
    }

    private string var_vl_plano_semana;

    public string Var_vl_plano_semana
    {
        get { return var_vl_plano_semana; }
        set { var_vl_plano_semana = value; }
    }
    
    private int qt_venda_mes;

    public int Qt_venda_mes
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

    private string var_vl_mes_ano_anterior;

    public string Var_vl_mes_ano_anterior
    {
        get { return var_vl_mes_ano_anterior; }
        set { var_vl_mes_ano_anterior = value; }
    }

    private string var_vl_plano_mes;

    public string Var_vl_plano_mes
    {
        get { return var_vl_plano_mes; }
        set { var_vl_plano_mes = value; }
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

    private string qt_estoque_semana;

    public string Qt_estoque_semana
    {
        get { return qt_estoque_semana; }
        set { qt_estoque_semana = value; }
    }

    private string vl_estoque_semana;

    public string Vl_estoque_semana
    {
        get { return vl_estoque_semana; }
        set { vl_estoque_semana = value; }
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

    private string cota_semana_1;

    public string Cota_semana_1
    {
        get { return cota_semana_1; }
        set { cota_semana_1 = value; }
    }

    private string cota_semana_2;

    public string Cota_semana_2
    {
        get { return cota_semana_2; }
        set { cota_semana_2 = value; }
    }

    private string cota_semana_3;

    public string Cota_semana_3
    {
        get { return cota_semana_3; }
        set { cota_semana_3 = value; }
    }

    private string cota_mes_1;

    public string Cota_mes_1
    {
        get { return cota_mes_1; }
        set { cota_mes_1 = value; }
    }

    private string cota_mes_2;

    public string Cota_mes_2
    {
        get { return cota_mes_2; }
        set { cota_mes_2 = value; }
    }

    private string cota_mes_3;

    public string Cota_mes_3
    {
        get { return cota_mes_3; }
        set { cota_mes_3 = value; }
    }
}