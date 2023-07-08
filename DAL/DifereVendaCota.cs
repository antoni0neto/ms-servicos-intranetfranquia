using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class DifereVendaCota
{
    private string supervisor;

    public string Supervisor
    {
        get { return supervisor; }
        set { supervisor = value; }
    }

    private string filial;

    public string Filial
    {
        get { return filial; }
        set { filial = value; }
    }

    private decimal valorVendaAtual;

    public decimal ValorVendaAtual
    {
        get { return valorVendaAtual; }
        set { valorVendaAtual = value; }
    }

    private decimal valorVendaAnterior;

    public decimal ValorVendaAnterior
    {
        get { return valorVendaAnterior; }
        set { valorVendaAnterior = value; }
    }

    private decimal diferenca;

    public decimal Diferenca
    {
        get { return diferenca; }
        set { diferenca = value; }
    }

    private int percAtingido;

    public int PercAtingido
    {
        get { return percAtingido; }
        set { percAtingido = value; }
    }

    private int diferencaAtingido;

    public int DiferencaAtingido
    {
        get { return diferencaAtingido; }
        set { diferencaAtingido = value; }
    }

    private decimal? valorCota;

    public decimal? ValorCota
    {
        get { return valorCota; }
        set { valorCota = value; }
    }
}