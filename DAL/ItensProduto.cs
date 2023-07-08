using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class ItensProduto
{
    private string qtdeItensProduto;

    public string QtdeItensProduto
    {
        get { return qtdeItensProduto; }
        set { qtdeItensProduto = value; }
    }

    private int qtdeTicket;

    public int QtdeTicket
    {
        get { return qtdeTicket; }
        set { qtdeTicket = value; }
    }

    private decimal valorTicket;

    public decimal ValorTicket
    {
        get { return valorTicket; }
        set { valorTicket = value; }
    }

    private decimal descontoTicket;

    public decimal DescontoTicket
    {
        get { return descontoTicket; }
        set { descontoTicket = value; }
    }
}