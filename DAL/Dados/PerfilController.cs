using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class PerfilController
    {
        DCDataContext db;

        public List<USUARIO_PERFIL> BuscaPerfil()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.USUARIO_PERFILs orderby p.DESCRICAO_PERFIL select p).ToList();
        }

        public List<FECHAMENTO_CONTA> BuscaContas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.FECHAMENTO_CONTAs select c).ToList();
        }

        public List<USUARIO_PERFIL> BuscaPerfilSupervisor()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.USUARIO_PERFILs where p.CODIGO_PERFIL == 3 || p.CODIGO_PERFIL == 4 select p).ToList();
        }

        public List<USUARIO_PERFIL> BuscaPerfilLoja()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.USUARIO_PERFILs where p.CODIGO_PERFIL == 2 select p).ToList();
        }

        public USUARIO_PERFIL BuscaPorCodigoPerfil(int CODIGO_PERFIL)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.USUARIO_PERFILs where p.CODIGO_PERFIL == CODIGO_PERFIL select p).SingleOrDefault();
        }
    }
}
