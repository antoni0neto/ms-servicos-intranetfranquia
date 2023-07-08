using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class Vendas
{

    private string codigoFilial;

    public string CodigoFilial
    {
        get { return codigoFilial; }
        set { codigoFilial = value; }
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
}