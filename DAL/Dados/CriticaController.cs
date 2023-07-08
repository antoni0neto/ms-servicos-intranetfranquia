using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class CriticaController
    {
        BaseController baseController = new BaseController();

        DCDataContext db;

        public List<Sp_Critica_Loja_DiaResult> BuscaCriticaLojaDia(string data)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from cld in db.Sp_Critica_Loja_Dia(data) select cld).ToList();
        }

        public List<Sp_Busca_Vendas_DiaResult> BuscaVendasDia(string cliente, string data)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bvd in db.Sp_Busca_Vendas_Dia(cliente, data) select bvd).ToList();
        }
    }
}
