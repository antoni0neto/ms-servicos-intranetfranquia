using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class ReportCriticaLoja
{
    private string numero_nf;

    public string Numero_nf
    {
        get { return numero_nf; }
        set { numero_nf = value; }
    }

    private string data_nf;

    public string Data_nf
    {
        get { return data_nf; }
        set { data_nf = value; }
    }

    private string valor_nf;

    public string Valor_nf
    {
        get { return valor_nf; }
        set { valor_nf = value; }
    }

    private string nome_cliente;

    public string Nome_cliente
    {
        get { return nome_cliente; }
        set { nome_cliente = value; }
    }

    private string cpf_cliente;

    public string Cpf_cliente
    {
        get { return cpf_cliente; }
        set { cpf_cliente = value; }
    }

    private string codigo_loja;

    public string Codigo_loja
    {
        get { return codigo_loja; }
        set { codigo_loja = value; }
    }

    private string descricao_loja;

    public string Descricao_loja
    {
        get { return descricao_loja; }
        set { descricao_loja = value; }
    }

    private string cor;

    public string Cor
    {
        get { return cor; }
        set { cor = value; }
    }
}