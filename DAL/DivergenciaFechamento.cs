using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class DivergenciaFechamento
{
    private string filial;

    public string Filial
    {
        get { return filial; }
        set { filial = value; }
    }

    private DateTime data;

    public DateTime Data
    {
        get { return data; }
        set { data = value; }
    }

    private decimal valorDinheiroLinx;

    public decimal ValorDinheiroLinx
    {
        get { return valorDinheiroLinx; }
        set { valorDinheiroLinx = value; }
    }

    private decimal valorCartaoLinx;

    public decimal ValorCartaoLinx
    {
        get { return valorCartaoLinx; }
        set { valorCartaoLinx = value; }
    }

    private decimal valorOutrosLinx;

    public decimal ValorOutrosLinx
    {
        get { return valorOutrosLinx; }
        set { valorOutrosLinx = value; }
    }

    private decimal valorDinheiroIntranet;

    public decimal ValorDinheiroIntranet
    {
        get { return valorDinheiroIntranet; }
        set { valorDinheiroIntranet = value; }
    }

    private decimal valorCartaoIntranet;

    public decimal ValorCartaoIntranet
    {
        get { return valorCartaoIntranet; }
        set { valorCartaoIntranet = value; }
    }

    private decimal valorOutrosIntranet;

    public decimal ValorOutrosIntranet
    {
        get { return valorOutrosIntranet; }
        set { valorOutrosIntranet = value; }
    }

    private decimal valorComandaIntranet;

    public decimal ValorComandaIntranet
    {
        get { return valorComandaIntranet; }
        set { valorComandaIntranet = value; }
    }
}