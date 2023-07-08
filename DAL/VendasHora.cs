using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class VendasHora
{
    private string diaSemana;

    public string DiaSemana
    {
        get { return diaSemana; }
        set { diaSemana = value; }
    }

    private string hora;

    public string Hora
    {
        get { return hora; }
        set { hora = value; }
    }

    private decimal valor;

    public decimal Valor
    {
        get { return valor; }
        set { valor = value; }
    }

    private int qtVendedor;

    public int QtVendedor
    {
        get { return qtVendedor; }
        set { qtVendedor = value; }
    }

    private int qtTicket;

    public int QtTicket
    {
        get { return qtTicket; }
        set { qtTicket = value; }
    }
}