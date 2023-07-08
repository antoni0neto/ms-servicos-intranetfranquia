using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class FuncionarioLojaController
    {
        DCDataContext db;

        public List<LOJA_VENDEDORE> BuscaFuncionariosLoja(List<USUARIOLOJA> usuarioLoja)
        {
            List<string> idLojas = new List<string>();

            foreach (USUARIOLOJA i in usuarioLoja)
                idLojas.Add(i.CODIGO_LOJA);

            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where idLojas.Contains(lv.VENDEDOR) orderby lv.NOME_VENDEDOR select lv).ToList();
        }

        public List<LOJA_VENDEDORE> BuscaFuncionariosLoja()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from lv in db.LOJA_VENDEDOREs orderby lv.NOME_VENDEDOR select lv).ToList();
        }

        public List<LOJA_VENDEDORE> BuscaFuncionariosLoja(int CODIGO_LOJA)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from lv in db.LOJA_VENDEDOREs where Convert.ToInt32(lv.CODIGO_FILIAL) == CODIGO_LOJA select lv).ToList();
        }

        public LOJA_VENDEDORE BuscaPorCodigoFuncionario(string CODIGO_FUNCIONARIO_LOJA)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from lv in db.LOJA_VENDEDOREs where lv.VENDEDOR.Equals(CODIGO_FUNCIONARIO_LOJA) select lv).SingleOrDefault();
        }

        public LOJA_VENDEDORE BuscaGerenteLoja(int CODIGO_LOJA)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where Convert.ToInt32(lv.CODIGO_FILIAL) == CODIGO_LOJA && Convert.ToInt32(lv.GERENTE) == 1 select lv).Take(1).SingleOrDefault();
        }

        public List<SP_Funcionarios_LojaResult> BuscaVendedores(string codigo_filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from fl in db.SP_Funcionarios_Loja(codigo_filial) select fl).ToList();
        }

        public List<SP_Funcionarios_Loja_GraficoResult> BuscaVendedoresCincoMais(string codigo_filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from flg in db.SP_Funcionarios_Loja_Grafico(codigo_filial) select flg).ToList();
        }
    }
}
