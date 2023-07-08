using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class ResumoSupervisor
{
    private string supervisor;

    public string Supervisor
    {
        get { return supervisor; }
        set { supervisor = value; }
    }

    private int qtde_verde;

    public int Qtde_verde
    {
        get { return qtde_verde; }
        set { qtde_verde = value; }
    }

    private int qtde_amarelo;

    public int Qtde_amarelo
    {
        get { return qtde_amarelo; }
        set { qtde_amarelo = value; }
    }

    private int qtde_vermelho;

    public int Qtde_vermelho
    {
        get { return qtde_vermelho; }
        set { qtde_vermelho = value; }
    }

    private int qtde_roxo;

    public int Qtde_roxo
    {
        get { return qtde_roxo; }
        set { qtde_roxo = value; }
    }

    private decimal atingido;

    public decimal Atingido
    {
        get { return atingido; }
        set { atingido = value; }
    }
}