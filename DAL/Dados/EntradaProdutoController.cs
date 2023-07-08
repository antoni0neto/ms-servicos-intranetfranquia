using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class EntradaProdutoController
    {
        BaseController baseController = new BaseController();

        DCDataContext db;

        public List<SP_Entradas_ProdutoResult> BuscaEntradasProduto(string dataInicio, string dataFim, string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from ep in db.SP_Entradas_Produto(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), produto) select ep).ToList();
        }

        public List<SP_Entradas_Produto_GraficoResult> BuscaEntradasProdutoGrafico(string dataInicio, string dataFim, string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from ep in db.SP_Entradas_Produto_Grafico(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), produto) select ep).ToList();
        }

        public List<PRODUTO> BuscaProdutos(string grupo, string griffe, string categoria)
        {
            if (grupo.Equals("Selecione"))
                grupo = null;
            if (griffe.Equals("Selecione"))
                griffe = null;
            if (categoria.Equals("Selecione"))
                categoria = null;

            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs
                    from pc in db.PRODUTOS_CATEGORIAs
                    where (p.COD_CATEGORIA == pc.COD_CATEGORIA) &&
                          (p.GRUPO_PRODUTO == grupo || grupo == null) &&
                          (p.COLECAO == "13") &&
                          (p.GRIFFE == griffe || griffe == null) &&
                          (p.COD_CATEGORIA == categoria || categoria == null)
                    orderby p.DESC_PRODUTO
                    select p).ToList();
        }

        public List<PRODUTO> BuscaProdutos(string grupo)
        {
            if (grupo.Equals("Selecione"))
                grupo = null;

            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs
                    where (p.GRUPO_PRODUTO == grupo || grupo == null)
                    orderby p.SUBGRUPO_PRODUTO, p.GRIFFE
                    select p).ToList();
        }

        public List<PRODUTO> BuscaProdutos(string grupo, string subgrupo, string linha, string griffe, string tipo)
        {
            if (grupo.Equals("Selecione"))
                grupo = null;
            if (griffe.Equals("Selecione"))
                griffe = null;
            if (subgrupo.Equals("Selecione"))
                subgrupo = null;
            if (linha.Equals("Selecione"))
                linha = null;
            if (tipo.Equals("Selecione"))
                tipo = null;

            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs
                    where (p.GRUPO_PRODUTO == grupo || grupo == null) &&
                          (p.COLECAO == "13") &&
                          (p.GRIFFE == griffe || griffe == null) &&
                          (p.SUBGRUPO_PRODUTO == subgrupo || subgrupo == null) &&
                          (p.LINHA == linha || linha == null) &&
                          (p.TIPO_PRODUTO == tipo || tipo == null)
                    orderby p.DESC_PRODUTO
                    select p).ToList();
        }

        public List<PRODUTOS_SUBGRUPO> BuscaSubGrupoProdutos(string grupo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from ps in db.PRODUTOS_SUBGRUPOs
                    where (ps.GRUPO_PRODUTO == grupo || grupo == null) 
                    orderby ps.SUBGRUPO_PRODUTO
                    select ps).ToList();
        }

        public List<PRODUTOS_SUBGRUPO> BuscaSubGrupoProdutos(string categoria, string grupo, string griffe)
        {
            if (griffe.Equals("Selecione"))
                griffe = null;

            db = new DCDataContext(Constante.ConnectionString);

            return (from ps in db.PRODUTOS_SUBGRUPOs
                    from p in db.PRODUTOs
                    where (ps.GRUPO_PRODUTO == grupo) &&
                          (ps.GRUPO_PRODUTO == p.GRUPO_PRODUTO) &&
                          (ps.SUBGRUPO_PRODUTO == p.SUBGRUPO_PRODUTO) &&
                          (p.COD_CATEGORIA == categoria) &&
                          (p.COLECAO == "13") &&
                          (p.GRIFFE == griffe || griffe == null)
                    select ps).Distinct().ToList();
        }
