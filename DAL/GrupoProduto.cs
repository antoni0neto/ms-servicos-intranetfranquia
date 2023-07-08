using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class GrupoProduto
{
    private string grupo;

    public string Grupo
    {
        get { return grupo; }
        set { grupo = value; }
    }

    private string griffe;

    public string Griffe
    {
        get { return griffe; }
        set { griffe = value; }
    }

    private int qt_produto_grupo;

    public int Qt_produto_grupo
    {
        get { return qt_produto_grupo; }
        set { qt_produto_grupo = value; }
    }

    private string colecao;

    public string Colecao
    {
        get { return colecao; }
        set { colecao = value; }
    }

    private string descricao_colecao;

    public string Descricao_colecao
    {
        get { return descricao_colecao; }
        set { descricao_colecao = value; }
    }
}