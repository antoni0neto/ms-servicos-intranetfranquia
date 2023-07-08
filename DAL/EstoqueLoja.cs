using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class EstoqueLoja
{
    private string grupo;

    public string Grupo
    {
        get { return grupo; }
        set { grupo = value; }
    }

    private string subGrupo;

    public string SubGrupo
    {
        get { return subGrupo; }
        set { subGrupo = value; }
    }

    private string tipoProduto;

    public string TipoProduto
    {
        get { return tipoProduto; }
        set { tipoProduto = value; }
    }

    private string linhaProduto;

    public string LinhaProduto
    {
        get { return linhaProduto; }
        set { linhaProduto = value; }
    }

    private string venda;

    public string Venda
    {
        get { return venda; }
        set { venda = value; }
    }

    private string estoque;

    public string Estoque
    {
        get { return estoque; }
        set { estoque = value; }
    }

    private string reposicao;

    public string Reposicao
    {
        get { return reposicao; }
        set { reposicao = value; }
    }

    private string novo;

    public string Novo
    {
        get { return novo; }
        set { novo = value; }
    }

    private string mar;

    public string Mar
    {
        get { return mar; }
        set { mar = value; }
    }

    private string ern;

    public string Ern
    {
        get { return ern; }
        set { ern = value; }
    }
}