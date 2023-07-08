using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class BranchReport
{
    public BranchReport()
    {
    }

    public string descricao;

    public string Descricao
    {
        get { return descricao; }
        set { descricao = value; }
    }

    private string vl_venda_semana;

    public string Vl_venda_semana
    {
        get { return vl_venda_semana; }
        set { vl_venda_semana = value; }
    }

    private int qt_pecas_estoque_semana;

    public int Qt_pecas_estoque_semana
    {
        get { return qt_pecas_estoque_semana; }
        set { qt_pecas_estoque_semana = value; }
    }

    private string vl_medio_semana;

    public string Vl_medio_semana
    {
        get { return vl_medio_semana; }
        set { vl_medio_semana = value; }
    }

    private string perc_venda_semana;

    public string Perc_venda_semana
    {
        get { return perc_venda_semana; }
        set { perc_venda_semana = value; }
    }

    private int qt_pecas_semana;

    public int Qt_pecas_semana
    {
        get { return qt_pecas_semana; }
        set { qt_pecas_semana = value; }
    }

    private string perc_estoque_atual;

    public string Perc_estoque_atual
    {
        get { return perc_estoque_atual; }
        set { perc_estoque_atual = value; }
    }

    private string vl_venda_mes;

    public string Vl_venda_mes
    {
        get { return vl_venda_mes; }
        set { vl_venda_mes = value; }
    }

    private int qt_pecas_mes;

    public int Qt_pecas_mes
    {
        get { return qt_pecas_mes; }
        set { qt_pecas_mes = value; }
    }

    private string vl_medio_mes;

    public string Vl_medio_mes
    {
        get { return vl_medio_mes; }
        set { vl_medio_mes = value; }
    }

    private string vl_cota;

    public string Vl_cota
    {
        get { return vl_cota; }
        set { vl_cota = value; }
    }

    private string perc_cota;

    public string Perc_cota
    {
        get { return perc_cota; }
        set { perc_cota = value; }
    }
}