/*
        public List<GrupoProduto> BuscaGrupoProdutos(string colecao)
        {
            SEMANACOLECAO colecao = baseController.BuscaColecao(semanaAno); // Buscar coleção correta na tabela Semana Coleção

            if (colecao == null)
                return null;

            List<GrupoProduto> grupoProduto = new List<GrupoProduto>();

            db = new DCDataContext(Constante.ConnectionString);

            List<Sp_Busca_Grupos_ProdutoResult> listGroup = (from bgp in db.Sp_Busca_Grupos_Produto(categoria, griffe, colecao.CODIGO_COLECAO.ToString()) select bgp).ToList();

            foreach (Sp_Busca_Grupos_ProdutoResult item in listGroup)
            {
                GrupoProduto grupoProdutoItem = new GrupoProduto();

                if (item != null)
                {
                    grupoProdutoItem.Grupo = item.grupo;
                    grupoProdutoItem.Qt_produto_grupo = item.produtos_grupo;
                    grupoProdutoItem.Griffe = griffe;
                    grupoProdutoItem.Colecao = colecao.CODIGO_COLECAO.ToString();

                    grupoProduto.Add(grupoProdutoItem);
                }
            }

            return grupoProduto;
        }*/
/*
        public List<GrupoProduto> BuscaGrupoColecaoProdutos(string categoria, string griffe, string semanaAno)
        {
            SEMANACOLECAO colecao = baseController.BuscaColecao(semanaAno); // Buscar coleção correta na tabela Semana Coleção

            if (colecao == null)
                return null;

            List<GrupoProduto> grupoProduto = new List<GrupoProduto>();

            db = new DCDataContext(Constante.ConnectionString);

            List<Sp_Busca_Grupos_Colecao_ProdutoResult> listGroup = (from bgcp in db.Sp_Busca_Grupos_Colecao_Produto(categoria, griffe, colecao.CODIGO_COLECAO.ToString()) select bgcp).ToList();

            foreach (Sp_Busca_Grupos_Colecao_ProdutoResult item in listGroup)
            {
                GrupoProduto grupoProdutoItem = new GrupoProduto();

                if (item != null)
                {
                    grupoProdutoItem.Grupo = item.grupo;
                    grupoProdutoItem.Descricao_colecao = item.descricao_colecao;
                    grupoProdutoItem.Griffe = griffe;
                    grupoProdutoItem.Colecao = colecao.CODIGO_COLECAO.ToString();

                    grupoProduto.Add(grupoProdutoItem);
                }
            }

            return grupoProduto;
        }
        */
        public List<Sp_Line_Report_ProductResult> BuscaMovimentoProdutos(string dataInicio, string dataFim, List<PRODUTO> produto)
        {
            //List<Sp_Line_ReportResult> lineReport = null;
            List<Sp_Line_Report_ProductResult> lineReport = new List<Sp_Line_Report_ProductResult>();

            db = new DCDataContext(Constante.ConnectionString);

            foreach (PRODUTO item in produto)
            {
                Sp_Line_Report_ProductResult sp_Line_ReportResult = new Sp_Line_Report_ProductResult();
                sp_Line_ReportResult = db.Sp_Line_Report_Product(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), item.PRODUTO1).SingleOrDefault();
                if (sp_Line_ReportResult != null & sp_Line_ReportResult.QT_VENDA_SEMANA > 0)
                    lineReport.Add(sp_Line_ReportResult);
            }

            return lineReport;
        }

        public List<Sp_Line_Report_VendaResult> BuscaMovimentoProdutos(string dataInicio, string dataFim, List<PRODUTOS_SUBGRUPO> subGrupo, string grupo)
        {
            //List<Sp_Line_ReportResult> lineReport = null;
            List<Sp_Line_Report_VendaResult> lineReport = new List<Sp_Line_Report_VendaResult>();

            db = new DCDataContext(Constante.ConnectionString);

            foreach (PRODUTOS_SUBGRUPO item in subGrupo)
            {
                Sp_Line_Report_VendaResult sp_Line_Report_VendaResult = new Sp_Line_Report_VendaResult();
                sp_Line_Report_VendaResult = db.Sp_Line_Report_Venda(dataInicio, dataFim, item.SUBGRUPO_PRODUTO, grupo).SingleOrDefault();
                if (sp_Line_Report_VendaResult != null)
                    lineReport.Add(sp_Line_Report_VendaResult);
            }

            return lineReport;
        }
    }
}
