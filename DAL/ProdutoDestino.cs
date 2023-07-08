using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class ProdutoDestino
{
    private string destino;

    public string Destino
    {
        get { return destino; }
        set { destino = value; }
    }

    private string defeito;

    public string Defeito
    {
        get { return defeito; }
        set { defeito = value; }
    }

    private string codigoProduto;

    public string CodigoProduto
    {
        get { return codigoProduto; }
        set { codigoProduto = value; }
    }

    private bool flagBaixado;

    public bool FlagBaixado
    {
        get { return flagBaixado; }
        set { flagBaixado = value; }
    }
}