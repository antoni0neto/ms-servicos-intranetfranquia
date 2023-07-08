using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class VendasCotasLoja
{
    private string filial;

    public string Filial
    {
        get { return filial; }
        set { filial = value; }
    }

    private decimal valAtingido;
    public decimal ValAtingido
    {
        get { return valAtingido; }
        set { valAtingido = value; }
    }

    private decimal valCota;
    public decimal ValCota
    {
        get { return valCota; }
        set { valCota = value; }
    }

    private decimal percAtingido;
    public decimal PercAtingido
    {
        get { return percAtingido; }
        set { percAtingido = value; }
    }

    private decimal percAtingidoAnoAnterior;
    public decimal PercAtingidoAnoAnterior
    {
        get { return percAtingidoAnoAnterior; }
        set { percAtingidoAnoAnterior = value; }
    }

    private string gerente;

    public string Gerente
    {
        get { return gerente; }
        set { gerente = value; }
    }

    private string supervisor;

    public string Supervisor
    {
        get { return supervisor; }
        set { supervisor = value; }
    }

    private int codigoFarol;

    public int CodigoFarol
    {
        get { return codigoFarol; }
        set { codigoFarol = value; }
    }

    private bool sobe;

    public bool Sobe
    {
        get { return sobe; }
        set { sobe = value; }
    }
}