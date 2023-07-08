using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class ControleProdutoController
    {
        DCDataContext db;

        //SP_OBTER_PRODUTO_SEM_CADASTROResult
        public List<SP_OBTER_PRODUTO_SEM_CADASTROResult> ObterProdutoSemCadastro(string colecao, string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_PRODUTO_SEM_CADASTRO(colecao, produto) select m).ToList();
        }

        //SP_OBTER_PRODUTO_CORTADOResult
        public List<SP_OBTER_PRODUTO_CORTADOResult> ObterProdutoCortado(int? codigoHB, int? hb, string pedido, string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_PRODUTO_CORTADO(codigoHB, hb, pedido, produto, cor) select m).ToList().Where(p => p.CORTADO == true).ToList();
        }

        public List<PRODUTO_NCM> ObterProdutoNCM()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_NCMs orderby m.CLASSIF_FISCAL select m).ToList();
        }
        public List<PRODUTO_NCM> ObterProdutoNCM(string subGrupoProduto, string griffe, string composicao, string grupoProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_NCMs
                    where
                        m.SUBGRUPO_PRODUTO == subGrupoProduto &&
                        m.GRIFFE.Contains(griffe.Trim()) &&
                        m.COMPOSICAO == composicao &&
                        m.GRUPO_PRODUTO == grupoProduto
                    orderby m.CLASSIF_FISCAL
                    select m).ToList();
        }
        public PRODUTO_NCM ObterProdutoNCM(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_NCMs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public void AtualizarProdutoNCM(PRODUTO_NCM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PRODUTO_NCM _produtoNCM = ObterProdutoNCM(_novo.CODIGO);

            if (_novo != null)
            {
                _produtoNCM.GRUPO_PRODUTO = _novo.GRUPO_PRODUTO;
                _produtoNCM.SUBGRUPO_PRODUTO = _novo.SUBGRUPO_PRODUTO;
                _produtoNCM.GRIFFE = _novo.GRIFFE;
                _produtoNCM.COMPOSICAO = _novo.COMPOSICAO;
                _produtoNCM.CLASSIF_FISCAL = _novo.CLASSIF_FISCAL;
                _produtoNCM.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                _produtoNCM.DATA_ALTERACAO = _novo.DATA_ALTERACAO;

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //SP_OBTER_COMPOSICAO_NCMResult
        public SP_OBTER_COMPOSICAO_NCMResult ObterComposicaoNCM(string material)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_COMPOSICAO_NCM(material) select m).FirstOrDefault();
        }

    }
}
