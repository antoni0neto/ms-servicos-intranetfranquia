using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class VendasCotas
{
    private string data;

    public string Data
    {
        get { return data; }
        set { data = value; }
    }

    private decimal valorVenda;

    public decimal ValorVenda
    {
        get { return valorVenda; }
        set { valorVenda = value; }
    }

    private decimal valorCota;

    public decimal ValorCota
    {
        get { return valorCota; }
        set { valorCota = value; }
    }

    private decimal percAtingido;

    public decimal PercAtingido
    {
        get { return percAtingido; }
        set { percAtingido = value; }
    }
}