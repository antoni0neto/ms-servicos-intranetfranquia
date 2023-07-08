using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class VendasBermuda
{
    private string filial;

    public string Filial
    {
        get { return filial; }
        set { filial = value; }
    }

    private string qtde;

    public string Qtde
    {
        get { return qtde; }
        set { qtde = value; }
    }

    private string cota;

    public string Cota
    {
        get { return cota; }
        set { cota = value; }
    }

    private string percAtingido;

    public string PercAtingido
    {
        get { return percAtingido; }
        set { percAtingido = value; }
    }

    private string data_minima;

    public string Data_minima
    {
        get { return data_minima; }
        set { data_minima = value; }
    }

    private string data_maxima;

    public string Data_maxima
    {
        get { return data_maxima; }
        set { data_maxima = value; }
    }
}