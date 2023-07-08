using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class Tamanho
{
    private int codigo;

    public int Codigo
    {
        get { return codigo; }
        set { codigo = value; }
    }

    private string descricao;

    public string Descricao
    {
        get { return descricao; }
        set { descricao = value; }
    }
}