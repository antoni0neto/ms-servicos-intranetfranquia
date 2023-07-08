using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class Importacao
{
    private int codigoProduto;

    public int CodigoProduto
    {
        get { return codigoProduto; }
        set { codigoProduto = value; }
    }

    private string descricaoProduto;

    public string DescricaoProduto
    {
        get { return descricaoProduto; }
        set { descricaoProduto = value; }
    }

    private string descricaoProdutoCor;

    public string DescricaoProdutoCor
    {
        get { return descricaoProdutoCor; }
        set { descricaoProdutoCor = value; }
    }

    private string fobLinx;

    public string FobLinx
    {
        get { return fobLinx; }
        set { fobLinx = value; }
    }

    private string fobIntranet;

    public string FobIntranet
    {
        get { return fobIntranet; }
        set { fobIntranet = value; }
    }
}