using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

public class Constante
{
    public Constante()
    {
    }

    public static string ConnectionString
    {
        get
        {
            if (ConfigurationManager.AppSettings["appAmbiente"].ToString() == "T")
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
            else if (ConfigurationManager.AppSettings["appAmbiente"].ToString() == "H")
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringHomologacao"].ToString();
            }
            else
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            }
        }
    }

    public static string ConnectionStringJonas
    {
        get { return ConfigurationManager.ConnectionStrings["ConnectionStringJonas"].ToString(); }
    }

    public static string ConnectionStringIntranet
    {
        get
        {
            if (ConfigurationManager.AppSettings["appAmbiente"].ToString() == "T")
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIntranetTeste"].ToString();
            }
            else if (ConfigurationManager.AppSettings["appAmbiente"].ToString() == "H")
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIntranetTeste"].ToString();
            }
            else
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIntranet"].ToString();
            }
        }
    }

    public static string ConnectionStringIntranetPOPUP
    {
        get
        {
            if (ConfigurationManager.AppSettings["appAmbiente"].ToString() == "T")
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIntranetTeste"].ToString();
            }
            else if (ConfigurationManager.AppSettings["appAmbiente"].ToString() == "H")
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIntranetTeste"].ToString();
            }
            else
            {
                return ConfigurationManager.ConnectionStrings["ConnectionStringIntranetPOPUP"].ToString();
            }
        }
    }

    public static string ConnectionStringJGM
    {
        get { return ConfigurationManager.ConnectionStrings["ConnectionStringJGM"].ToString(); }
    }

    public static string TesteConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["TesteConnectionString"].ToString(); }
    }

    public static string ConnectionStringFranquia
    {
        get { return ConfigurationManager.ConnectionStrings["ConnectionStringFranquia"].ToString(); }
    }

    public static string ConnectionStringAD
    {
        get { return ConfigurationManager.ConnectionStrings["ConnectionStringAD"].ToString(); }
    }

    public static string Ambiente
    {
        get { return ConfigurationManager.AppSettings["appAmbiente"].ToString(); }
    }

    public static string emailUser
    {
        get { return ConfigurationManager.AppSettings["emailUser"].ToString(); }
    }

    public static string emailPass
    {
        get { return ConfigurationManager.AppSettings["emailPass"].ToString(); }
    }

    public static bool gerarPedidoCompra
    {
        get { return ((ConfigurationManager.AppSettings["GerarPedidoCompra"].ToString() == "S") ? true : false); }
    }

    public static bool enviarEmail
    {
        get { return ((ConfigurationManager.AppSettings["EnviarEmail"].ToString() == "S") ? true : false); }
    }

    public static string userMagento
    {
        get { return ConfigurationManager.AppSettings["userMagento"].ToString(); }
    }

    public static string apiKeyMagento
    {
        get { return ConfigurationManager.AppSettings["apiKeyMagento"].ToString(); }
    }

    public static string urlBaseMagentoV2
    {
        get { return ConfigurationManager.AppSettings["urlBaseMagentoV2"].ToString(); }
    }

    public static string tokenMagentoV2
    {
        get { return ConfigurationManager.AppSettings["tokenMagentoV2"].ToString(); }
    }

    public static string pathFromXML
    {
        get { return ConfigurationManager.AppSettings["pathFromXML"].ToString(); }
    }

    public static string pathToXML
    {
        get { return ConfigurationManager.AppSettings["pathToXML"].ToString(); }
    }


}