using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class Grupo
{
    private string loja;

    public string Loja
    {
        get { return loja; }
        set { loja = value; }
    }
    
    private string cota;

    public string Cota
    {
        get { return cota; }
        set { cota = value; }
    }

    private int qtde_prod_1_sem_1;

    public int Qtde_prod_1_sem_1
    {
        get { return qtde_prod_1_sem_1; }
        set { qtde_prod_1_sem_1 = value; }
    }

    private int qtde_prod_1_sem_2;

    public int Qtde_prod_1_sem_2
    {
        get { return qtde_prod_1_sem_2; }
        set { qtde_prod_1_sem_2 = value; }
    }

    private int qtde_prod_1_sem_3;

    public int Qtde_prod_1_sem_3
    {
        get { return qtde_prod_1_sem_3; }
        set { qtde_prod_1_sem_3 = value; }
    }

    private int qtde_prod_1_sem_4;

    public int Qtde_prod_1_sem_4
    {
        get { return qtde_prod_1_sem_4; }
        set { qtde_prod_1_sem_4 = value; }
    }

    private int qtde_prod_1_total;

    public int Qtde_prod_1_total
    {
        get { return qtde_prod_1_total; }
        set { qtde_prod_1_total = value; }
    }

    private int qtde_prod_2_sem_1;

    public int Qtde_prod_2_sem_1
    {
        get { return qtde_prod_2_sem_1; }
        set { qtde_prod_2_sem_1 = value; }
    }

    private int qtde_prod_2_sem_2;

    public int Qtde_prod_2_sem_2
    {
        get { return qtde_prod_2_sem_2; }
        set { qtde_prod_2_sem_2 = value; }
    }

    private int qtde_prod_2_sem_3;

    public int Qtde_prod_2_sem_3
    {
        get { return qtde_prod_2_sem_3; }
        set { qtde_prod_2_sem_3 = value; }
    }

    private int qtde_prod_2_sem_4;

    public int Qtde_prod_2_sem_4
    {
        get { return qtde_prod_2_sem_4; }
        set { qtde_prod_2_sem_4 = value; }
    }

    private int qtde_prod_2_total;

    public int Qtde_prod_2_total
    {
        get { return qtde_prod_2_total; }
        set { qtde_prod_2_total = value; }
    }

    private int qtde_prod_3_sem_1;

    public int Qtde_prod_3_sem_1
    {
        get { return qtde_prod_3_sem_1; }
        set { qtde_prod_3_sem_1 = value; }
    }

    private int qtde_prod_3_sem_2;

    public int Qtde_prod_3_sem_2
    {
        get { return qtde_prod_3_sem_2; }
        set { qtde_prod_3_sem_2 = value; }
    }

    private int qtde_prod_3_sem_3;

    public int Qtde_prod_3_sem_3
    {
        get { return qtde_prod_3_sem_3; }
        set { qtde_prod_3_sem_3 = value; }
    }

    private int qtde_prod_3_sem_4;

    public int Qtde_prod_3_sem_4
    {
        get { return qtde_prod_3_sem_4; }
        set { qtde_prod_3_sem_4 = value; }
    }

    private int qtde_prod_3_total;

    public int Qtde_prod_3_total
    {
        get { return qtde_prod_3_total; }
        set { qtde_prod_3_total = value; }
    }

    private int qtde_prod_4_sem_1;

    public int Qtde_prod_4_sem_1
    {
        get { return qtde_prod_4_sem_1; }
        set { qtde_prod_4_sem_1 = value; }
    }

    private int qtde_prod_4_sem_2;

    public int Qtde_prod_4_sem_2
    {
        get { return qtde_prod_4_sem_2; }
        set { qtde_prod_4_sem_2 = value; }
    }

    private int qtde_prod_4_sem_3;

    public int Qtde_prod_4_sem_3
    {
        get { return qtde_prod_4_sem_3; }
        set { qtde_prod_4_sem_3 = value; }
    }

    private int qtde_prod_4_sem_4;

    public int Qtde_prod_4_sem_4
    {
        get { return qtde_prod_4_sem_4; }
        set { qtde_prod_4_sem_4 = value; }
    }

    private int qtde_prod_4_total;

    public int Qtde_prod_4_total
    {
        get { return qtde_prod_4_total; }
        set { qtde_prod_4_total = value; }
    }

    private int qtde_prod_5_sem_1;

    public int Qtde_prod_5_sem_1
    {
        get { return qtde_prod_5_sem_1; }
        set { qtde_prod_5_sem_1 = value; }
    }

    private int qtde_prod_5_sem_2;

    public int Qtde_prod_5_sem_2
    {
        get { return qtde_prod_5_sem_2; }
        set { qtde_prod_5_sem_2 = value; }
    }

    private int qtde_prod_5_sem_3;

    public int Qtde_prod_5_sem_3
    {
        get { return qtde_prod_5_sem_3; }
        set { qtde_prod_5_sem_3 = value; }
    }

    private int qtde_prod_5_sem_4;

    public int Qtde_prod_5_sem_4
    {
        get { return qtde_prod_5_sem_4; }
        set { qtde_prod_5_sem_4 = value; }
    }

    private int qtde_prod_5_total;

    public int Qtde_prod_5_total
    {
        get { return qtde_prod_5_total; }
        set { qtde_prod_5_total = value; }
    }

    private int qtde_prod_6_sem_1;

    public int Qtde_prod_6_sem_1
    {
        get { return qtde_prod_6_sem_1; }
        set { qtde_prod_6_sem_1 = value; }
    }

    private int qtde_prod_6_sem_2;

    public int Qtde_prod_6_sem_2
    {
        get { return qtde_prod_6_sem_2; }
        set { qtde_prod_6_sem_2 = value; }
    }

    private int qtde_prod_6_sem_3;

    public int Qtde_prod_6_sem_3
    {
        get { return qtde_prod_6_sem_3; }
        set { qtde_prod_6_sem_3 = value; }
    }

    private int qtde_prod_6_sem_4;

    public int Qtde_prod_6_sem_4
    {
        get { return qtde_prod_6_sem_4; }
        set { qtde_prod_6_sem_4 = value; }
    }

    private int qtde_prod_6_total;

    public int Qtde_prod_6_total
    {
        get { return qtde_prod_6_total; }
        set { qtde_prod_6_total = value; }
    }
}