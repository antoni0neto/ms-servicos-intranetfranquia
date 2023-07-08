using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class ResumoNotaTransfer
{
    private string barra;

    public string Barra
    {
        get { return barra; }
        set { barra = value; }
    }

    private string produto;

    public string Produto
    {
        get { return produto; }
        set { produto = value; }
    }

    private string cor;

    public string Cor
    {
        get { return cor; }
        set { cor = value; }
    }

    private string grade;

    public string Grade
    {
        get { return grade; }
        set { grade = value; }
    }

    private int qtEscaneada;

    public int QtEscaneada
    {
        get { return qtEscaneada; }
        set { qtEscaneada = value; }
    }

    private int qtEstoque;

    public int QtEstoque
    {
        get { return qtEstoque; }
        set { qtEstoque = value; }
    }
}