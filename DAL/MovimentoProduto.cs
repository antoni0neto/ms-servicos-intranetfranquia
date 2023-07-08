using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class MovimentoProduto
{
    private int produto;

    public int Produto
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

    private string grupo;

    public string Grupo
    {
        get { return grupo; }
        set { grupo = value; }
    }

    private string modelo;

    public string Modelo
    {
        get { return modelo; }
        set { modelo = value; }
    }

    private int venda_semana;

    public int Venda_semana
    {
        get { return venda_semana; }
        set { venda_semana = value; }
    }

    private int venda_geral;

    public int Venda_geral
    {
        get { return venda_geral; }
        set { venda_geral = value; }
    }

    private int estoque;

    public int Estoque
    {
        get { return estoque; }
        set { estoque = value; }
    }

    private int recebido_semana;

    public int Recebido_semana
    {
        get { return recebido_semana; }
        set { recebido_semana = value; }
    }

    private int recebido_geral;

    public int Recebido_geral
    {
        get { return recebido_geral; }
        set { recebido_geral = value; }
    }

    private string filial;

    public string Filial
    {
        get { return filial; }
        set { filial = value; }
    }

    private string data;

    public string Data
    {
        get { return data; }
        set { data = value; }
    }

    private int colecao;

    public int Colecao
    {
        get { return colecao; }
        set { colecao = value; }
    }

    private string preco;

    public string Preco
    {
        get { return preco; }
        set { preco = value; }
    }
}