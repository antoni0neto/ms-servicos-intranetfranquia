using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class ContaCorrente
{
    private DateTime data;

    public DateTime Data
    {
        get { return data; }
        set { data = value; }
    }

    private string descricao;

    public string Descricao
    {
        get { return descricao; }
        set { descricao = value; }
    }

    private decimal valor;

    public decimal Valor
    {
        get { return valor; }
        set { valor = value; }
    }
}