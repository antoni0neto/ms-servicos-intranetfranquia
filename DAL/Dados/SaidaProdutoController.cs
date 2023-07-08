using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class SaidaProdutoController
    {
        DCDataContext db;

        public List<SP_Vendas_VendedorResult> BuscaVendasVendedor(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from vv in db.SP_Vendas_Vendedor(AjustaData(dataInicio), AjustaData(dataFim), codigoFilial) select vv).ToList();
        }

        public List<Sp_Vendas_ConsumidorResult> BuscaVendasConsumidor(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from vc in db.Sp_Vendas_Consumidor(AjustaData(dataInicio), AjustaData(dataFim), codigoFilial) select vc).ToList();
        }

        public List<SP_Vendas_Vendedor_BermudaResult> BuscaVendasVendedorBermuda(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from vv in db.SP_Vendas_Vendedor_Bermuda(dataInicio, dataFim, codigoFilial) select vv).ToList();
        }

        public List<Sp_Vendas_Loja_BermudaResult> BuscaVendasLojaBermuda(string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from vl in db.Sp_Vendas_Loja_Bermuda(dataInicio, dataFim) select vl).ToList();
        }

        public List<SP_Vendas_Vendedor_GraficoResult> BuscaVendasVendedorGrafico(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from vvg in db.SP_Vendas_Vendedor_Grafico(AjustaData(dataInicio), AjustaData(dataFim), codigoFilial) select vvg).ToList();
        }

        public List<SP_Atendimento_LojaResult> BuscaAtendimentosLoja(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from al in db.SP_Atendimento_Loja(AjustaData(dataInicio), AjustaData(dataFim), codigoFilial) select al).ToList();
        }

        public List<SP_Atendimento_Loja_DiaResult> BuscaAtendimentosLojaDia(string data, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from ald in db.SP_Atendimento_Loja_Dia(data, codigoFilial) select ald).ToList();
        }

        public List<SP_Vendas_LojaResult> BuscaVendasLoja(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from vl in db.SP_Vendas_Loja(AjustaData(dataInicio), AjustaData(dataFim), codigoFilial) select vl).ToList();
        }

        public List<SP_Vendas_Loja_GraficoResult> BuscaVendasLojaGrafico(string dataInicio, string dataFim, string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from vlg in db.SP_Vendas_Loja_Grafico(AjustaData(dataInicio), AjustaData(dataFim), codigoFilial) select vlg).ToList();
        }

        public List<SP_Saidas_ProdutoResult> BuscaSaidasProduto(string dataInicio, string dataFim, string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from sp in db.SP_Saidas_Produto(AjustaData(dataInicio), AjustaData(dataFim), produto, cor) select sp).ToList();
        }

        public List<SP_Saidas_Produto_GraficoResult> BuscaSaidasProdutoGrafico(string dataInicio, string dataFim, string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from spg in db.SP_Saidas_Produto_Grafico(AjustaData(dataInicio), AjustaData(dataFim), produto, cor) select spg).ToList();
        }

        public List<PRODUTO> BuscaProdutos(string grupo, string subgrupo, string colecao, string linha, string griffe, string tipo)
        {
            if (grupo.Equals("Selecione"))
                grupo = null;
            if (subgrupo.Equals("Selecione"))
                subgrupo = null;
            if (colecao.Equals("Selecione"))
                colecao = null;
            if (linha.Equals("Selecione"))
                linha = null;
            if (griffe.Equals("Selecione"))
                griffe = null;
            if (tipo.Equals("Selecione"))
                tipo = null;

            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs where (p.GRUPO_PRODUTO == grupo || grupo == null) && 
                                                (p.SUBGRUPO_PRODUTO == subgrupo || subgrupo == null) && 
                                                (p.COLECAO == colecao || colecao == null) && 
                                                (p.LINHA == linha || linha == null) && 
                                                (p.GRIFFE == griffe || griffe == null) && 
                                                (p.TIPO_PRODUTO == tipo || tipo == null) orderby p.DESC_PRODUTO select p).ToList();
        }

        public List<PRODUTO> BuscaHistoricoProdutos(string grupo, string subgrupo, string colecao, string linha, string griffe, string tipo)
        {
            if (grupo.Equals("Selecione"))
                grupo = null;
            if (subgrupo.Equals("Selecione"))
                subgrupo = null;
            if (colecao.Equals("Selecione"))
                colecao = null;
            if (linha.Equals("Selecione"))
                linha = null;
            if (griffe.Equals("Selecione"))
                griffe = null;
            if (tipo.Equals("Selecione"))
                tipo = null;

            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs
                    where (p.GRUPO_PRODUTO == grupo || grupo == null) &&
                          (p.SUBGRUPO_PRODUTO == subgrupo || subgrupo == null) &&
                          (p.COLECAO == colecao || colecao == null) &&
                          (p.LINHA == linha || linha == null) &&
                          (p.GRIFFE == griffe || griffe == null) &&
                          (p.TIPO_PRODUTO == tipo || tipo == null)
                    orderby p.DESC_PRODUTO
                    select p).ToList();
        }

        public string AjustaData(string data)
        {
            string dataAjustada = "";

            dataAjustada = data.Substring(6, 4)+data.Substring(3, 2)+data.Substring(0, 2);

            return dataAjustada;
        }
    }
}
