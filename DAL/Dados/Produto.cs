using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class Produto
{
    private int indiceProduto;

    public int IndiceProduto
    {
        get { return indiceProduto; }
        set { indiceProduto = value; }
    }

    private string codigoBarra;

    public string CodigoBarra
    {
        get { return codigoBarra; }
        set { codigoBarra = value; }
    }

    private string codigoCorProduto;

    public string CodigoCorProduto
    {
        get { return codigoCorProduto; }
        set { codigoCorProduto = value; }
    }

    private string corProduto;

    public string CorProduto
    {
        get { return corProduto; }
        set { corProduto = value; }
    }

    private string descricaoProduto;

    public string DescricaoProduto
    {
        get { return descricaoProduto; }
        set { descricaoProduto = value; }
    }

    private string gradeProduto;

    public string GradeProduto
    {
        get { return gradeProduto; }
        set { gradeProduto = value; }
    }

    private decimal precoOriginal;

    public decimal PrecoOriginal
    {
        get { return precoOriginal; }
        set { precoOriginal = value; }
    }

    private decimal desconto;

    public decimal Desconto
    {
        get { return desconto; }
        set { desconto = value; }
    }
    
    private decimal precoLiquido;

    public decimal PrecoLiquido
    {
        get { return precoLiquido; }
        set { precoLiquido = value; }
    }

    private decimal precoLoja;

    public decimal PrecoLoja
    {
        get { return precoLoja; }
        set { precoLoja = value; }
    }

    private string colecao;

    public string Colecao
    {
        get { return colecao; }
        set { colecao = value; }
    }
}