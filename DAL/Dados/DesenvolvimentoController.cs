using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;
using System.Data.Common;
using System.Transactions;
using Dapper;
using System.Data;


namespace DAL
{
    public class DesenvolvimentoController
    {
        DCDataContext db;
        INTRADataContext dbINTRA;
        LINXDataContext dbLINX;

        //SEQUENCE PRODUTO
        public DESENV_PRODUTO_SEQUENCE ObterProdutoSequencia(bool cmax)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_SEQUENCEs
                    where m.CMAX == cmax && m.DATA_UTILIZACAO == null
                    orderby m.PRODUTO
                    select m).FirstOrDefault();
        }
        public DESENV_PRODUTO_SEQUENCE ObterProdutoSequencia(int produto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_SEQUENCEs
                    where m.PRODUTO == produto
                    orderby m.PRODUTO
                    select m).FirstOrDefault();
        }
        public void AtualizarProdutoSequencia(DESENV_PRODUTO_SEQUENCE _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_SEQUENCE s = ObterProdutoSequencia(_novo.PRODUTO);

            if (s != null)
            {
                s.PRODUTO = _novo.PRODUTO;
                s.DATA_UTILIZACAO = _novo.DATA_UTILIZACAO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //SEQUENCE ACESSORIO
        public DESENV_ACESSORIO_SEQUENCE ObterAcessorioSequencia()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_SEQUENCEs
                    where m.DATA_UTILIZACAO == null
                    orderby m.PRODUTO
                    select m).FirstOrDefault();
        }
        public DESENV_ACESSORIO_SEQUENCE ObterAcessorioSequencia(int produto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_SEQUENCEs
                    where m.PRODUTO == produto
                    orderby m.PRODUTO
                    select m).FirstOrDefault();
        }
        public void AtualizarAcessorioSequencia(DESENV_ACESSORIO_SEQUENCE _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_ACESSORIO_SEQUENCE s = ObterAcessorioSequencia(_novo.PRODUTO);

            if (s != null)
            {
                s.PRODUTO = _novo.PRODUTO;
                s.DATA_UTILIZACAO = _novo.DATA_UTILIZACAO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<SP_OBTER_VIEWResult> ObterView(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_VIEW(colecao) select i).ToList();

        }

        //DESENV_PRODUTO
        public List<DESENV_PRODUTO> ObterProduto()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    orderby m.COLECAO, m.GRUPO, m.MODELO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProduto(string col)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.COLECAO == col.ToUpper().Trim()
                    //&& m.STATUS == 'A'
                    orderby m.COLECAO, m.GRUPO, m.MODELO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProdutoPorProduto(string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.MODELO == produto
                    && m.STATUS == 'A'
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProdutoPorTecidoECorFornecedor(string tecido, string corFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.TECIDO_POCKET == tecido
                    && m.FORNECEDOR_COR == corFornecedor
                    && m.STATUS == 'A'
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProdutoPorTecido(string tecido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.TECIDO_POCKET == tecido
                    && m.STATUS == 'A'
                    select m).ToList();
        }
        public DESENV_PRODUTO ObterProduto(string col, string modelo, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.COLECAO == col.ToUpper().Trim()
                    && m.MODELO == modelo.ToUpper().Trim()
                    && m.COR == cor.ToUpper().Trim()
                    && m.STATUS == 'A'
                    select m).FirstOrDefault();
        }
        public DESENV_PRODUTO ObterProduto(string col, string modelo, string cor, int desenvOrigem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.COLECAO == col.ToUpper().Trim()
                    && m.MODELO == modelo.ToUpper().Trim()
                    && m.COR == cor.ToUpper().Trim()
                    && m.DESENV_PRODUTO_ORIGEM == desenvOrigem
                    && m.STATUS == 'A'
                    select m).FirstOrDefault();
        }
        public DESENV_PRODUTO ObterProdutoFoto(string modelo, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.MODELO == modelo.ToUpper().Trim()
                    && m.COR == cor.ToUpper().Trim()
                    && m.STATUS == 'A'
                    && m.FOTO != null
                    select m).FirstOrDefault();
        }
        public List<DESENV_PRODUTO> ObterProdutoRel(string col, string modelo, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.COLECAO == col.ToUpper().Trim()
                    && m.MODELO == modelo.ToUpper().Trim()
                    && m.COR == cor.ToUpper().Trim()
                    orderby m.COLECAO, m.GRUPO, m.MODELO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProduto(string col, string modelo, char status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.COLECAO.Trim() == col.Trim()
                    //&& m.DESENV_PRODUTO_ORIGEM == desenvOrigem
                    && m.MODELO.Trim() == modelo.Trim()
                    && m.STATUS == status
                    select m).ToList();
        }

        public List<DESENV_PRODUTO> ObterProduto(string col, string modelo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.COLECAO == col.ToUpper().Trim()
                    && m.MODELO == modelo.ToUpper().Trim()
                    && m.STATUS == 'A'
                    orderby m.COLECAO, m.GRUPO, m.MODELO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProdutoCor(string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs
                    where m.MODELO == produto.Trim()
                    && m.COR == cor.Trim()
                    && m.STATUS == 'A'
                    orderby m.COLECAO, m.GRUPO, m.MODELO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO> ObterProdutoSemPedido()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.DESENV_PRODUTOs

                    join c in db.DESENV_PRODUTO_PEDIDOs
                    on g.CODIGO equals c.DESENV_PRODUTO into pedidoLeft
                    from c in pedidoLeft.DefaultIfEmpty()
                    where c.CODIGO == null && g.STATUS == 'A' //&& g.COLECAO.Trim().ToUpper() == col.Trim().ToUpper()

                    join k in db.DESENV_PRODUTO_CARRINHOs
                    on g.CODIGO equals k.DESENV_PRODUTO into carrinhoLeft
                    from k in carrinhoLeft.DefaultIfEmpty()
                    where k.CODIGO == null

                    orderby g.COLECAO, g.GRUPO, g.MODELO
                    select g).ToList();

        }
        public List<DESENV_PRODUTO> ObterProdutoComPedidoFechado(int codigoPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.DESENV_PRODUTOs
                    join c in db.DESENV_PRODUTO_PEDIDOs
                    on g.CODIGO equals c.DESENV_PRODUTO
                    join i in db.DESENV_PEDIDO_QTDEs
                    on c.CODIGO equals i.DESENV_PEDIDO
                    where i.QTDE > 0
                    orderby g.COLECAO, g.GRUPO, g.MODELO
                    select g).ToList();
        }
        public List<DESENV_PRODUTO> ObterProdutoComPedido(int codigoPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (codigoPedido > 0)
            {
                return (from g in db.DESENV_PRODUTOs
                        join c in db.DESENV_PRODUTO_PEDIDOs
                        on g.CODIGO equals c.DESENV_PRODUTO
                        join p in db.DESENV_PEDIDOs
                        on c.DESENV_PEDIDO equals p.CODIGO
                        where p.CODIGO == codigoPedido
                        orderby g.COLECAO, g.GRUPO, g.MODELO
                        select g).ToList();
            }
            else
            {
                return (from g in db.DESENV_PRODUTOs
                        join c in db.DESENV_PRODUTO_PEDIDOs
                        on g.CODIGO equals c.DESENV_PRODUTO
                        join p in db.DESENV_PEDIDOs
                        on c.DESENV_PEDIDO equals p.CODIGO
                        orderby g.COLECAO, g.GRUPO, g.MODELO
                        select g).ToList();
            }
        }
        public List<DESENV_PRODUTO> ObterProdutoNoCarrinho(int _usuario, int? codPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.DESENV_PRODUTOs
                    join c in db.DESENV_PRODUTO_CARRINHOs
                    on g.CODIGO equals c.DESENV_PRODUTO
                    where c.USUARIO == _usuario
                    && c.DESENV_PEDIDO == codPedido
                    orderby g.COLECAO, g.GRUPO, g.MODELO
                    select g).ToList();
        }
        public DESENV_PRODUTO ObterProduto(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int ObterProdutoCODRef(int codRef, string col)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO _produto = (from m in db.DESENV_PRODUTOs
                                       where m.CODIGO_REF == codRef && m.COLECAO == col
                                       select m).Take(1).SingleOrDefault();
            if (_produto == null)
                return 0;

            return _produto.CODIGO;
        }
        public int InserirProduto(DESENV_PRODUTO _produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.DESENV_PRODUTOs.InsertOnSubmit(_produto);
                db.SubmitChanges();

                return _produto.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void AtualizarProduto(DESENV_PRODUTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DESENV_PRODUTO _produto = ObterProduto(_novo.CODIGO);
            if (_produto != null)
            {
                //_produto.CODIGO = _novo.CODIGO;
                _produto.CODIGO_REF = _novo.CODIGO_REF;
                _produto.COLECAO = _novo.COLECAO;
                _produto.DESENV_PRODUTO_ORIGEM = _novo.DESENV_PRODUTO_ORIGEM;
                _produto.GRUPO = _novo.GRUPO;
                _produto.MODELO = _novo.MODELO;

                if (_novo.MARCA != null && _novo.MARCA.Trim() != "")
                    _produto.MARCA = _novo.MARCA;

                if (_novo.GRIFFE != null && _novo.GRIFFE.Trim() != "")
                    _produto.GRIFFE = _novo.GRIFFE;

                if (_novo.TECIDO_POCKET != null && _novo.TECIDO_POCKET.Trim() != "")
                    _produto.TECIDO_POCKET = _novo.TECIDO_POCKET;

                if (_novo.COR != null && _novo.COR.Trim() != "")
                    _produto.COR = _novo.COR;

                if (_novo.QTDE != null)
                    _produto.QTDE = _novo.QTDE;

                if (_novo.PRECO != null)
                    _produto.PRECO = _novo.PRECO;

                if (_novo.FOTO != null)
                    _produto.FOTO = _novo.FOTO;

                if (_novo.FOTO2 != null)
                    _produto.FOTO2 = _novo.FOTO2;

                if (_novo.OBSERVACAO != null)
                    _produto.OBSERVACAO = _novo.OBSERVACAO;

                if (_novo.OBSERVACAO != null)
                    _produto.OBS_IMPRESSAO = _novo.OBS_IMPRESSAO;

                if (_novo.QTDE_ATACADO != null)
                    _produto.QTDE_ATACADO = _novo.QTDE_ATACADO;

                if (_novo.PRECO_ATACADO != null)
                    _produto.PRECO_ATACADO = _novo.PRECO_ATACADO;

                if (_novo.SEGMENTO != null)
                    _produto.SEGMENTO = _novo.SEGMENTO;

                if (_novo.FORNECEDOR != null && _novo.FORNECEDOR.Trim() != "")
                    _produto.FORNECEDOR = _novo.FORNECEDOR;

                if (_novo.CORTE_PETIT != null)
                    _produto.CORTE_PETIT = _novo.CORTE_PETIT;

                if (_novo.CORTE_VAREJO != null)
                    _produto.CORTE_VAREJO = _novo.CORTE_VAREJO;

                if (_novo.CORTE_ATACADO != null)
                    _produto.CORTE_ATACADO = _novo.CORTE_ATACADO;

                if (_novo.FORNECEDOR_COR != null)
                    _produto.FORNECEDOR_COR = _novo.FORNECEDOR_COR;

                if (_novo.REF_MODELAGEM != null)
                    _produto.REF_MODELAGEM = _novo.REF_MODELAGEM;

                if (_novo.PRODUTO_ACABADO != null)
                    _produto.PRODUTO_ACABADO = _novo.PRODUTO_ACABADO;

                if (_novo.QTDE_MOSTRUARIO != null)
                    _produto.QTDE_MOSTRUARIO = _novo.QTDE_MOSTRUARIO;

                if (_novo.BRINDE != null)
                    _produto.BRINDE = _novo.BRINDE;

                _produto.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _produto.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _produto.STATUS = _novo.STATUS;
                _produto.DATA_APROVACAO = _novo.DATA_APROVACAO;

                _produto.DATA_ATUALIZACAO_PRECO = null;

                _produto.DESC_MODELO = _novo.DESC_MODELO;
                _produto.SUBGRUPO_PRODUTO = _novo.SUBGRUPO_PRODUTO;
                _produto.LINHA = _novo.LINHA;
                _produto.GRADE = _novo.GRADE;
                _produto.DATA_CADASTRO_LIB = _novo.DATA_CADASTRO_LIB;
                _produto.NCM = _novo.NCM;
                _produto.LAVAGEM_MATERIAL = _novo.LAVAGEM_MATERIAL;

                if (_novo.OBS_MOSTRUARIO != null)
                    _produto.OBS_MOSTRUARIO = _novo.OBS_MOSTRUARIO;

                if (_novo.USUARIO_MOSTRUARIO_OK != null)
                    _produto.USUARIO_MOSTRUARIO_OK = _novo.USUARIO_MOSTRUARIO_OK;

                if (_novo.SIGNED != null)
                    _produto.SIGNED = _novo.SIGNED;
                if (_novo.SIGNED_NOME != null)
                    _produto.SIGNED_NOME = _novo.SIGNED_NOME;
                if (_novo.DATA_LIB_GEMEA != null)
                    _produto.DATA_LIB_GEMEA = _novo.DATA_LIB_GEMEA;

                if (_novo.DATA_MODELAGEM != null)
                    _produto.DATA_MODELAGEM = _novo.DATA_MODELAGEM;

                if (_novo.CUSTO != null)
                    _produto.CUSTO = _novo.CUSTO;
                if (_novo.CUSTO_NOTA != null)
                    _produto.CUSTO_NOTA = _novo.CUSTO_NOTA;

                if (_novo.GRADEADO != null)
                    _produto.GRADEADO = _novo.GRADEADO;

                if (_novo.MODDESIGN != null)
                    _produto.MODDESIGN = _novo.MODDESIGN;

                if (_novo.DATA_LIB_1PECA != null)
                    _produto.DATA_LIB_1PECA = _novo.DATA_LIB_1PECA;

                //Controle para apenas inserir 1x
                if (_produto.DATA_MODDESIGN == null)
                    _produto.DATA_MODDESIGN = _novo.DATA_MODDESIGN;
                ///////////////////////////////////////////////////////////-----------------------

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
        public void AtualizarTripa(DESENV_PRODUTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DESENV_PRODUTO _produto = ObterProduto(_novo.CODIGO);
            if (_produto != null)
            {
                //_produto.CODIGO = _novo.CODIGO;
                _produto.CODIGO_REF = _novo.CODIGO_REF;
                _produto.COLECAO = _novo.COLECAO;
                _produto.DESENV_PRODUTO_ORIGEM = _novo.DESENV_PRODUTO_ORIGEM;
                _produto.GRUPO = _novo.GRUPO;
                _produto.MODELO = _novo.MODELO;

                if (_novo.MARCA != null && _novo.MARCA.Trim() != "")
                    _produto.MARCA = _novo.MARCA;

                if (_novo.GRIFFE != null)
                    _produto.GRIFFE = _novo.GRIFFE;

                if (_novo.TECIDO_POCKET != null)
                    _produto.TECIDO_POCKET = _novo.TECIDO_POCKET;

                if (_novo.COR != null)
                    _produto.COR = _novo.COR;

                if (_novo.QTDE != null)
                    _produto.QTDE = _novo.QTDE;

                if (_novo.PRECO != null)
                    _produto.PRECO = _novo.PRECO;

                if (_novo.FOTO != null)
                    _produto.FOTO = _novo.FOTO;

                if (_novo.FOTO2 != null)
                    _produto.FOTO2 = _novo.FOTO2;

                if (_novo.OBSERVACAO != null)
                    _produto.OBSERVACAO = _novo.OBSERVACAO;

                if (_novo.OBSERVACAO != null)
                    _produto.OBS_IMPRESSAO = _novo.OBS_IMPRESSAO;

                if (_novo.QTDE_ATACADO != null)
                    _produto.QTDE_ATACADO = _novo.QTDE_ATACADO;

                if (_novo.PRECO_ATACADO != null)
                    _produto.PRECO_ATACADO = _novo.PRECO_ATACADO;

                if (_novo.SEGMENTO != null)
                    _produto.SEGMENTO = _novo.SEGMENTO;

                if (_novo.FORNECEDOR != null)
                    _produto.FORNECEDOR = _novo.FORNECEDOR;

                if (_novo.CORTE_PETIT != null)
                    _produto.CORTE_PETIT = _novo.CORTE_PETIT;

                if (_novo.CORTE_VAREJO != null)
                    _produto.CORTE_VAREJO = _novo.CORTE_VAREJO;

                if (_novo.CORTE_ATACADO != null)
                    _produto.CORTE_ATACADO = _novo.CORTE_ATACADO;

                if (_novo.FORNECEDOR_COR != null)
                    _produto.FORNECEDOR_COR = _novo.FORNECEDOR_COR;

                if (_novo.REF_MODELAGEM != null)
                    _produto.REF_MODELAGEM = _novo.REF_MODELAGEM;

                if (_novo.PRODUTO_ACABADO != null)
                    _produto.PRODUTO_ACABADO = _novo.PRODUTO_ACABADO;

                if (_novo.CUSTO != null)
                    _produto.CUSTO = _novo.CUSTO;

                if (_novo.CUSTO_NOTA != null)
                    _produto.CUSTO_NOTA = _novo.CUSTO_NOTA;

                if (_novo.QTDE_MOSTRUARIO != null)
                    _produto.QTDE_MOSTRUARIO = _novo.QTDE_MOSTRUARIO;

                if (_novo.BRINDE != null)
                    _produto.BRINDE = _novo.BRINDE;

                _produto.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _produto.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _produto.STATUS = _novo.STATUS;
                _produto.DATA_APROVACAO = _novo.DATA_APROVACAO;

                if (_novo.OBS_MOSTRUARIO != null)
                    _produto.OBS_MOSTRUARIO = _novo.OBS_MOSTRUARIO;

                if (_novo.GRADEADO != null)
                    _produto.GRADEADO = _novo.GRADEADO;

                if (_novo.SIGNED != null)
                    _produto.SIGNED = _novo.SIGNED;
                if (_novo.SIGNED_NOME != null)
                    _produto.SIGNED_NOME = _novo.SIGNED_NOME;
                if (_novo.DATA_LIB_GEMEA != null)
                    _produto.DATA_LIB_GEMEA = _novo.DATA_LIB_GEMEA;

                if (_novo.DATA_MODELAGEM != null)
                    _produto.DATA_MODELAGEM = _novo.DATA_MODELAGEM;

                if (_novo.MODDESIGN != null)
                    _produto.MODDESIGN = _novo.MODDESIGN;

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
        public bool InserirAtualizarProduto(List<DESENV_PRODUTO> produtos)
        {
            bool retorno = false;

            foreach (DESENV_PRODUTO prod in produtos)
            {
                if (prod.CODIGO > 0)
                    AtualizarProduto(prod);
                else
                    InserirProduto(prod);
            }
            retorno = true;

            return retorno;
        }
        public void AtualizarProdutoConsumo(DESENV_PRODUTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO _produto = ObterProduto(_novo.CODIGO);

            if (_produto != null)
            {
                _produto.CODIGO = _novo.CODIGO;
                _produto.CONSUMO = _novo.CONSUMO;

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
        public void AtualizarProdutoSigned(DESENV_PRODUTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO _produto = ObterProduto(_novo.CODIGO);

            if (_produto != null)
            {
                _produto.CODIGO = _novo.CODIGO;
                _produto.SIGNED = _novo.SIGNED;

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
        public void ExcluirProduto(DESENV_PRODUTO _excluir)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO _produto = ObterProduto(_excluir.CODIGO);

            if (_produto != null)
            {
                _produto.CODIGO = _excluir.CODIGO;
                _produto.STATUS = _excluir.STATUS;
                _produto.DATA_APROVACAO = _excluir.DATA_APROVACAO;
                _produto.DATA_FICHATECNICA = _excluir.DATA_FICHATECNICA;
                _produto.DATA_EXCLUSAO = _excluir.DATA_EXCLUSAO;

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
        public void AprovarProduto(DESENV_PRODUTO _aprovar)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO _produto = ObterProduto(_aprovar.CODIGO);

            if (_produto != null)
            {
                _produto.DATA_APROVACAO = _aprovar.DATA_APROVACAO;
                _produto.DATA_FICHATECNICA = _aprovar.DATA_FICHATECNICA;
                _produto.USUARIO_FICHATECNICA = _aprovar.USUARIO_FICHATECNICA;

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
        //FIM DESENV_PRODUTO 

        //DESENV_PEDIDO
        public int ObterUltimoPedido()
        {
            int? numeroPedido = null;
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            numeroPedido = (from i in db.DESENV_PEDIDOs
                            select i).Max(p => p.NUMERO_PEDIDO);

            if (numeroPedido.HasValue)
                return Convert.ToInt32(numeroPedido += 1);

            return 1;
        }

        public List<DESENV_PEDIDO> ObterPedido1000()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDOs
                    where m.NUMERO_PEDIDO >= 1000
                    orderby m.DATA_PEDIDO, m.NUMERO_PEDIDO
                    select m).ToList();
        }
        public List<DESENV_PEDIDO> ObterPedido1000(string grupo, string subGrupo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDOs
                    where m.NUMERO_PEDIDO >= 1000
                    && m.GRUPO.Trim() == grupo.Trim()
                    && m.SUBGRUPO.Trim() == subGrupo.Trim()
                    orderby m.NUMERO_PEDIDO
                    select m).ToList();
        }
        public List<DESENV_PEDIDO> ObterPedido1000(string texto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDOs
                    where m.NUMERO_PEDIDO >= 1000
                    && (m.GRUPO.Trim().Contains(texto)
                        || m.SUBGRUPO.Trim().Contains(texto)
                        || m.COR.Trim().Contains(texto)
                        || m.COR_FORNECEDOR.Trim().Contains(texto))
                    orderby m.NUMERO_PEDIDO
                    select m).ToList();
        }
        [Obsolete("Utilize o método ObterPedido1000()")]
        public List<DESENV_PEDIDO> ObterPedido()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDOs
                    orderby m.DATA_PEDIDO, m.NUMERO_PEDIDO
                    select m).ToList();
        }
        public DESENV_PEDIDO ObterPedidoNumero(int numero_pedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDOs
                    where m.NUMERO_PEDIDO == numero_pedido
                    select m).SingleOrDefault();
        }
        public DESENV_PEDIDO ObterPedido(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirPedido(DESENV_PEDIDO _pedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PEDIDOs.InsertOnSubmit(_pedido);
                db.SubmitChanges();

                return _pedido.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedido(DESENV_PEDIDO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO _pedido = ObterPedido(_novo.CODIGO);

            if (_pedido != null)
            {
                _pedido.CODIGO = _novo.CODIGO;
                _pedido.COLECAO = _novo.COLECAO;
                _pedido.NUMERO_PEDIDO = _novo.NUMERO_PEDIDO;
                _pedido.FORNECEDOR = _novo.FORNECEDOR;
                _pedido.COR = _novo.COR;
                _pedido.DATA_PEDIDO = _novo.DATA_PEDIDO;
                _pedido.DATA_ENTREGA_PREV = _novo.DATA_ENTREGA_PREV;
                _pedido.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _pedido.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _pedido.OBSERVACAO = _novo.OBSERVACAO;
                _pedido.STATUS = _novo.STATUS;
                _pedido.UNIDADE_MEDIDA = _novo.UNIDADE_MEDIDA;
                _pedido.QTDE = _novo.QTDE;
                _pedido.VALOR = _novo.VALOR;
                _pedido.CONDICAO_PGTO = _novo.CONDICAO_PGTO;
                _pedido.CONDICAO_PGTO_OUTRO = _novo.CONDICAO_PGTO_OUTRO;
                _pedido.FOTO_TECIDO = _novo.FOTO_TECIDO;
                _pedido.NUMERO_PEDIDO_FORNECEDOR = _novo.NUMERO_PEDIDO_FORNECEDOR;
                _pedido.DATA_RESERVA = _novo.DATA_RESERVA;
                _pedido.GRUPO = _novo.GRUPO;
                _pedido.SUBGRUPO = _novo.SUBGRUPO;
                _pedido.COR_FORNECEDOR = _novo.COR_FORNECEDOR;
                _pedido.ESTOQUE_CONFERIDO = _novo.ESTOQUE_CONFERIDO;

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
        public void ExcluirPedido(DESENV_PEDIDO pedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO _pedido = ObterPedido(pedido.CODIGO);

            if (_pedido != null)
            {
                try
                {
                    db.DESENV_PEDIDOs.DeleteOnSubmit(_pedido);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void BaixarPedido(DESENV_PEDIDO _pedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO pedido = ObterPedido(_pedido.CODIGO);

            if (pedido != null)
            {
                pedido.CODIGO = _pedido.CODIGO;
                pedido.DATA_ENTREGA_REAL = _pedido.DATA_ENTREGA_REAL;
                pedido.STATUS = _pedido.STATUS;

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
        //FIM DESENV_PEDIDO

        //DESENV_PRODUTO_PEDIDO
        public DESENV_PRODUTO_PEDIDO ObterProdutoPedido(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDIDOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_PRODUTO_PEDIDO ObterProdutoPedidoProduto(int desenv_produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDIDOs
                    where m.DESENV_PRODUTO == desenv_produto
                    select m).SingleOrDefault();
        }
        public List<DESENV_PRODUTO_PEDIDO> ObterProdutoPedidoPedido(int desenv_pedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDIDOs where m.DESENV_PEDIDO == desenv_pedido select m).ToList();
        }
        public void InserirProdutoPedido(DESENV_PRODUTO_PEDIDO _produtoPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_PEDIDOs.InsertOnSubmit(_produtoPedido);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoPedidoProduto(int desenv_produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_PEDIDO _produtoPedido = ObterProdutoPedidoProduto(desenv_produto);

            if (_produtoPedido != null)
            {
                try
                {
                    db.DESENV_PRODUTO_PEDIDOs.DeleteOnSubmit(_produtoPedido);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM DESENV_PRODUTO_PEDIDO

        //DESENV_PRODUTO_HB
        public DESENV_PRODUTO_HB ObterProdutoHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_HBs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_PRODUTO_HB ObterProdutoHB(int prod_hb, int desenv_produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_HBs
                    where m.PROD_HB == prod_hb && m.DESENV_PRODUTO == desenv_produto
                    select m).SingleOrDefault();
        }
        public void InserirProdutoHB(DESENV_PRODUTO_HB _produtoHB)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_HBs.InsertOnSubmit(_produtoHB);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_HB _produtoHB = ObterProdutoHB(codigo);

            if (_produtoHB != null)
            {
                try
                {
                    db.DESENV_PRODUTO_HBs.DeleteOnSubmit(_produtoHB);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public PROD_HB ObterProdutoHBPorProduto(int desenv_produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_HBs
                    where m.DESENV_PRODUTO == desenv_produto
                    select m.PROD_HB1).SingleOrDefault();
        }

        //FIM DESENV_PRODUTO_HB

        //DESENV_PRODUTO_CARRINHO
        public DESENV_PRODUTO_CARRINHO ObterProdutoCarrinho(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.DESENV_PRODUTO_CARRINHOs where i.CODIGO == codigo select i).SingleOrDefault();
        }
        public List<DESENV_PRODUTO_CARRINHO> ObterProdutoCarrinho()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.DESENV_PRODUTO_CARRINHOs orderby i.DESENV_PRODUTO1.GRUPO, i.DESENV_PRODUTO1.MODELO select i).ToList();
        }
        public int InserirProdutoCarrinho(DESENV_PRODUTO_CARRINHO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_CARRINHOs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoCarrinho(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_CARRINHO _produtoCarrinho = ObterProdutoCarrinho(codigo);

            if (_produtoCarrinho != null)
            {
                try
                {
                    db.DESENV_PRODUTO_CARRINHOs.DeleteOnSubmit(_produtoCarrinho);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        // FIM DESENV_PRODUTO_CARRINHO

        //DESENV_PEDIDO_QTDE
        public DESENV_PEDIDO_QTDE ObterPedidoQtde(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_QTDEs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public List<DESENV_PEDIDO_QTDE> ObterPedidoQtdeEmissaoNF()
        {
            var dataInicio = new DateTime(2019, 10, 23).Date;

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_QTDEs where j.EMISSAO == null && j.DATA_NOTA > dataInicio && j.QTDE < 0 orderby j.DATA_NOTA select j).ToList();
        }
        public List<DESENV_PEDIDO_QTDE> ObterPedidoQtdeRetiradaBaixa()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_QTDEs
                    where j.EMISSAO != null
                    && j.QTDE < 0
                    && j.DATA_RETIRADA != null
                    && j.DATA_BAIXA_RETIRADA == null
                    orderby j.DATA_NOTA
                    select j).ToList();
        }
        public List<DESENV_PEDIDO_QTDE> ObterPedidoQtdePedido(int desenv_pedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_QTDEs where j.DESENV_PEDIDO == desenv_pedido select j).ToList();
        }
        public List<DESENV_PEDIDO_QTDE> ObterPedidoQtdeDevolucoes()
        {
            var dataInicio = new DateTime(2019, 10, 23).Date;
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_QTDEs
                    where
                        j.QTDE < 0
                    && j.DATA > dataInicio
                    && j.DESENV_PEDIDO_SUB > 0
                    orderby j.DATA
                    select j).ToList();
        }
        public List<DESENV_PEDIDO_QTDE> ObterPedidoQtdePedidoPorSub(int codigoPedidoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_QTDEs where j.DESENV_PEDIDO_SUB == codigoPedidoSub orderby j.DATA select j).ToList();
        }
        public int InserirPedidoQtde(DESENV_PEDIDO_QTDE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PEDIDO_QTDEs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedidoQtde(DESENV_PEDIDO_QTDE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_QTDE _pedidoQtde = ObterPedidoQtde(_novo.CODIGO);

            if (_pedidoQtde != null)
            {
                _pedidoQtde.CODIGO = _novo.CODIGO;
                _pedidoQtde.DATA = _novo.DATA;
                _pedidoQtde.DATA_IMPRESSAO = _novo.DATA_IMPRESSAO;
                _pedidoQtde.DESENV_PEDIDO = _novo.DESENV_PEDIDO;
                _pedidoQtde.NOTA_FISCAL = _novo.NOTA_FISCAL;
                _pedidoQtde.RENDIMENTO_MT = _novo.RENDIMENTO_MT;
                _pedidoQtde.QTDE = _novo.QTDE;

                _pedidoQtde.PRODUTO_FORNECEDOR = _novo.PRODUTO_FORNECEDOR;
                _pedidoQtde.VALOR_NOTA = _novo.VALOR_NOTA;
                _pedidoQtde.LARGURA = _novo.LARGURA;
                _pedidoQtde.VOLUME = _novo.VOLUME;

                _pedidoQtde.VENCIMENTO1 = _novo.VENCIMENTO1;
                _pedidoQtde.VENCIMENTO2 = _novo.VENCIMENTO2;
                _pedidoQtde.VENCIMENTO3 = _novo.VENCIMENTO3;
                _pedidoQtde.VENCIMENTO4 = _novo.VENCIMENTO4;
                _pedidoQtde.VENCIMENTO5 = _novo.VENCIMENTO5;
                _pedidoQtde.VENCIMENTO6 = _novo.VENCIMENTO6;

                _pedidoQtde.DATA_RETIRADA = _novo.DATA_RETIRADA;
                _pedidoQtde.OBS = _novo.OBS;
                _pedidoQtde.DATA_NOTA = _novo.DATA_NOTA;

                _pedidoQtde.TRANSPORTADORA = _novo.TRANSPORTADORA;
                _pedidoQtde.CNPJ = _novo.CNPJ;
                _pedidoQtde.NATUREZA_OPERACAO = _novo.NATUREZA_OPERACAO;
                _pedidoQtde.VALOR_MATERIAL = _novo.VALOR_MATERIAL;
                _pedidoQtde.PESO_KG = _novo.PESO_KG;
                _pedidoQtde.PESOLIQ_KG = _novo.PESOLIQ_KG;

                _pedidoQtde.EMISSAO = _novo.EMISSAO;
                _pedidoQtde.NF_DEV = _novo.NF_DEV;
                _pedidoQtde.SERIE_DEV = _novo.SERIE_DEV;
                _pedidoQtde.COD_FILIAL_DEV = _novo.COD_FILIAL_DEV;

                _pedidoQtde.FRETE = _novo.FRETE;

                _pedidoQtde.DATA_BAIXA_RETIRADA = _novo.DATA_BAIXA_RETIRADA;

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
        public void ExcluirPedidoQtde(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_QTDE _pedidoQtde = ObterPedidoQtde(codigo);

            if (_pedidoQtde != null)
            {
                try
                {
                    db.DESENV_PEDIDO_QTDEs.DeleteOnSubmit(_pedidoQtde);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM DESENV_PEDIDO_QTDE


        public List<SP_OBTER_DESENV_REL_GERALResult> ObterRelGeral(string col, char atacado, char? status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            //    var conn = db.Connection;

            //    var p = new DynamicParameters();
            //    p.Add("@P_COL", _col);
            //    p.Add("@P_ATACADO", atacado);
            //    p.Add("@P_STATUS", _status);

            //    var producaoGeral = conn.Query<SP_OBTER_DESENV_REL_GERALResult>("SP_OBTER_DESENV_REL_GERAL", p,
            //commandType: CommandType.StoredProcedure).ToList();

            //    return producaoGeral;


            return (from i in db.SP_OBTER_DESENV_REL_GERAL(col, atacado, status) select i).ToList();
        }
        //FIM SP_OBTER_DESENV_REL_GERALResult

        //DESENV_PRODUTO_ORIGEM
        public List<DESENV_PRODUTO_ORIGEM> ObterProdutoOrigem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_ORIGEMs
                    orderby m.COLECAO, m.DESCRICAO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO_ORIGEM> ObterProdutoOrigem(string col)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_ORIGEMs
                    where m.COLECAO.Trim() == col.Trim()
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public DESENV_PRODUTO_ORIGEM ObterProdutoOrigem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_ORIGEMs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirProdutoOrigem(DESENV_PRODUTO_ORIGEM _origem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_ORIGEMs.InsertOnSubmit(_origem);
                db.SubmitChanges();

                return _origem.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoOrigem(DESENV_PRODUTO_ORIGEM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_ORIGEM _origem = ObterProdutoOrigem(_novo.CODIGO);

            if (_origem != null)
            {
                _origem.CODIGO = _novo.CODIGO;
                _origem.COLECAO = _novo.COLECAO;
                _origem.DESCRICAO = _novo.DESCRICAO;
                _origem.DATA_INICIAL = _novo.DATA_INICIAL;
                _origem.DATA_FINAL = _novo.DATA_FINAL;
                _origem.STATUS = _novo.STATUS;

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
        public void ExcluirProdutoOrigem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_ORIGEM _produtoOrigem = ObterProdutoOrigem(codigo);

            if (_produtoOrigem != null)
            {
                try
                {
                    db.DESENV_PRODUTO_ORIGEMs.DeleteOnSubmit(_produtoOrigem);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM DESENV_PRODUTO_ORIGEM

        //DESENV_BUDGET
        public List<DESENV_BUDGET> ObterBudget()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_BUDGETs select j).ToList();
        }
        public List<DESENV_BUDGET> ObterBudget(string col)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_BUDGETs where j.COLECAO.Trim() == col.Trim() select j).ToList();
        }
        public DESENV_BUDGET ObterBudget(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_BUDGETs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        //FIM DESENV_BUDGET

        //SP_OBTER_DESENV_REL_RESUMO
        public List<SP_OBTER_DESENV_REL_RESUMOResult> ObterRelatorioResumo(string col, char atacado, char? status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_REL_RESUMO(col, atacado, status) select i).ToList();
        }
        //FIM SP_OBTER_DESENV_REL_RESUMO

        //SP_OBTER_HB_CORTADOResult
        public List<SP_OBTER_HB_CORTADOResult> ObterProdutoHBCortado(int? numeroPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_HB_CORTADO((numeroPedido == 0) ? null : numeroPedido) select i).ToList();
        }

        //SP_OBTER_DESENV_COLECAOResult>
        public List<SP_OBTER_DESENV_COLECAOResult> ObterDesenvolvimentoColecao(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_COLECAO(colecao.Trim()) select i).ToList();
        }

        // DESENV_CORE
        public List<DESENV_CORE> ObterCorPocket()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.DESENV_PRODUTOs
                    join j in db.DESENV_COREs
                    on i.FORNECEDOR_COR.Trim() equals j.COR.Trim() into corLeft
                    from c in corLeft.DefaultIfEmpty()
                    where i.FORNECEDOR_COR != null && i.FORNECEDOR_COR.Trim() != "" && i.STATUS == 'A'
                    orderby i.FORNECEDOR_COR
                    select new { i.FORNECEDOR_COR, c.IMAGEM }).AsEnumerable().Select(x => new DESENV_CORE { COR = x.FORNECEDOR_COR, IMAGEM = x.IMAGEM }).Distinct().ToList();
        }
        public DESENV_CORE ObterCorPocket(string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.DESENV_COREs where i.COR.Trim().ToUpper() == cor orderby i.COR select i).SingleOrDefault();
        }
        public void InserirCorPocket(DESENV_CORE _cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_COREs.InsertOnSubmit(_cor);
                db.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCorPocket(DESENV_CORE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_CORE _cor = ObterCorPocket(_novo.COR);

            if (_cor != null)
            {
                _cor.COR = _novo.COR;
                _cor.IMAGEM = _novo.IMAGEM;
                _cor.DATA_ALTERACAO = DateTime.Now;

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
        public void ExcluirCorPocket(string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_CORE _cor = ObterCorPocket(cor);

            if (_cor != null)
            {
                try
                {
                    db.DESENV_COREs.DeleteOnSubmit(_cor);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        // FIM DESEN_CORE

        //SP_OBTER_HB_PRODUTO_RELACIONADOResult
        public List<SP_OBTER_HB_PRODUTO_RELACIONADOResult> ObterProdutoHBRelacionado(string colecao, int? hb, string modelo, string nome)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_HB_PRODUTO_RELACIONADO(colecao, hb, modelo, nome) select i).ToList();
        }

        public List<VW_OBTER_HB_MODELO_POR_PEDIDO> ObterHBModeloPorPedido()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.VW_OBTER_HB_MODELO_POR_PEDIDOs orderby j.NUMERO_PEDIDO select j).ToList();
        }

        /*MATERIAIS_GRUPO - TABELA DE GRUPOS DE MATERIAL DO LINX*/
        public List<MATERIAIS_GRUPO> ObterMaterialGrupo()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_GRUPOs
                    where j.CODIGO_GRUPO.Trim() != "01" &&
                          j.CODIGO_GRUPO.Trim() != "02" &&
                          j.CODIGO_GRUPO.Trim() != "03" &&
                          j.CODIGO_GRUPO.Trim() != "04" &&
                          j.CODIGO_GRUPO.Trim() != "05"
                    orderby j.GRUPO
                    select j).ToList();
        }
        public MATERIAIS_GRUPO ObterMaterialGrupo(string grupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_GRUPOs where j.GRUPO == grupo select j).SingleOrDefault();
        }
        public void InserirMaterialGrupo(MATERIAIS_GRUPO _grupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.MATERIAIS_GRUPOs.InsertOnSubmit(_grupo);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialGrupo(MATERIAIS_GRUPO _novo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_GRUPO _materialGrupo = ObterMaterialGrupo(_novo.GRUPO);
            if (_materialGrupo != null)
            {
                _materialGrupo.GRUPO = _novo.GRUPO;
                _materialGrupo.CODIGO_GRUPO = _novo.CODIGO_GRUPO;
                _materialGrupo.CLASSE = "  ";
                _materialGrupo.FECHA_CM_AJUSTE_INFLACAO = false; ;
                _materialGrupo.INATIVO = false;

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
        public void ExcluirMaterialGrupo(string grupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_GRUPO _matGrupo = ObterMaterialGrupo(grupo);

            if (_matGrupo != null)
            {
                try
                {
                    db.MATERIAIS_GRUPOs.DeleteOnSubmit(_matGrupo);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*MATERIAIS_SUBGRUPO - TABELA DE SUB GRUPOS DO LINX*/
        public List<MATERIAIS_SUBGRUPO> ObterMaterialSubGrupo()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_SUBGRUPOs orderby j.CODIGO_SUBGRUPO select j).ToList();
        }
        public MATERIAIS_SUBGRUPO ObterMaterialSubGrupo(string grupo, string subGrupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_SUBGRUPOs where j.GRUPO == grupo && j.SUBGRUPO == subGrupo select j).SingleOrDefault();
        }
        public void InserirMaterialSubGrupo(MATERIAIS_SUBGRUPO _subgrupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.MATERIAIS_SUBGRUPOs.InsertOnSubmit(_subgrupo);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialSubGrupo(MATERIAIS_SUBGRUPO _novo, string subGrupoAtual)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_SUBGRUPO _materialSubGrupo = ObterMaterialSubGrupo(_novo.GRUPO, subGrupoAtual);

            if (_materialSubGrupo != null)
            {
                _materialSubGrupo.GRUPO = _novo.GRUPO;
                _materialSubGrupo.SUBGRUPO = _novo.SUBGRUPO;
                _materialSubGrupo.CODIGO_SUBGRUPO = _novo.CODIGO_SUBGRUPO;
                //_materialSubGrupo.CODIGO_SEQUENCIAL = _novo.CODIGO_SEQUENCIAL;
                _materialSubGrupo.UNIDADE_ESTOQUE = _novo.UNIDADE_ESTOQUE;
                _materialSubGrupo.FATOR_CONVERSAO = Convert.ToDecimal(1.000000000000);
                _materialSubGrupo.UNIDADE_FICHA_TEC = _novo.UNIDADE_ESTOQUE;
                _materialSubGrupo.FASE_PRODUCAO = "01";
                _materialSubGrupo.SETOR_PRODUCAO = "01";
                _materialSubGrupo.MRP_PARTICIPANTE = 0;

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
        public void ExcluirMaterialSubGrupo(string grupo, string subGrupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_SUBGRUPO _matSubGrupo = ObterMaterialSubGrupo(grupo, subGrupo);

            if (_matSubGrupo != null)
            {
                try
                {
                    db.MATERIAIS_SUBGRUPOs.DeleteOnSubmit(_matSubGrupo);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*UNIDADE - TABELA DE UNIDADE DO LINX*/
        public List<UNIDADE> ObterUnidadeMedidaLinx()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.UNIDADEs where j.UNIDADE1.Trim() == "KG" || j.UNIDADE1.Trim() == "UN" || j.UNIDADE1.Trim() == "MT" orderby j.DESC_UNIDADE select j).ToList();
        }
        public UNIDADE ObterUnidadeMedidaLinx(string unidadeMedida)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.UNIDADEs where j.UNIDADE1 == unidadeMedida select j).SingleOrDefault();
        }

        /*MATERIAIS_FORNECEDOR - TABELA DE FORNECEDORES DE MATERIAL DO LINX*/
        public List<MATERIAIS_FORNECEDOR> ObterMaterialFornecedor()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_FORNECEDORs orderby j.FORNECEDOR, j.MATERIAL, j.COR_MATERIAL select j).ToList();
        }
        public MATERIAIS_FORNECEDOR ObterMaterialFornecedor(string material, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_FORNECEDORs
                    where
                        j.MATERIAL == material &&
                        j.COR_MATERIAL == cor
                    select j).SingleOrDefault();
        }
        public void InserirMaterialFornecedor(MATERIAIS_FORNECEDOR _fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.MATERIAIS_FORNECEDORs.InsertOnSubmit(_fornecedor);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialFornecedor(MATERIAIS_FORNECEDOR _novo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_FORNECEDOR _materialFornecedor = ObterMaterialFornecedor(_novo.MATERIAL, _novo.COR_MATERIAL);

            if (_materialFornecedor != null)
            {
                _materialFornecedor.FORNECEDOR = _novo.FORNECEDOR;
                _materialFornecedor.MATERIAL = _novo.MATERIAL;
                _materialFornecedor.COR_MATERIAL = _novo.COR_MATERIAL;


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
        public void ExcluirMaterialFornecedor(string material, string corMaterial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_FORNECEDOR _matFornecedor = ObterMaterialFornecedor(material, corMaterial);

            if (_matFornecedor != null)
            {
                try
                {
                    db.MATERIAIS_FORNECEDORs.DeleteOnSubmit(_matFornecedor);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*MATERIAIS_CORES - TABELA DE CORES DO MATERIAL DO LINX*/
        public List<SP_OBTER_MATERIAL_CORESResult> ObterMaterialCorV2(string grupo, string subGrupo, string material)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from j in dbLINX.SP_OBTER_MATERIAL_CORES(grupo, subGrupo, material) select j).ToList();
        }
        public List<MATERIAIS_CORE> ObterMaterialCor()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_COREs orderby j.MATERIAL, j.COR_MATERIAL select j).ToList();
        }
        public MATERIAIS_CORE ObterMaterialCor(string material, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_COREs
                    where
                        j.MATERIAL == material &&
                        j.COR_MATERIAL == cor
                    select j).SingleOrDefault();
        }
        public MATERIAIS_CORE ObterMaterialCor(string material, string cor, string corFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_COREs
                    where
                        j.MATERIAL == material &&
                        j.COR_MATERIAL == cor &&
                        j.REFER_FABRICANTE == corFornecedor
                    select j).SingleOrDefault();
        }
        public void InserirMaterialCor(MATERIAIS_CORE _materialCor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.MATERIAIS_COREs.InsertOnSubmit(_materialCor);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialCor(MATERIAIS_CORE _novo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_CORE _materialCor = ObterMaterialCor(_novo.MATERIAL, _novo.COR_MATERIAL);

            if (_materialCor != null)
            {
                _materialCor.MATERIAL = _novo.MATERIAL;
                _materialCor.COR_MATERIAL = _novo.COR_MATERIAL;

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
        public void ExcluirMaterialCor(string material, string corMaterial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAIS_CORE _matCor = ObterMaterialCor(material, corMaterial);

            if (_matCor != null)
            {
                try
                {
                    db.MATERIAIS_COREs.DeleteOnSubmit(_matCor);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*MATERIAIS - TABELA DE PRINCIPAL DE MATERIAIS DO LINX*/
        public List<MATERIAI> ObterMaterial()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIs orderby j.MATERIAL select j).ToList();
        }
        public List<MATERIAI> ObterMaterial(string grupo, string subGrupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIs
                    where
                        j.GRUPO.Trim() == grupo &&
                        j.SUBGRUPO.Trim() == subGrupo
                    orderby j.MATERIAL
                    select j).ToList();
        }
        public MATERIAI ObterMaterial(string material)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIs
                    where
                        j.MATERIAL == material
                    select j).SingleOrDefault();
        }
        public void InserirMaterial(MATERIAI _material)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.MATERIAIs.InsertOnSubmit(_material);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterial(MATERIAI _novo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAI _material = ObterMaterial(_novo.MATERIAL);

            if (_material != null)
            {
                _material.MATERIAL = _novo.MATERIAL;
                _material.DESC_MATERIAL = _novo.DESC_MATERIAL;
                _material.SUBGRUPO = _novo.SUBGRUPO;
                _material.GRUPO = _novo.GRUPO;
                _material.TIPO = _novo.TIPO;
                _material.FASE_PRODUCAO = _novo.FASE_PRODUCAO;
                _material.SETOR_PRODUCAO = _novo.SETOR_PRODUCAO;
                _material.TRIBUT_ICMS = _novo.TRIBUT_ICMS;
                _material.TRIBUT_ORIGEM = _novo.TRIBUT_ORIGEM;
                _material.CLASSIF_FISCAL = _novo.CLASSIF_FISCAL;
                _material.FABRICANTE = _novo.FABRICANTE;
                _material.COLECAO = _novo.COLECAO;
                _material.FATOR_CONVERSAO = _novo.FATOR_CONVERSAO;
                _material.UNID_ESTOQUE = _novo.UNID_ESTOQUE;
                _material.UNID_FICHA_TEC = _novo.UNID_FICHA_TEC;
                _material.CONDICAO_PGTO = _novo.CONDICAO_PGTO;
                _material.CTRL_UNID_AUX = _novo.CTRL_UNID_AUX;
                _material.COD_MATERIAL_TAMANHO = _novo.COD_MATERIAL_TAMANHO;
                _material.VARIA_MATERIAL_TAMANHO = _novo.VARIA_MATERIAL_TAMANHO;
                _material.CTRL_PARTIDAS = _novo.CTRL_PARTIDAS;
                _material.CTRL_PECAS = _novo.CTRL_PECAS;
                _material.CTRL_PECAS_PARCIAL = _novo.CTRL_PECAS_PARCIAL;
                _material.DATA_REPOSICAO = _novo.DATA_REPOSICAO;
                _material.CUSTO_A_VISTA = _novo.CUSTO_A_VISTA;
                _material.TAXA_JUROS_CUSTO = _novo.TAXA_JUROS_CUSTO;
                _material.MATERIAL_INDIRETO = _novo.MATERIAL_INDIRETO;
                _material.RESERVA_MATERIAL_OP = _novo.RESERVA_MATERIAL_OP;
                _material.ABATER_RESERVA_QTDE = _novo.ABATER_RESERVA_QTDE;
                _material.CONTA_CONTABIL = _novo.CONTA_CONTABIL;
                _material.COMPOSICAO = _novo.COMPOSICAO;
                _material.INDICADOR_CFOP = _novo.INDICADOR_CFOP;
                _material.MRP_AGRUPAR_NECESSIDADE_TIPO = _novo.MRP_AGRUPAR_NECESSIDADE_TIPO;
                _material.CONTA_CONTABIL_COMPRA = _novo.CONTA_CONTABIL_COMPRA;
                _material.CONTA_CONTABIL_VENDA = _novo.CONTA_CONTABIL_VENDA;
                _material.CONTA_CONTABIL_DEV_COMPRA = _novo.CONTA_CONTABIL_DEV_COMPRA;
                _material.CONTA_CONTABIL_DEV_VENDA = _novo.CONTA_CONTABIL_DEV_VENDA;
                _material.TIPO_ITEM_SPED = _novo.TIPO_ITEM_SPED;

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
        public void ExcluirMaterial(string material)
        {
            db = new DCDataContext(Constante.ConnectionString);
            MATERIAI _mat = ObterMaterial(material);

            if (_mat != null)
            {
                try
                {
                    db.MATERIAIs.DeleteOnSubmit(_mat);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<MATERIAIS_TIPO> ObterMaterialTipo()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.MATERIAIS_TIPOs orderby j.TIPO select j).ToList();
        }

        //VALIDAR EXCLUSAO DO MATERIAL
        public int ValidarExclusaoMaterial(string material, string corMaterial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            var retorno = (from j in db.SP_VALIDAR_EXCLUSAO_MATERIAL(material, corMaterial) select j).SingleOrDefault();
            return Convert.ToInt32(retorno.OK);
        }

        /*MATERIAIS - TABELA DE PRINCIPAL DE MATERIAIS DO LINX*/
        public List<ESTOQUE_MATERIAI> ObterMaterialEstoque(string material, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.ESTOQUE_MATERIAIs
                    where
                    j.MATERIAL.Trim() == material.Trim() &&
                    j.COR_MATERIAL.Trim() == cor.Trim()
                    orderby j.MATERIAL, j.COR_MATERIAL
                    select j).ToList();
        }
        public List<ESTOQUE_MATERIAI> ObterMaterialEstoque(string material)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.ESTOQUE_MATERIAIs
                    where
                    j.MATERIAL.Trim() == material.Trim()
                    orderby j.MATERIAL, j.COR_MATERIAL
                    select j).ToList();
        }

        /*CLASSIFICAÇÃO FISCAL - TABELA DE CLASSIFICAÇÂO FISCAL*/
        public List<CLASSIF_FISCAL> ObterClassificacaoFiscal()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CLASSIF_FISCALs
                    where j.CLASSIF_FISCAL1.Trim() != ""
                    orderby j.CLASSIF_FISCAL1, j.DESC_CLASSIFICACAO
                    select j).ToList();
        }
        public CLASSIF_FISCAL ObterClassificacaoFiscal(string classif_fiscal)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CLASSIF_FISCALs
                    where j.CLASSIF_FISCAL1.Trim() == classif_fiscal
                    select j).SingleOrDefault();
        }

        /*TRIBUT_ORIGEM - TABELA DE ORIGEM DOS TRIBUTOS*/
        public List<TRIBUT_ORIGEM> ObterTributacaoOrigem()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.TRIBUT_ORIGEMs
                    orderby j.TRIBUT_ORIGEM1
                    select j).ToList();
        }
        public TRIBUT_ORIGEM ObterTributacaoOrigem(string tribut_origem)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.TRIBUT_ORIGEMs
                    where j.TRIBUT_ORIGEM1.Trim() == tribut_origem
                    select j).SingleOrDefault();
        }

        /*CTB_CONTA_PLANO - TABELA DE CONTAS CONTABEIS*/
        public List<CTB_CONTA_PLANO> ObterContaContabil()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CTB_CONTA_PLANOs
                    where j.CONTA_CONTABIL.Trim() != ""
                    orderby j.CONTA_CONTABIL
                    select j).ToList();
        }
        public CTB_CONTA_PLANO ObterContaContabil(string conta_contabil)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CTB_CONTA_PLANOs
                    where j.CONTA_CONTABIL.Trim() == conta_contabil.Trim()
                    select j).SingleOrDefault();
        }
        public List<CTB_CONTA_PLANO> ObterContaContabilDescricao(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CTB_CONTA_PLANOs
                    where j.DESC_CONTA.Trim() == descricao.Trim()
                    select j).ToList();
        }

        /*CTB_LX_INDICADOR_CFOP - TABELA DE TIPOS DE SPED*/
        public List<CTB_LX_INDICADOR_CFOP> ObterCaractContabil()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CTB_LX_INDICADOR_CFOPs
                    orderby j.INDICADOR_CFOP
                    select j).ToList();
        }
        public CTB_LX_INDICADOR_CFOP ObterCaractContabil(int indicador_cfop)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.CTB_LX_INDICADOR_CFOPs
                    where j.INDICADOR_CFOP == indicador_cfop
                    select j).SingleOrDefault();
        }

        /*CTB_LX_INDICADOR_CFOP - TABELA DE TIPOS DE SPED*/
        public List<LCF_LX_ITEM_TIPO> ObterTipoSped()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.LCF_LX_ITEM_TIPOs
                    orderby j.COD_TIPO_SPED
                    select j).ToList();
        }
        public LCF_LX_ITEM_TIPO ObterTipoSped(string cod_tipo_sped)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from j in db.LCF_LX_ITEM_TIPOs
                    where j.COD_TIPO_SPED == cod_tipo_sped
                    select j).SingleOrDefault();
        }

        /*MATERIAL_FOTO - TABELA DE FOTOS DA INTRANET*/
        public List<MATERIAL_FOTO> ObterMaterialFotoIntranet()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.MATERIAL_FOTOs orderby j.MATERIAL select j).ToList();
        }
        public MATERIAL_FOTO ObterMaterialFotoIntranet(string material)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.MATERIAL_FOTOs
                    where
                        j.MATERIAL == material
                    select j).SingleOrDefault();
        }
        public void InserirMaterialFotoIntranet(MATERIAL_FOTO _materialFoto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.MATERIAL_FOTOs.InsertOnSubmit(_materialFoto);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialFotoIntranet(MATERIAL_FOTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            MATERIAL_FOTO _materialFoto = ObterMaterialFotoIntranet(_novo.MATERIAL);

            if (_materialFoto != null)
            {
                _materialFoto.MATERIAL = _novo.MATERIAL;
                _materialFoto.FOTO = _novo.FOTO;

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
        public void ExcluirMaterialFotoIntranet(string material)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            MATERIAL_FOTO _materialFoto = ObterMaterialFotoIntranet(material);

            if (_materialFoto != null)
            {
                try
                {
                    db.MATERIAL_FOTOs.DeleteOnSubmit(_materialFoto);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /*MATERIAL_COMPOSICAO - TABELA DE COMPOSICAO DO MATERIAL DA INTRANET*/
        public List<MATERIAL_COMPOSICAO> ObterMaterialComposicao(string material)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.MATERIAL_COMPOSICAOs where m.MATERIAL == material select m).ToList();
        }
        public MATERIAL_COMPOSICAO ObterMaterialComposicao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.MATERIAL_COMPOSICAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public void InserirMaterialComposicao(MATERIAL_COMPOSICAO _composicao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.MATERIAL_COMPOSICAOs.InsertOnSubmit(_composicao);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirMaterialComposicao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                MATERIAL_COMPOSICAO _composicao = ObterMaterialComposicao(codigo);
                if (_composicao != null)
                {
                    db.MATERIAL_COMPOSICAOs.DeleteOnSubmit(_composicao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM MATERIAL_COMPOSICAO

        /*DESENV_PRODUTO_FICTEC - TABELA DE FICHA TECNICA DO PRODUTO DA INTRANET*/
        public List<DESENV_PRODUTO_FICTEC> ObterFichaTecnica()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_FICTECs
                    orderby m.GRUPO, m.SUBGRUPO, m.COR_MATERIAL
                    select m).ToList();
        }
        public List<DESENV_PRODUTO_FICTEC> ObterFichaTecnicaPorMaterial(string material, string corMaterial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_FICTECs
                    where m.MATERIAL == material
                    && m.COR_MATERIAL == corMaterial
                    select m).ToList();
        }
        public DESENV_PRODUTO_FICTEC ObterFichaTecnicaPorMaterial(int desenvProduto, string material, string corMaterial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_FICTECs
                    where m.MATERIAL == material
                    && m.COR_MATERIAL == corMaterial
                    && m.DESENV_PRODUTO == desenvProduto
                    select m).FirstOrDefault();
        }
        public List<DESENV_PRODUTO_FICTEC> ObterFichaTecnica(int desenv_produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_FICTECs where m.DESENV_PRODUTO == desenv_produto orderby m.MATERIAL select m).ToList();
        }
        public DESENV_PRODUTO_FICTEC ObterFichaTecnicaCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_FICTECs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public void InserirFichaTecnica(DESENV_PRODUTO_FICTEC _fichaTecnica)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_FICTECs.InsertOnSubmit(_fichaTecnica);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFichaTecnica(DESENV_PRODUTO_FICTEC _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_FICTEC _fichaTecnica = ObterFichaTecnicaCodigo(_novo.CODIGO);

            if (_fichaTecnica != null)
            {
                _fichaTecnica.PROD_DETALHE = _novo.PROD_DETALHE;
                _fichaTecnica.MATERIAL = _novo.MATERIAL;
                _fichaTecnica.GRUPO = _novo.GRUPO;
                _fichaTecnica.SUBGRUPO = _novo.SUBGRUPO;
                _fichaTecnica.COR_MATERIAL = _novo.COR_MATERIAL;
                _fichaTecnica.COR_FORNECEDOR = _novo.COR_FORNECEDOR;
                _fichaTecnica.FORNECEDOR = _novo.FORNECEDOR;
                _fichaTecnica.CONSUMO = _novo.CONSUMO;
                _fichaTecnica.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _fichaTecnica.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

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
        public void ExcluirFichaTecnica(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_FICTEC _produtoFichaTecnica = ObterFichaTecnicaCodigo(codigo);
                if (_produtoFichaTecnica != null)
                {
                    db.DESENV_PRODUTO_FICTECs.DeleteOnSubmit(_produtoFichaTecnica);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_PRODUTO_FICTEC

        //SP_OBTER_MATERIAL_PRODUTOResult
        public List<SP_OBTER_MATERIAL_PRODUTOResult> ObterMaterialProduto(string modelo, string cor, string nome, string grupo, string subGrupo, string fornecedor, string cor_material, string corFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_MATERIAL_PRODUTO(modelo, cor, nome, grupo, subGrupo, fornecedor, cor_material, corFornecedor) select i).ToList();
        }

        //SP_OBTER_MATERIAL_SALDOResult
        public List<SP_OBTER_MATERIAL_SALDOResult> ObterMaterialSaldo(string grupo, string subGrupo, string cor, string corFornecedor, string fornecedor, int query)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_MATERIAL_SALDO(grupo, subGrupo, cor, corFornecedor, fornecedor, query) select i).ToList();
        }

        //DESENV_PRODUTO_PRODUCAO
        public DESENV_PRODUTO_PRODUCAO ObterProdutoProducaoCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PRODUCAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<DESENV_PRODUTO_PRODUCAO> ObterProdutoProducao(int desenv_produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PRODUCAOs
                    where m.DESENV_PRODUTO == desenv_produto
                    select m).ToList();
        }
        public int InserirProdutoProducao(DESENV_PRODUTO_PRODUCAO _produtoProducao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_PRODUCAOs.InsertOnSubmit(_produtoProducao);
                db.SubmitChanges();

                return _produtoProducao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoProducao(DESENV_PRODUTO_PRODUCAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_PRODUCAO _produtoProducao = ObterProdutoProducaoCodigo(_novo.CODIGO);

            if (_produtoProducao != null)
            {
                _produtoProducao.DESENV_PRODUTO = _novo.DESENV_PRODUTO;
                _produtoProducao.PROD_DETALHE = _novo.PROD_DETALHE;
                _produtoProducao.QTDE = _novo.QTDE;
                _produtoProducao.DATA_EXCLUSAO = _novo.DATA_EXCLUSAO;
                _produtoProducao.TIPO = _novo.TIPO;
                _produtoProducao.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _produtoProducao.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _produtoProducao.STATUS = _novo.STATUS;
                _produtoProducao.DATA_LIBERACAO = _novo.DATA_LIBERACAO;
                _produtoProducao.USUARIO_LIBERACAO = _novo.USUARIO_LIBERACAO;
                _produtoProducao.PROD_HB = _novo.PROD_HB;

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
        public void ExcluirProdutoProducao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_PRODUCAO _produtoProducao = ObterProdutoProducaoCodigo(codigo);

            if (_produtoProducao != null)
            {
                try
                {
                    db.DESENV_PRODUTO_PRODUCAOs.DeleteOnSubmit(_produtoProducao);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //SP_OBTER_LIBERACAO_PRODUTOResult
        public List<SP_OBTER_LIBERACAO_PRODUTOResult> ObterLiberacaoProduto(string modelo, string nome, string grupo, string subGrupo, string cor, string corFornecedor, string tipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_LIBERACAO_PRODUTO(modelo, nome, grupo, subGrupo, cor, corFornecedor, tipo) select i).ToList();
        }

        //SP_OBTER_PEDIDOS_ENTRADAResult
        public List<SP_OBTER_PEDIDOS_ENTRADAResult> ObterRelPedidosEntrada(int? numeroPedido, string notaFiscal, string grupo, string subGrupo, string cor, string corFornecedor, string fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_PEDIDOS_ENTRADA(numeroPedido, notaFiscal, grupo, subGrupo, cor, corFornecedor, fornecedor) select i).ToList();
        }

        //SP_OBTER_VENDA_ATACADO_PRODUTOResult
        public List<SP_OBTER_VENDA_ATACADO_PRODUTOResult> ObterVendaAtacadoProduto(string colecao, string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_VENDA_ATACADO_PRODUTO(colecao.Trim(), produto.Trim(), cor.Trim()) select i).ToList();
        }

        //SP_OBTER_NECESSIDADE_COMPRAResult
        public List<SP_OBTER_NECESSIDADE_COMPRAResult> ObterNecessidadeCompraMaterial(string grupo, string subGrupo, string cor, string corFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_NECESSIDADE_COMPRA(grupo, subGrupo, cor, corFornecedor) select i).ToList();
        }

        public List<SP_OBTER_PEDIDOSUB_CONTROLEResult> ObterPedidoSubControle(string fornecedor, string colecao, int pedido, string grupo, string subGrupo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PEDIDOSUB_CONTROLE(fornecedor, colecao, pedido, grupo, subGrupo) select i).ToList();
        }

        //DESENV_PEDIDO_SUB
        public DESENV_PEDIDO_SUB ObterPedidoSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_SUBs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public List<DESENV_PEDIDO_SUB> ObterPedidoSubPorDesenvPedido(int codigoDesenvPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_SUBs where j.DESENV_PEDIDO == codigoDesenvPedido orderby j.DATA_INCLUSAO descending select j).ToList();
        }
        public int InserirPedidoSub(DESENV_PEDIDO_SUB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PEDIDO_SUBs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedidoSub(DESENV_PEDIDO_SUB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_SUB _pedidoSub = ObterPedidoSub(_novo.CODIGO);

            if (_pedidoSub != null)
            {
                _pedidoSub.CODIGO = _novo.CODIGO;
                _pedidoSub.DESENV_PEDIDO = _novo.DESENV_PEDIDO;
                _pedidoSub.FORNECEDOR = _novo.FORNECEDOR;
                _pedidoSub.QTDE = _novo.QTDE;
                _pedidoSub.VALOR = _novo.VALOR;
                _pedidoSub.DATA_PEDIDO = _novo.DATA_PEDIDO;
                _pedidoSub.DATA_ENTREGA_PREV = _novo.DATA_ENTREGA_PREV;
                _pedidoSub.DATA_ENTREGA_REAL = _novo.DATA_ENTREGA_REAL;
                _pedidoSub.EMAIL = _novo.EMAIL;

                _pedidoSub.COLECAO = _novo.COLECAO;
                _pedidoSub.ITEM = _novo.ITEM;
                _pedidoSub.DATA_PREVISAO_FINAL = _novo.DATA_PREVISAO_FINAL;
                _pedidoSub.DATA_BAIXA = _novo.DATA_BAIXA;
                _pedidoSub.STATUS = _novo.STATUS;

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
        public void ExcluirPedidoSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_SUB _pedidoSub = ObterPedidoSub(codigo);

            if (_pedidoSub != null)
            {
                try
                {
                    db.DESENV_PEDIDO_SUBs.DeleteOnSubmit(_pedidoSub);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM DESENV_PEDIDO_SUB

        //DESENV_PEDIDO_SUB
        public List<DESENV_PEDIDO_SUB_PGTO> ObterPedidoSubPgtoPorDesenvPedidoSub(int codigoSubPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_SUB_PGTOs where j.DESENV_PEDIDO_SUB == codigoSubPedido orderby j.PARCELA ascending select j).ToList();
        }
        public DESENV_PEDIDO_SUB_PGTO ObterPedidoSubPgtoPorParcela(int codigoSubPedido, int parcela)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_SUB_PGTOs where j.DESENV_PEDIDO_SUB == codigoSubPedido && j.PARCELA == parcela select j).SingleOrDefault();
        }
        public DESENV_PEDIDO_SUB_PGTO ObterPedidoSubPgto(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.DESENV_PEDIDO_SUB_PGTOs where j.CODIGO == codigo select j).SingleOrDefault();
        }
        public int InserirPedidoSubPgto(DESENV_PEDIDO_SUB_PGTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PEDIDO_SUB_PGTOs.InsertOnSubmit(_novo);
                db.SubmitChanges();

                return _novo.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPedidoSubPgto(DESENV_PEDIDO_SUB_PGTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_SUB_PGTO _pedidoSubPgto = ObterPedidoSubPgto(_novo.CODIGO);

            if (_pedidoSubPgto != null)
            {
                _pedidoSubPgto.CODIGO = _novo.CODIGO;
                _pedidoSubPgto.DESENV_PEDIDO_SUB = _novo.DESENV_PEDIDO_SUB;
                _pedidoSubPgto.PARCELA = _novo.PARCELA;
                _pedidoSubPgto.VALOR = _novo.VALOR;
                _pedidoSubPgto.PORC = _novo.PORC;
                _pedidoSubPgto.DATA_PAGAMENTO = _novo.DATA_PAGAMENTO;
                _pedidoSubPgto.DATA_BAIXA = _novo.DATA_BAIXA;
                _pedidoSubPgto.OBS = _novo.OBS;

                _pedidoSubPgto.NOTA_FISCAL = _novo.NOTA_FISCAL;
                _pedidoSubPgto.EMISSAO = _novo.EMISSAO;

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
        public void ExcluirPedidoSubPgto(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_SUB_PGTO _pedidoSubPgto = ObterPedidoSubPgto(codigo);

            if (_pedidoSubPgto != null)
            {
                try
                {
                    db.DESENV_PEDIDO_SUB_PGTOs.DeleteOnSubmit(_pedidoSubPgto);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirPedidoSubPgto(int codigoPedidoSub, int parcela)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PEDIDO_SUB_PGTO pedidoSubPgto = ObterPedidoSubPgtoPorParcela(codigoPedidoSub, parcela);
                if (pedidoSubPgto != null)
                {
                    db.DESENV_PEDIDO_SUB_PGTOs.DeleteOnSubmit(pedidoSubPgto);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_PEDIDO_SUB

        public SP_GERAR_PEDIDO_COMPRA_TECIDOResult GerarPedidoCompraTecidoIntra(string fornecedor, string colecao, DateTime entrega, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_GERAR_PEDIDO_COMPRA_TECIDO(fornecedor, colecao, entrega, codigoUsuario) select i).SingleOrDefault();
        }
        public SP_GERAR_PEDIDO_COMPRA_TECIDO_RESResult GerarPedidoCompraTecidoIntraReservado(string fornecedor, string colecao, DateTime entrega, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_GERAR_PEDIDO_COMPRA_TECIDO_RES(fornecedor, colecao, entrega, codigoUsuario) select i).SingleOrDefault();
        }

        //SP_OBTER_HB_AVIAMENTO_PEDIDOResult
        public List<SP_OBTER_HB_AVIAMENTO_PEDIDOResult> ObterAviamentoHBPedido(string grupo, string subGrupo, string cor, string corFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_HB_AVIAMENTO_PEDIDO(grupo, subGrupo, cor, corFornecedor) select i).ToList();
        }

        public List<SP_OBTER_LAVAGEMResult> ObterLavagemGrid()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_LAVAGEM() select i).ToList();
        }
        public DESENV_LAVAGEM ObterLavagem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_LAVAGEMs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<DESENV_LAVAGEM> ObterLavagem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_LAVAGEMs
                    orderby m.CODIGO
                    select m).ToList();
        }

        /*MATERIAL_LAVAGEM */
        public List<MATERIAL_LAVAGEM> ObterMaterialLavagem(string material)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.MATERIAL_LAVAGEMs where m.MATERIAL == material select m).OrderBy(p => p.ORDEM).ToList();
        }
        public MATERIAL_LAVAGEM ObterMaterialLavagem(string material, string imagem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.MATERIAL_LAVAGEMs
                    where
                    m.MATERIAL.Trim() == material.Trim() &&
                    m.IMAGEM.Trim() == imagem.Trim()
                    select m).SingleOrDefault();
        }
        public void InserirMaterialLavagem(MATERIAL_LAVAGEM _lavagem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.MATERIAL_LAVAGEMs.InsertOnSubmit(_lavagem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirMaterialLavagem(string material, string imagem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                MATERIAL_LAVAGEM _lavagem = ObterMaterialLavagem(material, imagem);
                if (_lavagem != null)
                {
                    db.MATERIAL_LAVAGEMs.DeleteOnSubmit(_lavagem);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM MATERIAL_LAVAGEM

        /*DESENV_ACESSORIO */
        public List<DESENV_ACESSORIO> ObterAcessorio(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIOs where m.COLECAO == colecao orderby m.COLECAO select m).ToList();
        }
        public List<DESENV_ACESSORIO> ObterAcessorioPorProduto(string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIOs where m.PRODUTO == produto && m.STATUS == 'A' orderby m.COLECAO select m).ToList();
        }
        public DESENV_ACESSORIO ObterAcessorio(string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIOs where m.PRODUTO == produto && m.COR == cor && m.STATUS == 'A' orderby m.COLECAO select m).FirstOrDefault();
        }
        public DESENV_ACESSORIO ObterAcessorio(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirAcessorio(DESENV_ACESSORIO _acessorio)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_ACESSORIOs.InsertOnSubmit(_acessorio);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAcessorio(DESENV_ACESSORIO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_ACESSORIO _acessorio = ObterAcessorio(_novo.CODIGO);

            if (_acessorio != null)
            {
                _acessorio.CODIGO = _novo.CODIGO;
                _acessorio.COLECAO = _novo.COLECAO;
                _acessorio.DESENV_PRODUTO_ORIGEM = _novo.DESENV_PRODUTO_ORIGEM;
                _acessorio.GRUPO = _novo.GRUPO;
                _acessorio.PRODUTO = _novo.PRODUTO;
                _acessorio.COR = _novo.COR;
                _acessorio.COR_FORNECEDOR = _novo.COR_FORNECEDOR;
                _acessorio.FORNECEDOR = _novo.FORNECEDOR;
                _acessorio.GRIFFE = _novo.GRIFFE;
                _acessorio.DATA_APROVACAO = _novo.DATA_APROVACAO;
                _acessorio.FOTO1 = _novo.FOTO1;
                _acessorio.FOTO2 = _novo.FOTO2;
                _acessorio.OBS = _novo.OBS;
                _acessorio.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _acessorio.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _acessorio.STATUS = _novo.STATUS;
                _acessorio.PRECO = _novo.PRECO;
                _acessorio.CUSTO = _novo.CUSTO;
                _acessorio.CUSTO_NOTA = _novo.CUSTO_NOTA;
                _acessorio.QTDE = _novo.QTDE;
                _acessorio.DESCRICAO_SUGERIDA = _novo.DESCRICAO_SUGERIDA;
                _acessorio.REFER_FABRICANTE = _novo.REFER_FABRICANTE;
                _acessorio.DATA_PREVISAO_ENTREGA = _novo.DATA_PREVISAO_ENTREGA;

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
        public void ExcluirAcessorio(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_ACESSORIO _acessorio = ObterAcessorio(codigo);
                if (_acessorio != null)
                {
                    db.DESENV_ACESSORIOs.DeleteOnSubmit(_acessorio);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_ACESSORIO


        /*DESENV_ACESSORIO_CARRINHO */
        public List<DESENV_ACESSORIO_CARRINHO> ObterCarrinhoAcessorioPorUsuario(int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHOs where m.USUARIO == codigoUsuario orderby m.DATA_INCLUSAO select m).ToList();
        }
        public DESENV_ACESSORIO_CARRINHO ObterCarrinhoAcessorio(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_ACESSORIO_CARRINHO ObterCarrinhoAcessorio(int codigoAcessorio, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHOs
                    where
                        m.DESENV_ACESSORIO == codigoAcessorio
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoAcessorio(DESENV_ACESSORIO_CARRINHO _acessorio)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_ACESSORIO_CARRINHOs.InsertOnSubmit(_acessorio);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoAcessorio(DESENV_ACESSORIO_CARRINHO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_ACESSORIO_CARRINHO _acessorio = ObterCarrinhoAcessorio(_novo.CODIGO);

            if (_acessorio != null)
            {
                _acessorio.CODIGO = _novo.CODIGO;
                _acessorio.DESENV_ACESSORIO = _novo.DESENV_ACESSORIO;
                _acessorio.GRADE1 = _novo.GRADE1;
                _acessorio.GRADE2 = _novo.GRADE2;
                _acessorio.GRADE3 = _novo.GRADE3;
                _acessorio.GRADE4 = _novo.GRADE4;
                _acessorio.GRADE5 = _novo.GRADE5;
                _acessorio.GRADE6 = _novo.GRADE6;
                _acessorio.GRADE7 = _novo.GRADE7;
                _acessorio.GRADE8 = _novo.GRADE8;
                _acessorio.GRADE9 = _novo.GRADE9;
                _acessorio.GRADE10 = _novo.GRADE10;
                _acessorio.GRADE11 = _novo.GRADE11;
                _acessorio.GRADE12 = _novo.GRADE12;
                _acessorio.GRADE13 = _novo.GRADE13;
                _acessorio.GRADE14 = _novo.GRADE14;
                _acessorio.GRADE_TOTAL = _novo.GRADE_TOTAL;
                _acessorio.DESENV_ACESSORIO_GRADE = _novo.DESENV_ACESSORIO_GRADE;
                _acessorio.ETI_COMPOSICAO = _novo.ETI_COMPOSICAO;
                _acessorio.ETI_BARRA = _novo.ETI_BARRA;
                _acessorio.TAG = _novo.TAG;
                _acessorio.AVIAMENTO = _novo.AVIAMENTO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCarrinhoAcessorioPorUsuario(int codigoUsuario)
        {
            var carrinho = ObterCarrinhoAcessorioPorUsuario(codigoUsuario);

            foreach (var c in carrinho)
                ExcluirCarrinhoAcessorio(c.CODIGO);
        }
        public void ExcluirCarrinhoAcessorio(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_ACESSORIO_CARRINHO _acessorio = ObterCarrinhoAcessorio(codigo);
                if (_acessorio != null)
                {
                    dbINTRA.DESENV_ACESSORIO_CARRINHOs.DeleteOnSubmit(_acessorio);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_ACESSORIO_CARRINHO

        /*DESENV_ACESSORIO_CARRINHO_LINX */
        public List<DESENV_ACESSORIO_CARRINHO_LINX> ObterCarrinhoLinxAcessorioPorUsuario(int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes where m.USUARIO == codigoUsuario orderby m.PRODUTO select m).ToList();
        }
        public DESENV_ACESSORIO_CARRINHO_LINX ObterCarrinhoLinxAcessorio(string pedido, string produto, string cor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes
                    where
                        m.PEDIDO == pedido
                        && m.PRODUTO == produto
                        && m.COR_PRODUTO == cor
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public DESENV_ACESSORIO_CARRINHO_LINX ObterCarrinhoLinxAcessorio(string produto, string cor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes
                    where
                        m.PRODUTO == produto
                        && m.COR_PRODUTO == cor
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public DESENV_ACESSORIO_CARRINHO_LINX ObterCarrinhoLinxAcessorio(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoLinxAcessorio(DESENV_ACESSORIO_CARRINHO_LINX _acessorio)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes.InsertOnSubmit(_acessorio);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoLinxAcessorioPorUsuario(int codigoUsuario)
        {
            var carrinho = ObterCarrinhoLinxAcessorioPorUsuario(codigoUsuario);

            foreach (var c in carrinho)
                ExcluirCarrinhoLinxAcessorio(c.PEDIDO, c.PRODUTO, c.COR_PRODUTO, codigoUsuario);
        }
        public void ExcluirCarrinhoLinxAcessorio(string pedido, string produto, string cor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_ACESSORIO_CARRINHO_LINX _acessorio = ObterCarrinhoLinxAcessorio(pedido, produto, cor, codigoUsuario);
                if (_acessorio != null)
                {
                    dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes.DeleteOnSubmit(_acessorio);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoLinxAcessorioPorCodigo(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_ACESSORIO_CARRINHO_LINX _acessorio = ObterCarrinhoLinxAcessorio(codigo);
                if (_acessorio != null)
                {
                    dbINTRA.DESENV_ACESSORIO_CARRINHO_LINXes.DeleteOnSubmit(_acessorio);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_ACESSORIO_CARRINHO_LINX


        public List<COMPRAS_MATERIAL_PREPEDIDO> ObterComprasMaterialPrePedido(string material, string cor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_MATERIAL_PREPEDIDOs
                    where m.MATERIAL == material
                    && m.COR_MATERIAL == cor
                    && m.PEDIDO_LINX == null
                    orderby m.MATERIAL
                    select m).ToList();
        }
        public COMPRAS_MATERIAL_PREPEDIDO ObterComprasMaterialPrePedido(string pedido, string material, string cor, int? codFicTec)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_MATERIAL_PREPEDIDOs
                    where m.PEDIDO == pedido
                    && m.MATERIAL == material
                    && m.COR_MATERIAL == cor
                    && m.DESENV_PRODUTO_FICTEC == ((codFicTec == null) ? 0 : codFicTec)
                    orderby m.MATERIAL
                    select m).SingleOrDefault();
        }
        public void AtualizarComprasMaterialPrePedido(COMPRAS_MATERIAL_PREPEDIDO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            COMPRAS_MATERIAL_PREPEDIDO compraMaterial = ObterComprasMaterialPrePedido(_novo.PEDIDO, _novo.MATERIAL, _novo.COR_MATERIAL, _novo.DESENV_PRODUTO_FICTEC);

            if (compraMaterial != null)
            {

                compraMaterial.PEDIDO = _novo.PEDIDO;
                compraMaterial.MATERIAL = _novo.MATERIAL;
                compraMaterial.COR_MATERIAL = _novo.COR_MATERIAL;
                compraMaterial.ENTREGA = _novo.ENTREGA;
                compraMaterial.LIMITE_ENTREGA = _novo.LIMITE_ENTREGA;
                compraMaterial.CUSTO = _novo.CUSTO;
                compraMaterial.DESCONTO_ITEM = _novo.DESCONTO_ITEM;
                compraMaterial.VALOR_ENTREGAR = _novo.VALOR_ENTREGAR;
                compraMaterial.IPI = _novo.IPI;
                compraMaterial.VALOR_ORIGINAL = _novo.VALOR_ORIGINAL;
                compraMaterial.QTDE_ORIGINAL = _novo.QTDE_ORIGINAL;
                compraMaterial.QTDE_ENTREGAR = _novo.QTDE_ENTREGAR;
                compraMaterial.PEDIDO_LINX = _novo.PEDIDO_LINX;
                compraMaterial.CUSTO_NOTA = _novo.CUSTO_NOTA;
                compraMaterial.QTDE_NOTA = _novo.QTDE_NOTA;
                compraMaterial.DESCONTO_ITEM_NOTA = _novo.DESCONTO_ITEM_NOTA;


                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public COMPRAS_PREPEDIDO ObterComprasPrePedido(string pedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_PREPEDIDOs
                    where m.PEDIDO == pedido
                    select m).FirstOrDefault();
        }
        public void InserirComprasPrePedido(COMPRAS_PREPEDIDO comprasPrepedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.COMPRAS_PREPEDIDOs.InsertOnSubmit(comprasPrepedido);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<COMPRAS_PRODUTO_PREPEDIDO> ObterComprasProdutoPrePedido(string pedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_PRODUTO_PREPEDIDOs
                    where m.PEDIDO == pedido
                    select m).ToList();
        }
        public COMPRAS_PRODUTO_PREPEDIDO ObterComprasProdutoPrePedido(string pedido, string produto, string cor, DateTime entrega)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_PRODUTO_PREPEDIDOs
                    where m.PEDIDO == pedido && m.PRODUTO == produto && m.COR_PRODUTO == cor && m.ENTREGA.Date == entrega.Date
                    select m).SingleOrDefault();
        }
        public void InserirComprasProdutoPrePedido(COMPRAS_PRODUTO_PREPEDIDO comprasProdutoPrepedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.COMPRAS_PRODUTO_PREPEDIDOs.InsertOnSubmit(comprasProdutoPrepedido);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarComprasProdutoPrePedido(COMPRAS_PRODUTO_PREPEDIDO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            COMPRAS_PRODUTO_PREPEDIDO produtoPrePedido = ObterComprasProdutoPrePedido(_novo.PEDIDO, _novo.PRODUTO, _novo.COR_PRODUTO, _novo.ENTREGA);

            if (produtoPrePedido != null)
            {

                produtoPrePedido.PEDIDO = _novo.PEDIDO;
                produtoPrePedido.PRODUTO = _novo.PRODUTO;
                produtoPrePedido.COR_PRODUTO = _novo.COR_PRODUTO;
                produtoPrePedido.ENTREGA = _novo.ENTREGA;
                produtoPrePedido.LIMITE_ENTREGA = _novo.LIMITE_ENTREGA;
                produtoPrePedido.CUSTO1 = _novo.CUSTO1;
                produtoPrePedido.CUSTO2 = _novo.CUSTO2;
                produtoPrePedido.CUSTO3 = _novo.CUSTO3;
                produtoPrePedido.CUSTO4 = _novo.CUSTO4;
                produtoPrePedido.VALOR_ENTREGAR = _novo.VALOR_ENTREGAR;
                produtoPrePedido.IPI = _novo.IPI;
                produtoPrePedido.VALOR_ORIGINAL = _novo.VALOR_ORIGINAL;
                produtoPrePedido.QTDE_ORIGINAL = _novo.QTDE_ORIGINAL;
                produtoPrePedido.QTDE_ENTREGAR = _novo.QTDE_ENTREGAR;
                produtoPrePedido.CO1 = _novo.CO1;
                produtoPrePedido.CO2 = _novo.CO2;
                produtoPrePedido.CO3 = _novo.CO3;
                produtoPrePedido.CO4 = _novo.CO4;
                produtoPrePedido.CO5 = _novo.CO5;
                produtoPrePedido.CO6 = _novo.CO6;
                produtoPrePedido.CO7 = _novo.CO7;
                produtoPrePedido.CO8 = _novo.CO8;
                produtoPrePedido.CO9 = _novo.CO9;
                produtoPrePedido.CO10 = _novo.CO10;
                produtoPrePedido.CO11 = _novo.CO11;
                produtoPrePedido.CO12 = _novo.CO12;
                produtoPrePedido.CO13 = _novo.CO13;
                produtoPrePedido.CO14 = _novo.CO14;
                produtoPrePedido.CE1 = _novo.CE1;
                produtoPrePedido.CE2 = _novo.CE2;
                produtoPrePedido.CE3 = _novo.CE3;
                produtoPrePedido.CE4 = _novo.CE4;
                produtoPrePedido.CE5 = _novo.CE5;
                produtoPrePedido.CE6 = _novo.CE6;
                produtoPrePedido.CE7 = _novo.CE7;
                produtoPrePedido.CE8 = _novo.CE8;
                produtoPrePedido.CE9 = _novo.CE9;
                produtoPrePedido.CE10 = _novo.CE10;
                produtoPrePedido.CE11 = _novo.CE11;
                produtoPrePedido.CE12 = _novo.CE12;
                produtoPrePedido.CE13 = _novo.CE13;
                produtoPrePedido.CE14 = _novo.CE14;
                produtoPrePedido.NOTA_FISCAL = _novo.NOTA_FISCAL;
                produtoPrePedido.PEDIDO_LINX = _novo.PEDIDO_LINX;
                produtoPrePedido.DATA_EXCLUSAO = _novo.DATA_EXCLUSAO;

                produtoPrePedido.DATA_BAIXA_ETI_COMP = _novo.DATA_BAIXA_ETI_COMP;
                produtoPrePedido.USUARIO_BAIXA_ETI_COMP = _novo.USUARIO_BAIXA_ETI_COMP;

                produtoPrePedido.DATA_BAIXA_TAG = _novo.DATA_BAIXA_TAG;
                produtoPrePedido.USUARIO_BAIXA_TAG = _novo.USUARIO_BAIXA_TAG;

                produtoPrePedido.DATA_BAIXA_ETI_BARRA = _novo.DATA_BAIXA_ETI_BARRA;
                produtoPrePedido.USUARIO_BAIXA_ETI_BARRA = _novo.USUARIO_BAIXA_ETI_BARRA;

                produtoPrePedido.DATA_BAIXA_AVIAMENTO = _novo.DATA_BAIXA_AVIAMENTO;
                produtoPrePedido.USUARIO_BAIXA_AVIAMENTO = _novo.USUARIO_BAIXA_AVIAMENTO;

                produtoPrePedido.OBS = _novo.OBS;

                produtoPrePedido.NF_ENTRADA = _novo.NF_ENTRADA;
                produtoPrePedido.RECEBIMENTO = _novo.RECEBIMENTO;
                produtoPrePedido.RECEBIDO_POR = _novo.RECEBIDO_POR;


                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirComprasProdutoPrePedido(string pedidoIntra, string produto, string cor, DateTime entrega)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                COMPRAS_PRODUTO_PREPEDIDO compraProdutoPrePedido = ObterComprasProdutoPrePedido(pedidoIntra, produto, cor, entrega);
                if (compraProdutoPrePedido != null)
                {
                    dbINTRA.COMPRAS_PRODUTO_PREPEDIDOs.DeleteOnSubmit(compraProdutoPrePedido);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<COMPRAS_PRODUTO_PREPEDIDO_PGTO> ObterComprasProdutoPrePedidoPagto(string pedido, string produto, string cor, DateTime entrega)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_PRODUTO_PREPEDIDO_PGTOs
                    where m.PEDIDO == pedido && m.PRODUTO == produto && m.COR_PRODUTO == cor && m.ENTREGA.Date == entrega.Date
                    orderby m.PARCELA
                    select m).ToList();
        }
        public COMPRAS_PRODUTO_PREPEDIDO_PGTO ObterComprasProdutoPrePedidoPagto(string pedido, string produto, string cor, DateTime entrega, int parcela)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.COMPRAS_PRODUTO_PREPEDIDO_PGTOs
                    where m.PEDIDO == pedido && m.PRODUTO == produto && m.COR_PRODUTO == cor && m.ENTREGA.Date == entrega.Date && m.PARCELA == parcela
                    select m).SingleOrDefault();
        }
        public void InserirComprasProdutoPrePedidoPagto(COMPRAS_PRODUTO_PREPEDIDO_PGTO comprasProdutoPrepedidoPgto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.COMPRAS_PRODUTO_PREPEDIDO_PGTOs.InsertOnSubmit(comprasProdutoPrepedidoPgto);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarComprasProdutoPrePedidoPagto(COMPRAS_PRODUTO_PREPEDIDO_PGTO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            COMPRAS_PRODUTO_PREPEDIDO_PGTO comprasProdutoPrepedidoPgto = ObterComprasProdutoPrePedidoPagto(_novo.PEDIDO, _novo.PRODUTO, _novo.COR_PRODUTO, _novo.ENTREGA, _novo.PARCELA);

            if (comprasProdutoPrepedidoPgto != null)
            {

                comprasProdutoPrepedidoPgto.PEDIDO = _novo.PEDIDO;
                comprasProdutoPrepedidoPgto.PRODUTO = _novo.PRODUTO;
                comprasProdutoPrepedidoPgto.COR_PRODUTO = _novo.COR_PRODUTO;
                comprasProdutoPrepedidoPgto.ENTREGA = _novo.ENTREGA;
                comprasProdutoPrepedidoPgto.PARCELA = _novo.PARCELA;
                comprasProdutoPrepedidoPgto.VALOR = _novo.VALOR;
                comprasProdutoPrepedidoPgto.PORC = _novo.PORC;
                comprasProdutoPrepedidoPgto.DATA_PAGAMENTO = _novo.DATA_PAGAMENTO;
                comprasProdutoPrepedidoPgto.DATA_BAIXA = _novo.DATA_BAIXA;
                comprasProdutoPrepedidoPgto.OBS = _novo.OBS;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirComprasProdutoPrePedidoPagto(string pedidoIntra, string produto, string cor, DateTime entrega, int parcela)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                COMPRAS_PRODUTO_PREPEDIDO_PGTO compraProdutoPrePedido = ObterComprasProdutoPrePedidoPagto(pedidoIntra, produto, cor, entrega, parcela);
                if (compraProdutoPrePedido != null)
                {
                    dbINTRA.COMPRAS_PRODUTO_PREPEDIDO_PGTOs.DeleteOnSubmit(compraProdutoPrePedido);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<DESENV_ACESSORIO_GRADE> ObterGradeAcessorio(string grade)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_GRADEs where m.GRADE == grade orderby m.NOME select m).ToList();
        }
        public DESENV_ACESSORIO_GRADE ObterGradeAcessorio(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_GRADEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<DESENV_ACESSORIO_GRADE> ObterGradeAcessorio()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_GRADEs orderby m.GRADE, m.NOME select m).ToList();
        }
        public void InserirGradeAcessorio(DESENV_ACESSORIO_GRADE _grade)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_ACESSORIO_GRADEs.InsertOnSubmit(_grade);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarGradeAcessorio(DESENV_ACESSORIO_GRADE _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_ACESSORIO_GRADE _grade = ObterGradeAcessorio(_novo.CODIGO);

            if (_grade != null)
            {
                _grade.CODIGO = _novo.CODIGO;
                _grade.GRADE = _novo.GRADE;
                _grade.NOME = _novo.NOME;
                _grade.GRADE1 = _novo.GRADE1;
                _grade.GRADE2 = _novo.GRADE2;
                _grade.GRADE3 = _novo.GRADE3;
                _grade.GRADE4 = _novo.GRADE4;
                _grade.GRADE5 = _novo.GRADE5;
                _grade.GRADE6 = _novo.GRADE6;
                _grade.GRADE7 = _novo.GRADE7;
                _grade.GRADE8 = _novo.GRADE8;
                _grade.GRADE9 = _novo.GRADE9;
                _grade.GRADE10 = _novo.GRADE10;
                _grade.GRADE11 = _novo.GRADE11;
                _grade.GRADE12 = _novo.GRADE12;
                _grade.GRADE13 = _novo.GRADE13;
                _grade.GRADE14 = _novo.GRADE14;


                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirGradeAcessorio(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_ACESSORIO_GRADE _gradeAcessorio = ObterGradeAcessorio(codigo);
                if (_gradeAcessorio != null)
                {
                    dbINTRA.DESENV_ACESSORIO_GRADEs.DeleteOnSubmit(_gradeAcessorio);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*DESENV_ACESSORIO_PEDIDO */
        public List<DESENV_ACESSORIO_PEDIDO> ObterAcessorioPedido()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIO_PEDIDOs
                    orderby m.PEDIDO
                    select m).ToList();
        }
        public List<DESENV_ACESSORIO_PEDIDO> ObterAcessorioPedido(string desenv_acessorio)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIO_PEDIDOs where m.DESENV_ACESSORIO.ToString() == desenv_acessorio select m).ToList();
        }
        public DESENV_ACESSORIO_PEDIDO ObterAcessorioPedido(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_ACESSORIO_PEDIDOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirAcessorioPedido(DESENV_ACESSORIO_PEDIDO _acessorioPed)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_ACESSORIO_PEDIDOs.InsertOnSubmit(_acessorioPed);
                db.SubmitChanges();

                return _acessorioPed.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAcessorioPedido(DESENV_ACESSORIO_PEDIDO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_ACESSORIO_PEDIDO _acessorioPed = ObterAcessorioPedido(_novo.CODIGO);

            if (_acessorioPed != null)
            {
                _acessorioPed.CODIGO = _novo.CODIGO;
                _acessorioPed.DESENV_ACESSORIO = _novo.DESENV_ACESSORIO;
                _acessorioPed.PRECO = _novo.PRECO;
                _acessorioPed.PEDIDO = _novo.PEDIDO;
                _acessorioPed.QTDE_ORIGINAL = _novo.QTDE_ORIGINAL;
                _acessorioPed.QTDE_O1 = _novo.QTDE_O1;
                _acessorioPed.QTDE_O2 = _novo.QTDE_O2;
                _acessorioPed.QTDE_O3 = _novo.QTDE_O3;
                _acessorioPed.QTDE_O4 = _novo.QTDE_O4;
                _acessorioPed.QTDE_O5 = _novo.QTDE_O5;
                _acessorioPed.QTDE_O6 = _novo.QTDE_O6;
                _acessorioPed.QTDE_O7 = _novo.QTDE_O7;
                _acessorioPed.QTDE_O8 = _novo.QTDE_O8;
                _acessorioPed.QTDE_O9 = _novo.QTDE_O9;
                _acessorioPed.QTDE_O10 = _novo.QTDE_O10;
                _acessorioPed.DATA_PEDIDO = _novo.DATA_PEDIDO;
                _acessorioPed.USUARIO_PEDIDO = _novo.USUARIO_PEDIDO;
                _acessorioPed.DATA_PREV_ENTREGA = _novo.DATA_PREV_ENTREGA;

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
        public void ExcluirAcessorioPedido(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_ACESSORIO_PEDIDO _acessorioPed = ObterAcessorioPedido(codigo);
                if (_acessorioPed != null)
                {
                    db.DESENV_ACESSORIO_PEDIDOs.DeleteOnSubmit(_acessorioPed);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_ACESSORIO
        public List<SP_OBTER_DESENV_ACESS_SEM_PEDIDOResult> ObterDesenvAcessSemPedido(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_ACESS_SEM_PEDIDO(colecao) select i).ToList();
        }
        public List<SP_OBTER_DESENV_ACESS_COM_PEDIDOResult> ObterDesenvAcessComPedido(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_ACESS_COM_PEDIDO(colecao) select i).ToList();
        }

        //SP_OBTER_DESENV_COLECAO_ACESSResult
        public List<SP_OBTER_DESENV_COLECAO_ACESSResult> ObterDesenvolvimentoColecaoAcessorio(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_COLECAO_ACESS(colecao) select i).ToList();
        }
        //FIM //SP_OBTER_DESENV_COLECAO_ACESSResult


        public SP_GERAR_PEDIDO_COMPRA_ACESSORIOResult GerarLINXPedidoCompraAcessorio(string fornecedor, DateTime limiteEntrega, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.SP_GERAR_PEDIDO_COMPRA_ACESSORIO(fornecedor, limiteEntrega, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }
        public SP_GERAR_PEDIDO_COMPRA_ACESSORIO_PREPEDIDOResult GerarLINXPedidoCompraAcessorioPorPrePedido(string fornecedor, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_GERAR_PEDIDO_COMPRA_ACESSORIO_PREPEDIDO(fornecedor, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }
        public SP_GERAR_PREPEDIDO_COMPRA_ACESSORIOResult GerarINTRAPrePedidoCompraAcessorio(string fornecedor, DateTime limiteEntrega, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_GERAR_PREPEDIDO_COMPRA_ACESSORIO(fornecedor, limiteEntrega, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }

        //SP_OBTER_DESENV_REL_ACESSResult
        public List<SP_OBTER_DESENV_REL_ACESSResult> ObterRelColecaoAcessorio(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_REL_ACESS(colecao) select i).ToList();
        }
        //FIM //SP_OBTER_DESENV_REL_ACESSResult

        public List<SP_OBTER_DESENV_REL_ACESSRESUMOResult> ObterRelColecaoAcessorioResumo(string colecao, string status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_DESENV_REL_ACESSRESUMO(colecao, status) select i).ToList();
        }

        public List<SP_OBTER_CONTROLE_MOSTRUARIOResult> ObterControleMostruario(string colecao, int? desenvOrigem, string grupoProduto, string produto, string nome, string fabricante, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return (from i in db.SP_OBTER_CONTROLE_MOSTRUARIO(colecao, desenvOrigem, grupoProduto, produto, nome, fabricante, griffe) select i).ToList();
        }

        public List<SP_OBTER_CONTROLE_PRODUCAOResult> ObterControleProducao(string colecao, int? desenvOrigem, string grupoProduto, string produto, string nome, string fabricante, string griffe, string corFornecedor, string tecido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            var p = (from i in db.SP_OBTER_CONTROLE_PRODUCAO(colecao, desenvOrigem, grupoProduto, produto, nome, fabricante, griffe, corFornecedor, tecido) select i).ToList();

            db.Dispose();

            return p;
        }

        public List<SP_OBTER_PRODUTO_ACABADOResult> ObterProdutoAcabado(string colecao, int? desenvOrigem, string grupoProduto, string griffe, string produto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_PRODUTO_ACABADO(colecao, desenvOrigem, grupoProduto, griffe, produto, mostruario) select i).ToList();
        }

        public List<SP_OBTER_PRODUTO_ACABADO_ACESSORIOResult> ObterProdutoAcabadoAcessorio(string colecao, int? desenvOrigem, string grupoProduto, string produto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_ACABADO_ACESSORIO(colecao, desenvOrigem, grupoProduto, produto) select i).ToList();
        }
        public List<SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PEDResult> ObterProdutoAcabadoAcessorioPed(string colecao, int? desenvOrigem, string grupoProduto, string produto, string filial, string fornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PED(colecao, desenvOrigem, grupoProduto, produto, filial, fornecedor) select i).ToList();
        }

        public List<SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDOResult> ObterProdutoAcabadoAcessorioPrePedido(string colecao, int? desenvOrigem, string grupoProduto, string produto, string filial, string fornecedor)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PRODUTO_ACABADO_ACESSORIO_PREPEDIDO(colecao, desenvOrigem, grupoProduto, produto, filial, fornecedor) select i).ToList();
        }

        public SP_GERAR_PEDIDO_COMPRA_PRODUTOACABADOResult GerarPedidoCompraProdutoAcabado(string produto, string corProduto, string fornecedor,
                                                                            DateTime limiteEntrega, string filial, string codigoFilialRateio,
                                                                            decimal custoProduto, char status, string aprovadoPor,
                                                                            int exp, int xp, int pp, int p, int m, int g, int gg)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.SP_GERAR_PEDIDO_COMPRA_PRODUTOACABADO(produto, corProduto, fornecedor, limiteEntrega, filial, codigoFilialRateio, custoProduto, status, aprovadoPor, exp, xp, pp, p, m, g, gg) select i).SingleOrDefault();
        }


        public SP_OBTER_PRODUTO_COMPRAResult ObterProdutoAcabado(string produto, string cor, string pedido)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PRODUTO_COMPRA(produto, cor, pedido) select i).SingleOrDefault();
        }

        public List<SP_OBTER_PRODUTO_COMPRA_INTRANETResult> ObterProdutoAcabadoIntranet(string pedidoIntra, string produto, string cor, DateTime entrega, char mostruario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_COMPRA_INTRANET(pedidoIntra, produto, cor, entrega, mostruario) select i).ToList();
        }
        public SP_OBTER_PRODUTO_COMPRA_INTRANET_TAMResult ObterProdutoAcabadoIntranetTamanho(string pedidoIntra, string produto, string cor, DateTime entrega)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_COMPRA_INTRANET_TAM(pedidoIntra, produto, cor, entrega) select i).SingleOrDefault();
        }

        public bool VerificarProdutoComprasLinxEntregue(string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            var compras = (from m in db.COMPRAS_PRODUTOs
                           where
                               m.PRODUTO == produto
                               && m.COR_PRODUTO == cor
                               && m.QTDE_ENTREGUE > 0
                           select m).FirstOrDefault();

            if (compras == null)
                return false;

            return true;
        }

        public SP_ATUALIZAR_PEDIDO_COMPRAResult AtualizarPedidoCompraProdutoAcabado(string pedidoAtacado, string pedidoVarejo, string produto, string cor, int grade1, int grade2, int grade3, int grade4, int grade5, int grade6, int grade7)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_ATUALIZAR_PEDIDO_COMPRA(pedidoAtacado, pedidoVarejo, produto, cor, grade1, grade2, grade3, grade4, grade5, grade6, grade7) select i).SingleOrDefault();
        }


        /* DESENV_PRODUTO_PEDLINX */
        public List<DESENV_PRODUTO_PEDLINX> ObterProdutoPedidoLinx()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDLINXes
                    orderby m.PEDIDO
                    select m).ToList();
        }
        public List<DESENV_PRODUTO_PEDLINX> ObterProdutoPedidoLinx(string desenvProduto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDLINXes where m.DESENV_PRODUTO.ToString() == desenvProduto && m.MOSTRUARIO == mostruario select m).ToList();
        }
        public DESENV_PRODUTO_PEDLINX ObterProdutoPedidoLinx(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDLINXes
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirProdutoPedidoLinx(DESENV_PRODUTO_PEDLINX produtoPedidoLinx)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_PEDLINXes.InsertOnSubmit(produtoPedidoLinx);
                db.SubmitChanges();

                return produtoPedidoLinx.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoPedidoLinx(DESENV_PRODUTO_PEDLINX _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_PEDLINX produtoPedidoLinx = ObterProdutoPedidoLinx(_novo.CODIGO);

            if (produtoPedidoLinx != null)
            {
                produtoPedidoLinx.CODIGO = _novo.CODIGO;
                produtoPedidoLinx.DESENV_PRODUTO = _novo.DESENV_PRODUTO;
                produtoPedidoLinx.FILIAL = _novo.FILIAL;
                produtoPedidoLinx.PEDIDO = _novo.PEDIDO;
                produtoPedidoLinx.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                produtoPedidoLinx.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirProdutoPedidoLinx(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_PEDLINX produtoPedidoLinx = ObterProdutoPedidoLinx(codigo);
                if (produtoPedidoLinx != null)
                {
                    db.DESENV_PRODUTO_PEDLINXes.DeleteOnSubmit(produtoPedidoLinx);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DESENV_ACESSORIO_PEDLINX> ObterAcessorioPedidoLinx(string desenvAcessorio)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_ACESSORIO_PEDLINXes where m.DESENV_ACESSORIO.ToString() == desenvAcessorio select m).ToList();
        }
        public int InserirAcessorioPedidoLinx(DESENV_ACESSORIO_PEDLINX produtoPedidoLinx)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_ACESSORIO_PEDLINXes.InsertOnSubmit(produtoPedidoLinx);
                dbINTRA.SubmitChanges();

                return produtoPedidoLinx.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* DESENV_PRODUTO_PEDLINX_ENTRADA */
        public List<DESENV_PRODUTO_PEDLINX_ENTRADA> ObterProdutoPedidoLinxEntrada()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDLINX_ENTRADAs
                    select m).ToList();
        }
        public List<DESENV_PRODUTO_PEDLINX_ENTRADA> ObterProdutoPedidoLinxEntrada(string desenvProdutoPedLinx)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDLINX_ENTRADAs where m.DESENV_PRODUTO_PEDLINX.ToString() == desenvProdutoPedLinx select m).ToList();
        }
        public DESENV_PRODUTO_PEDLINX_ENTRADA ObterProdutoPedidoLinxEntrada(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PRODUTO_PEDLINX_ENTRADAs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirProdutoPedidoLinxEntrada(DESENV_PRODUTO_PEDLINX_ENTRADA produtoPedidoLinxEntrada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PRODUTO_PEDLINX_ENTRADAs.InsertOnSubmit(produtoPedidoLinxEntrada);
                db.SubmitChanges();

                return produtoPedidoLinxEntrada.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoPedidoLinxEntrada(DESENV_PRODUTO_PEDLINX_ENTRADA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_PEDLINX_ENTRADA produtoPedidoLinxEntrada = ObterProdutoPedidoLinxEntrada(_novo.CODIGO);

            if (produtoPedidoLinxEntrada != null)
            {
                produtoPedidoLinxEntrada.CODIGO = _novo.CODIGO;
                produtoPedidoLinxEntrada.DESENV_PRODUTO_PEDLINX = _novo.DESENV_PRODUTO_PEDLINX;
                produtoPedidoLinxEntrada.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                produtoPedidoLinxEntrada.NF_ENTRADA = _novo.NF_ENTRADA;
                produtoPedidoLinxEntrada.SERIE_NF = _novo.SERIE_NF;
                produtoPedidoLinxEntrada.EMISSAO = _novo.EMISSAO;
                produtoPedidoLinxEntrada.RECEBIMENTO = _novo.RECEBIMENTO;
                produtoPedidoLinxEntrada.USUARIO_RECEBIMENTO = _novo.USUARIO_RECEBIMENTO;
                produtoPedidoLinxEntrada.GRADE_EXP = _novo.GRADE_EXP;
                produtoPedidoLinxEntrada.GRADE_XP = _novo.GRADE_XP;
                produtoPedidoLinxEntrada.GRADE_PP = _novo.GRADE_PP;
                produtoPedidoLinxEntrada.GRADE_P = _novo.GRADE_P;
                produtoPedidoLinxEntrada.GRADE_M = _novo.GRADE_M;
                produtoPedidoLinxEntrada.GRADE_G = _novo.GRADE_G;
                produtoPedidoLinxEntrada.GRADE_GG = _novo.GRADE_GG;
                produtoPedidoLinxEntrada.GRADE_TOTAL = _novo.GRADE_TOTAL;
                produtoPedidoLinxEntrada.STATUS = _novo.STATUS;
                produtoPedidoLinxEntrada.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                produtoPedidoLinxEntrada.QTDE_NOTA = _novo.QTDE_NOTA;
                produtoPedidoLinxEntrada.OBSERVACAO = _novo.OBSERVACAO;

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
        public void ExcluirProdutoPedidoLinxEntrada(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_PEDLINX_ENTRADA produtoPedidoLinxEntrada = ObterProdutoPedidoLinxEntrada(codigo);
                if (produtoPedidoLinxEntrada != null)
                {
                    db.DESENV_PRODUTO_PEDLINX_ENTRADAs.DeleteOnSubmit(produtoPedidoLinxEntrada);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_FABRICANTEResult> ObterFabricante()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.SP_OBTER_FABRICANTE() select i).ToList();
        }

        public List<SP_OBTER_PRODUTO_ACABADO_ENTRADAResult> ObterProdutoAcabadoEntrada(string colecao, string produto, string nome)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.SP_OBTER_PRODUTO_ACABADO_ENTRADA(colecao, produto, nome) select i).ToList();
        }

        public List<SP_OBTER_MATERIALResult> ObterMaterialPesquisa(string pesquisa)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_MATERIAL("", "", pesquisa) select i).ToList();
        }
        public SP_OBTER_MATERIALResult ObterMaterialPesquisa(string material, string corMaterial)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_MATERIAL(material, corMaterial, "") select i).SingleOrDefault();
        }

        public List<DESENV_GRUPO_PRODUTO> ObterGrupoProdutoPrePedido()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_GRUPO_PRODUTOs
                    orderby m.GRUPO
                    select m).ToList();
        }
        public DESENV_GRUPO_PRODUTO ObterGrupoProdutoPrePedidoDePara(string grupoPrePedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_GRUPO_PRODUTOs
                    where m.GRUPO == grupoPrePedido
                    select m).SingleOrDefault();
        }
        public DESENV_GRUPO_PRODUTO ObterGrupoProdutoPrePedidoParaDe(string grupoProdutoLinx)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_GRUPO_PRODUTOs
                    where m.GRUPO_PRODUTO == grupoProdutoLinx
                    select m).SingleOrDefault();
        }

        public List<SP_OBTER_PREPEDIDOResult> ObterPrePedidoMarca(string marca, string colecao, string griffe)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PREPEDIDO(marca, colecao, griffe) select i).ToList();
        }
        public List<SP_OBTER_PREPEDIDO_RESUMOResult> ObterPrePedidoResumo(string marca, string colecao, string griffe, int desenvOrigem)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PREPEDIDO_RESUMO(marca, colecao, griffe, desenvOrigem) select i).ToList();
        }
        public List<SP_OBTER_PREPEDIDO_RESUMO_COTAResult> ObterPrePedidoResumoCota(string marca, string colecao, string griffe, int desenvOrigem)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PREPEDIDO_RESUMO_COTA(marca, colecao, griffe, desenvOrigem) select i).ToList();
        }

        public List<SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTOResult> ObterPrePedidoLiberacaoProduto(string marca, string colecao, int desenvOrigem, string griffe, string descCor)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PREPEDIDO_LIBERACAO_PRODUTO(marca, colecao, desenvOrigem, griffe, descCor) select i).ToList();
        }

        public DESENV_PREPEDIDO ObterPrePedido(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_PREPEDIDO ObterPrePedido(string colecao, string produto, string cor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDOs
                    where
                        m.COLECAO == colecao
                        && m.PRODUTO == produto
                        && m.COR == cor
                    select m).SingleOrDefault();
        }
        public List<DESENV_PREPEDIDO> ObterPrePedidoPorColecao(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDOs
                    where
                        m.COLECAO == colecao
                    select m).ToList();
        }
        public int InserirPrePedido(DESENV_PREPEDIDO prePedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PREPEDIDOs.InsertOnSubmit(prePedido);
                dbINTRA.SubmitChanges();

                return prePedido.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPrePedido(DESENV_PREPEDIDO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PREPEDIDO prePedido = ObterPrePedido(_novo.CODIGO);

            if (prePedido != null)
            {
                prePedido.CODIGO = _novo.CODIGO;
                prePedido.COLECAO = _novo.COLECAO;
                prePedido.MATERIAL = _novo.MATERIAL;
                prePedido.TECIDO = _novo.TECIDO;
                prePedido.FORNECEDOR = _novo.FORNECEDOR;
                prePedido.COR_FORNECEDOR = _novo.COR_FORNECEDOR;
                prePedido.COR = _novo.COR;
                prePedido.GRUPO_PRODUTO = _novo.GRUPO_PRODUTO;
                prePedido.GRIFFE = _novo.GRIFFE;
                prePedido.UNIDADE_MEDIDA = _novo.UNIDADE_MEDIDA;
                prePedido.CONSUMO = _novo.CONSUMO;
                prePedido.PRECO_TECIDO = _novo.PRECO_TECIDO;
                prePedido.QTDE_ATACADO = _novo.QTDE_ATACADO;
                prePedido.QTDE_VAREJO = _novo.QTDE_VAREJO;
                prePedido.PRECO_VENDA = _novo.PRECO_VENDA;
                prePedido.CONSUMO_TOTAL = _novo.CONSUMO_TOTAL;
                prePedido.VALOR_TOTAL_VAREJO = _novo.VALOR_TOTAL_VAREJO;
                prePedido.VALOR_TOTAL_ATACADO = _novo.VALOR_TOTAL_ATACADO;
                prePedido.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                prePedido.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

                prePedido.DATA_CRIACAO = _novo.DATA_CRIACAO;
                prePedido.USUARIO_CRIACAO = _novo.USUARIO_CRIACAO;
                prePedido.PRODUTO = _novo.PRODUTO;

                prePedido.SIGNED_NOME = _novo.SIGNED_NOME;
                prePedido.REF_MODELAGEM = _novo.REF_MODELAGEM;
                prePedido.DESENV_PRODUTO_ORIGEM = _novo.DESENV_PRODUTO_ORIGEM;

                prePedido.MARCA = _novo.MARCA;
                prePedido.DATA_APROVACAO_TECIDO = _novo.DATA_APROVACAO_TECIDO;
                prePedido.PROD_PARCIAL = _novo.PROD_PARCIAL;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirPrePedido(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PREPEDIDO prePedido = ObterPrePedido(codigo);
                if (prePedido != null)
                {
                    dbINTRA.DESENV_PREPEDIDOs.DeleteOnSubmit(prePedido);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DESENV_PREPEDIDO_DEF ObterPrePedidoDEF(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDO_DEFs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirPrePedidoDEF(DESENV_PREPEDIDO_DEF prePedidoDEF)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PREPEDIDO_DEFs.InsertOnSubmit(prePedidoDEF);
                dbINTRA.SubmitChanges();

                return prePedidoDEF.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPrePedidoDEF(DESENV_PREPEDIDO_DEF _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PREPEDIDO_DEF prePedidoDEF = ObterPrePedidoDEF(_novo.CODIGO);

            if (prePedidoDEF != null)
            {
                prePedidoDEF.CODIGO = _novo.CODIGO;
                prePedidoDEF.DESENV_PREPEDIDO = _novo.DESENV_PREPEDIDO;
                prePedidoDEF.QTDE_VAREJO = _novo.QTDE_VAREJO;
                prePedidoDEF.QTDE_ATACADO = _novo.QTDE_ATACADO;
                prePedidoDEF.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                prePedidoDEF.DATA_ALTERACAO = _novo.DATA_ALTERACAO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public DESENV_PREPEDIDO_SKU ObterPrePedidoSKU(string marca, string colecao, string grupoProduto, string griffe, int desenvProdutoOrigem)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDO_SKUs
                    where
                        m.COLECAO == colecao
                        && m.MARCA == marca
                        && m.GRUPO_PRODUTO == grupoProduto
                        && m.GRIFFE == griffe
                        && m.DESENV_PRODUTO_ORIGEM == desenvProdutoOrigem
                    select m).SingleOrDefault();
        }
        public void InserirPrePedidoSKU(DESENV_PREPEDIDO_SKU prePedidoSKU)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PREPEDIDO_SKUs.InsertOnSubmit(prePedidoSKU);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPrePedidoSKU(DESENV_PREPEDIDO_SKU _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PREPEDIDO_SKU prePedidoSKU = ObterPrePedidoSKU(_novo.MARCA, _novo.COLECAO, _novo.GRUPO_PRODUTO, _novo.GRIFFE, _novo.DESENV_PRODUTO_ORIGEM);

            if (prePedidoSKU != null)
            {
                prePedidoSKU.MARCA = _novo.MARCA;
                prePedidoSKU.COLECAO = _novo.COLECAO;
                prePedidoSKU.GRUPO_PRODUTO = _novo.GRUPO_PRODUTO;
                prePedidoSKU.GRIFFE = _novo.GRIFFE;
                prePedidoSKU.SKU_PLAN_ATACADO = _novo.SKU_PLAN_ATACADO;
                prePedidoSKU.SKU_PLAN_VAREJO = _novo.SKU_PLAN_VAREJO;
                prePedidoSKU.DESENV_PRODUTO_ORIGEM = _novo.DESENV_PRODUTO_ORIGEM;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirPrePedidoSKU(string marca, string colecao, string grupoProduto, string griffe, int desenvProdutoOrigem)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PREPEDIDO_SKU prePedidoSKU = ObterPrePedidoSKU(marca, colecao, grupoProduto, griffe, desenvProdutoOrigem);
                if (prePedidoSKU != null)
                {
                    dbINTRA.DESENV_PREPEDIDO_SKUs.DeleteOnSubmit(prePedidoSKU);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DESENV_PREPEDIDO_TECIDO> ObterPrePedidoTecido()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDO_TECIDOs
                    select m).ToList();
        }
        public List<DESENV_PREPEDIDO_TECIDO> ObterPrePedidoTecido(string marca, string material, string tecido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDO_TECIDOs
                    where
                        m.MATERIAL == material
                        && m.MARCA == marca
                        && m.TECIDO == tecido
                    select m).ToList();
        }
        public List<DESENV_PREPEDIDO_TECIDO> ObterPrePedidoTecido(string marca, string material, string tecido, string corLinx)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDO_TECIDOs
                    where
                        m.MATERIAL == material
                        && m.MARCA == marca
                        && m.TECIDO == tecido
                        && m.COR_LINX == corLinx
                    select m).ToList();
        }
        public DESENV_PREPEDIDO_TECIDO ObterPrePedidoTecido(string marca, string material, string tecido, string corLinx, string corFornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PREPEDIDO_TECIDOs
                    where
                        m.MATERIAL == material
                        && m.MARCA == marca
                        && m.TECIDO == tecido
                        && m.COR_LINX == corLinx
                        && m.COR_FORNECEDOR == corFornecedor
                    select m).SingleOrDefault();
        }
        public void InserirPrePedidoTecido(DESENV_PREPEDIDO_TECIDO prePedidoTecido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PREPEDIDO_TECIDOs.InsertOnSubmit(prePedidoTecido);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPrePedidoTecido(DESENV_PREPEDIDO_TECIDO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PREPEDIDO_TECIDO prePedidoTecido = ObterPrePedidoTecido(_novo.MARCA, _novo.MATERIAL, _novo.TECIDO, _novo.COR_LINX, _novo.COR_FORNECEDOR);

            if (prePedidoTecido != null)
            {
                prePedidoTecido.MARCA = _novo.MARCA;
                prePedidoTecido.MATERIAL = _novo.MATERIAL;
                prePedidoTecido.TECIDO = _novo.TECIDO;
                prePedidoTecido.COR_LINX = _novo.COR_LINX;
                prePedidoTecido.COR_FORNECEDOR = _novo.COR_FORNECEDOR;
                prePedidoTecido.QTDE_COMPRA = _novo.QTDE_COMPRA;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirPrePedidoTecido(string marca, string material, string tecido, string corLinx, string corFornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PREPEDIDO_TECIDO prePedidoTecido = ObterPrePedidoTecido(marca, material, tecido, corLinx, corFornecedor);
                if (prePedidoTecido != null)
                {
                    dbINTRA.DESENV_PREPEDIDO_TECIDOs.DeleteOnSubmit(prePedidoTecido);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_PREPEDIDO_TECIDOResult> ObterPrePedidoTecidoConsumo(string marca, string colecao, int desenvProdutoOrigem, string material, string tecido, string cor, string corFornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PREPEDIDO_TECIDO(marca, colecao, desenvProdutoOrigem, material, tecido, cor, corFornecedor) select i).ToList();
        }


        public List<SP_OBTER_VENDA_CALCADOSResult> ObterVendaCalcados(string colecao, string grupoProduto, string produto, string fornecedor, DateTime? dataIni, DateTime? dataFim)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_VENDA_CALCADOS(colecao, grupoProduto, produto, fornecedor, dataIni, dataFim) select i).ToList();
        }
        public List<SP_OBTER_VENDA_CALCADOS_PORMESResult> ObterVendaCalcadosPorMes(string produto, string cor, DateTime? dataIni, DateTime? dataFim)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_VENDA_CALCADOS_PORMES(produto, cor, dataIni, dataFim) select i).ToList();
        }


        public List<SP_OBTER_PRODUTO_ACABADO_PRODUTOV2Result> ObterProdutoAcabadoProdutoV2(string colecao, int? desenvOrigem, string grupoProduto, string produto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_ACABADO_PRODUTOV2(colecao, desenvOrigem, grupoProduto, produto) select i).ToList();
        }

        /*DESENV_PRODUTO_CARRINHOV2 */
        public List<DESENV_PRODUTO_CARRINHOV2> ObterCarrinhoProdutoPorUsuario(int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHOV2s where m.USUARIO == codigoUsuario orderby m.DATA_INCLUSAO select m).ToList();
        }
        public DESENV_PRODUTO_CARRINHOV2 ObterCarrinhoProduto(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHOV2s
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_PRODUTO_CARRINHOV2 ObterCarrinhoProduto(int codigoProduto, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHOV2s
                    where
                        m.DESENV_PRODUTO == codigoProduto
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoProduto(DESENV_PRODUTO_CARRINHOV2 _produto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PRODUTO_CARRINHOV2s.InsertOnSubmit(_produto);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoProduto(DESENV_PRODUTO_CARRINHOV2 _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_CARRINHOV2 _produto = ObterCarrinhoProduto(_novo.CODIGO);

            if (_produto != null)
            {
                _produto.CODIGO = _novo.CODIGO;
                _produto.DESENV_PRODUTO = _novo.DESENV_PRODUTO;
                _produto.GRADE1 = _novo.GRADE1;
                _produto.GRADE2 = _novo.GRADE2;
                _produto.GRADE3 = _novo.GRADE3;
                _produto.GRADE4 = _novo.GRADE4;
                _produto.GRADE5 = _novo.GRADE5;
                _produto.GRADE6 = _novo.GRADE6;
                _produto.GRADE7 = _novo.GRADE7;
                _produto.GRADE8 = _novo.GRADE8;
                _produto.GRADE9 = _novo.GRADE9;
                _produto.GRADE10 = _novo.GRADE10;
                _produto.GRADE11 = _novo.GRADE11;
                _produto.GRADE12 = _novo.GRADE12;
                _produto.GRADE13 = _novo.GRADE13;
                _produto.GRADE14 = _novo.GRADE14;
                _produto.GRADE_TOTAL = _novo.GRADE_TOTAL;
                _produto.DESENV_PRODUTO_GRADE = _novo.DESENV_PRODUTO_GRADE;
                _produto.ETI_COMPOSICAO = _novo.ETI_COMPOSICAO;
                _produto.ETI_BARRA = _novo.ETI_BARRA;
                _produto.TAG = _novo.TAG;
                _produto.AVIAMENTO = _novo.AVIAMENTO;
                _produto.OBS = _novo.OBS;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCarrinhoProdutoPorUsuario(int codigoUsuario)
        {
            var carrinho = ObterCarrinhoProdutoPorUsuario(codigoUsuario);

            foreach (var c in carrinho)
                ExcluirCarrinhoProduto(c.CODIGO);
        }
        public void ExcluirCarrinhoProduto(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_CARRINHOV2 _produto = ObterCarrinhoProduto(codigo);
                if (_produto != null)
                {
                    dbINTRA.DESENV_PRODUTO_CARRINHOV2s.DeleteOnSubmit(_produto);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_PRODUTO_CARRINHOV2

        /*DESENV_PRODUTO_CARRINHO_LINX */
        public List<DESENV_PRODUTO_CARRINHO_LINX> ObterCarrinhoLinxProdutoPorUsuario(int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes where m.USUARIO == codigoUsuario orderby m.PRODUTO select m).ToList();
        }
        public DESENV_PRODUTO_CARRINHO_LINX ObterCarrinhoLinxProduto(string pedido, string produto, string cor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes
                    where
                        m.PEDIDO == pedido
                        && m.PRODUTO == produto
                        && m.COR_PRODUTO == cor
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public DESENV_PRODUTO_CARRINHO_LINX ObterCarrinhoLinxProduto(string produto, string cor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes
                    where
                        m.PRODUTO == produto
                        && m.COR_PRODUTO == cor
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public DESENV_PRODUTO_CARRINHO_LINX ObterCarrinhoLinxProduto(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoLinxProduto(DESENV_PRODUTO_CARRINHO_LINX _produto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes.InsertOnSubmit(_produto);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoLinxProdutoPorUsuario(int codigoUsuario)
        {
            var carrinho = ObterCarrinhoLinxProdutoPorUsuario(codigoUsuario);

            foreach (var c in carrinho)
                ExcluirCarrinhoLinxProduto(c.PEDIDO, c.PRODUTO, c.COR_PRODUTO, codigoUsuario);
        }
        public void ExcluirCarrinhoLinxProduto(string pedido, string produto, string cor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_CARRINHO_LINX _produto = ObterCarrinhoLinxProduto(pedido, produto, cor, codigoUsuario);
                if (_produto != null)
                {
                    dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes.DeleteOnSubmit(_produto);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoLinxProdutoPorCodigo(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_CARRINHO_LINX produto = ObterCarrinhoLinxProduto(codigo);
                if (produto != null)
                {
                    dbINTRA.DESENV_PRODUTO_CARRINHO_LINXes.DeleteOnSubmit(produto);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_PRODUTO_CARRINHO_LINX

        public List<DESENV_PRODUTO_GRADE> ObterGradeProduto(string grade)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_GRADEs where m.GRADE == grade orderby m.NOME select m).ToList();
        }
        public DESENV_PRODUTO_GRADE ObterGradeProduto(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_GRADEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<DESENV_PRODUTO_GRADE> ObterGradeProduto()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_GRADEs orderby m.GRADE, m.NOME select m).ToList();
        }
        public void InserirGradeProduto(DESENV_PRODUTO_GRADE _grade)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PRODUTO_GRADEs.InsertOnSubmit(_grade);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarGradeProduto(DESENV_PRODUTO_GRADE _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_GRADE _grade = ObterGradeProduto(_novo.CODIGO);

            if (_grade != null)
            {
                _grade.CODIGO = _novo.CODIGO;
                _grade.GRADE = _novo.GRADE;
                _grade.NOME = _novo.NOME;
                _grade.GRADE1 = _novo.GRADE1;
                _grade.GRADE2 = _novo.GRADE2;
                _grade.GRADE3 = _novo.GRADE3;
                _grade.GRADE4 = _novo.GRADE4;
                _grade.GRADE5 = _novo.GRADE5;
                _grade.GRADE6 = _novo.GRADE6;
                _grade.GRADE7 = _novo.GRADE7;
                _grade.GRADE8 = _novo.GRADE8;
                _grade.GRADE9 = _novo.GRADE9;
                _grade.GRADE10 = _novo.GRADE10;
                _grade.GRADE11 = _novo.GRADE11;
                _grade.GRADE12 = _novo.GRADE12;
                _grade.GRADE13 = _novo.GRADE13;
                _grade.GRADE14 = _novo.GRADE14;


                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirGradeProduto(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_GRADE _gradeProduto = ObterGradeProduto(codigo);
                if (_gradeProduto != null)
                {
                    dbINTRA.DESENV_PRODUTO_GRADEs.DeleteOnSubmit(_gradeProduto);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_GERAR_PREPEDIDO_COMPRA_PRODUTOResult GerarINTRAPrePedidoCompraProduto(string fornecedor, DateTime limiteEntrega, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_GERAR_PREPEDIDO_COMPRA_PRODUTO(fornecedor, limiteEntrega, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }
        public SP_GERAR_PEDIDO_COMPRA_PRODUTO_PREPEDIDOResult GerarLINXPedidoCompraProdutoPorPrePedido(string fornecedor, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_GERAR_PEDIDO_COMPRA_PRODUTO_PREPEDIDO(fornecedor, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }


        public SP_GERAR_PEDIDO_COMPRA_PRODUTO_PREPEDIDOV10Result GerarLINXPedidoCompraProdutoPorPrePedidoV10(string fornecedor, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario, string tipo)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_GERAR_PEDIDO_COMPRA_PRODUTO_PREPEDIDOV10(fornecedor, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario, tipo) select i).SingleOrDefault();
        }



        public List<SP_OBTER_PRODUTO_ACABADO_PRODUTO_PEDResult> ObterProdutoAcabadoProdutoPed(string colecao, int? desenvOrigem, string grupoProduto, string produto, string filial, string fornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;

            if (desenvOrigem == null)
                desenvOrigem = 0;

            return (from i in dbINTRA.SP_OBTER_PRODUTO_ACABADO_PRODUTO_PED(colecao, desenvOrigem, grupoProduto, produto, filial, fornecedor) select i).ToList();
        }
        public List<SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDOResult> ObterProdutoAcabadoProdutoPrePedido(string colecao, int? desenvOrigem, string grupoProduto, string produto, string filial, string fornecedor)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PRODUTO_ACABADO_PRODUTO_PREPEDIDO(colecao, desenvOrigem, grupoProduto, produto, filial, fornecedor) select i).ToList();
        }

        public List<SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10Result> ObterProdutoAcabadoProdutoPrePedidoV10(string colecao, int? desenvOrigem, string grupoProduto, string produto, string filial, string fornecedor)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PRODUTO_ACABADO_PRODUTO_V10(colecao, desenvOrigem, grupoProduto, produto, filial, fornecedor) select i).ToList();
        }


        public SP_GERAR_PREPEDIDO_COMPRA_MATERIALResult GerarINTRAPrePedidoCompraMaterial(int codigoCarrinhoCab, string fornecedor, DateTime limiteEntrega, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_GERAR_PREPEDIDO_COMPRA_MATERIAL(codigoCarrinhoCab, fornecedor, limiteEntrega, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }
        public List<SP_OBTER_MATERIAL_PREPEDIDOResult> ObterMaterialPrePedido(string colecao, int? desenvOrigem, string produto, string grupoProduto, string grupoMaterial, string subGrupoMaterial, string filial, string fornecedorMaterial, string pedidoIntra)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_PREPEDIDO(colecao, desenvOrigem, produto, grupoProduto, grupoMaterial, subGrupoMaterial, filial, fornecedorMaterial, pedidoIntra) select i).ToList();
        }
        public SP_GERAR_PEDIDO_COMPRA_MATERIAL_PREPEDIDOResult GerarLINXPedidoCompraMaterialPorPrePedido(int codigoCarrinhoLinxCab, string fornecedor, string filial, string codigoFilialRateio, char status, string aprovadoPor, int codigoUsuario)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_GERAR_PEDIDO_COMPRA_MATERIAL_PREPEDIDO(codigoCarrinhoLinxCab, fornecedor, filial, codigoFilialRateio, status, aprovadoPor, codigoUsuario)
                    select i).SingleOrDefault();
        }


        public List<SP_OBTER_MATERIAL_PEDLINXResult> ObterMaterialPedido(string colecao, int? desenvOrigem, string grupoProduto, string produto, string filialMaterial, string fornecedorMaterial, string grupoMaterial, string subGrupoMaterial, string pedidoIntra, string pedidoLinx)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;

            if (desenvOrigem == null)
                desenvOrigem = 0;

            return (from i in dbINTRA.SP_OBTER_MATERIAL_PEDLINX(colecao, desenvOrigem, grupoProduto, produto, filialMaterial, fornecedorMaterial, grupoMaterial, subGrupoMaterial, pedidoIntra, pedidoLinx)
                    select i).ToList();
        }


        /*DESENV_MATERIAL_CARRINHO_LINX */
        public List<DESENV_MATERIAL_CARRINHO_LINX> ObterCarrinhoLinxMaterialPorCab(int codigoCarrinhoLinxCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes where m.DESENV_MATERIAL_CARRINHO_LINX_CAB == codigoCarrinhoLinxCab orderby m.MATERIAL, m.COR_MATERIAL select m).ToList();
        }
        public DESENV_MATERIAL_CARRINHO_LINX ObterCarrinhoLinxMaterial(string pedido, string material, string cor, int? codFicTec, int codigoCarrinhoLinxCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);

            if (codFicTec == null)
            {

                return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes
                        where
                            m.PEDIDO == pedido
                            && m.MATERIAL == material
                            && m.DESENV_PRODUTO_FICTEC == null
                            && m.COR_MATERIAL == cor
                            && m.DESENV_MATERIAL_CARRINHO_LINX_CAB == codigoCarrinhoLinxCab
                        select m).SingleOrDefault();
            }
            else
            {
                return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes
                        where
                            m.PEDIDO == pedido
                            && m.MATERIAL == material
                            && m.DESENV_PRODUTO_FICTEC == codFicTec
                            && m.COR_MATERIAL == cor
                            && m.DESENV_MATERIAL_CARRINHO_LINX_CAB == codigoCarrinhoLinxCab
                        select m).SingleOrDefault();
            }
        }

        public DESENV_MATERIAL_CARRINHO_LINX ObterCarrinhoLinxMaterial(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoLinxMaterial(DESENV_MATERIAL_CARRINHO_LINX material)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes.InsertOnSubmit(material);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoLinxMaterialPorCab(int codigoCarrinhoLinxCab)
        {
            var carrinho = ObterCarrinhoLinxMaterialPorCab(codigoCarrinhoLinxCab);

            foreach (var c in carrinho)
                ExcluirCarrinhoLinxMaterial(c.PEDIDO, c.MATERIAL, c.COR_MATERIAL, c.DESENV_PRODUTO_FICTEC, codigoCarrinhoLinxCab);
        }
        public void ExcluirCarrinhoLinxMaterial(string pedido, string material, string corMaterial, int? codFicTec, int codigoCarrinhoLinxCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_CARRINHO_LINX _material = ObterCarrinhoLinxMaterial(pedido, material, corMaterial, codFicTec, codigoCarrinhoLinxCab);
                if (_material != null)
                {
                    dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes.DeleteOnSubmit(_material);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoLinxMaterialPorCodigo(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_CARRINHO_LINX material = ObterCarrinhoLinxMaterial(codigo);
                if (material != null)
                {
                    dbINTRA.DESENV_MATERIAL_CARRINHO_LINXes.DeleteOnSubmit(material);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_CARRINHO_LINX


        public List<SP_OBTER_PRODUTO_COMPRA_MATERIALResult> ObterProdutoCompraMATERIAL(string pedidoIntra, string fornecedorMaterial, string colecao, string produto, string grupoProduto, string griffe, string grupoMaterial, string subGrupoMaterial)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_COMPRA_MATERIAL(pedidoIntra, fornecedorMaterial, colecao, produto, grupoProduto, griffe, grupoMaterial, subGrupoMaterial) select i).ToList();
        }
        /*DESENV_MATERIAL_CARRINHO */
        public List<DESENV_MATERIAL_CARRINHO> ObterCarrinhoMaterialPorCab(int codigoCarrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHOs
                    where m.DESENV_MATERIAL_CARRINHO_CAB == codigoCarrinhoCab
                    orderby m.DESENV_PRODUTO_FICTEC
                    select m).ToList();
        }
        public DESENV_MATERIAL_CARRINHO ObterCarrinhoMaterial(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_MATERIAL_CARRINHO ObterCarrinhoMaterial(int codigoProdutoFicTec, int codigoCarrinhoCab, string pedidoOrigem)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHOs
                    where
                        m.DESENV_PRODUTO_FICTEC == codigoProdutoFicTec
                        && m.DESENV_MATERIAL_CARRINHO_CAB == codigoCarrinhoCab
                        && m.PEDIDO_ORIGEM == pedidoOrigem
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoMaterial(DESENV_MATERIAL_CARRINHO _material)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_CARRINHOs.InsertOnSubmit(_material);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoMaterial(DESENV_MATERIAL_CARRINHO _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_CARRINHO _material = ObterCarrinhoMaterial(_novo.CODIGO);

            if (_material != null)
            {
                _material.CODIGO = _novo.CODIGO;
                _material.DESENV_PRODUTO_FICTEC = _novo.DESENV_PRODUTO_FICTEC;
                _material.QTDE = _novo.QTDE;
                _material.CUSTO = _novo.CUSTO;
                _material.DESCONTO_ITEM = _novo.DESCONTO_ITEM;
                _material.HB = _novo.HB;
                _material.PEDIDO_ORIGEM = _novo.PEDIDO_ORIGEM;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCarrinhoMaterialPorCab(int codigoCarrinhoCab)
        {
            var carrinho = ObterCarrinhoMaterialPorCab(codigoCarrinhoCab);

            foreach (var c in carrinho)
                ExcluirCarrinhoMaterial(c.CODIGO);
        }
        public void ExcluirCarrinhoMaterial(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_CARRINHO _material = ObterCarrinhoMaterial(codigo);
                if (_material != null)
                {
                    dbINTRA.DESENV_MATERIAL_CARRINHOs.DeleteOnSubmit(_material);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_CARRINHO


        /*DESENV_MATERIAL_CARRINHO_CAB */
        public List<DESENV_MATERIAL_CARRINHO_CAB> ObterCarrinhoMaterialCab()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_CABs
                    orderby m.DATA_ABERTURA descending
                    select m).ToList();
        }
        public List<DESENV_MATERIAL_CARRINHO_CAB> ObterCarrinhoMaterialCabPorUsuario(int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_CABs where m.USUARIO == codigoUsuario orderby m.DATA_ABERTURA select m).ToList();
        }
        public DESENV_MATERIAL_CARRINHO_CAB ObterCarrinhoMaterialCab(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_CABs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public DESENV_MATERIAL_CARRINHO_CAB ObterCarrinhoMaterialCabPorPedido(string pedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_CABs
                    where
                        m.PEDIDO == pedido
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoMaterialCab(DESENV_MATERIAL_CARRINHO_CAB carrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_CARRINHO_CABs.InsertOnSubmit(carrinhoCab);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoMaterialCab(DESENV_MATERIAL_CARRINHO_CAB carrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_CARRINHO_CAB _carrinhoCab = ObterCarrinhoMaterialCab(carrinhoCab.CODIGO);

            if (_carrinhoCab != null)
            {
                _carrinhoCab.CODIGO = carrinhoCab.CODIGO;
                _carrinhoCab.DATA_FECHAMENTO = carrinhoCab.DATA_FECHAMENTO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCarrinhoMaterialCabPorUsuario(int codigoUsuario)
        {
            var carrinho = ObterCarrinhoMaterialCabPorUsuario(codigoUsuario);

            foreach (var c in carrinho)
                ExcluirCarrinhoMaterialCab(c.CODIGO);
        }
        public void ExcluirCarrinhoMaterialCab(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_CARRINHO_CAB _material = ObterCarrinhoMaterialCab(codigo);
                if (_material != null)
                {
                    dbINTRA.DESENV_MATERIAL_CARRINHO_CABs.DeleteOnSubmit(_material);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_CARRINHO_CAB

        public List<DESENV_MATERIAL_PEDIDO_PERFIL> ObterCarrinhoMaterialPerfil()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_PEDIDO_PERFILs
                    orderby m.PERFIL
                    select m).ToList();
        }
        public DESENV_MATERIAL_PEDIDO_PERFIL ObterCarrinhoMaterialPerfil(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_PEDIDO_PERFILs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }

        //DESENV_MATERIAL_CARRINHO_ESTOQUE
        public List<DESENV_MATERIAL_CARRINHO_ESTOQUE> ObterCarrinhoMaterialEstoquePorCab(int codigoCarrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_ESTOQUEs
                    where m.DESENV_MATERIAL_CARRINHO_CAB == codigoCarrinhoCab
                    orderby m.SUBGRUPO, m.COR_FORNECEDOR
                    select m).ToList();
        }
        public DESENV_MATERIAL_CARRINHO_ESTOQUE ObterCarrinhoMaterialEstoque(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_ESTOQUEs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoMaterialEstoque(DESENV_MATERIAL_CARRINHO_ESTOQUE _materialEstoque)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_CARRINHO_ESTOQUEs.InsertOnSubmit(_materialEstoque);
                dbINTRA.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoMaterialEstoque(DESENV_MATERIAL_CARRINHO_ESTOQUE _novo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_CARRINHO_ESTOQUE _material = ObterCarrinhoMaterialEstoque(_novo.CODIGO);

            if (_material != null)
            {
                _material.CODIGO = _novo.CODIGO;
                _material.QTDE = _novo.QTDE;
                _material.CUSTO = _novo.CUSTO;
                _material.DESCONTO_ITEM = _novo.DESCONTO_ITEM;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCarrinhoMaterialEstoquePorCab(int codigoCarrinhoCab)
        {
            var carrinho = ObterCarrinhoMaterialEstoquePorCab(codigoCarrinhoCab);

            foreach (var c in carrinho)
                ExcluirCarrinhoMaterialEstoque(c.CODIGO);
        }
        public void ExcluirCarrinhoMaterialEstoque(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_CARRINHO_ESTOQUE _material = ObterCarrinhoMaterialEstoque(codigo);
                if (_material != null)
                {
                    dbINTRA.DESENV_MATERIAL_CARRINHO_ESTOQUEs.DeleteOnSubmit(_material);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_CARRINHO_ESTOQUE


        public SP_ATUALIZAR_MATERIAL_CARRINHOResult AtualizarMaterialCarrinhoCusto(int codigoCarrinhoCab, decimal custo, int codigoCarrinho, char tipo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_ATUALIZAR_MATERIAL_CARRINHO(codigoCarrinhoCab, custo, codigoCarrinho, tipo)
                    select i).SingleOrDefault();
        }
        public List<SP_OBTER_MATERIAL_CARRINHO_PREPEDIDOResult> ObterMaterialCarrinhoPrePedido(int codigoCarrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_CARRINHO_PREPEDIDO(codigoCarrinhoCab)
                    select i).ToList();
        }

        public List<SP_OBTER_MATERIAL_PREPEDIDO_EMAILResult> ObterMaterialCarrinhoPrePedidoEmail(int codigoCarrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_PREPEDIDO_EMAIL(codigoCarrinhoCab)
                    select i).ToList();
        }

        public List<SP_OBTER_MATERIAL_CARRINHO_LINX_PREPEDIDOResult> ObterMaterialCarrinhoLinxPrePedido(int codigoCarrinhoCabLinx)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_CARRINHO_LINX_PREPEDIDO(codigoCarrinhoCabLinx)
                    select i).ToList();
        }

        public List<SP_OBTER_MATERIAL_PEDIDO_RESERVADOResult> ObterMaterialPedidoReserva(string pedidoIntranet, string pedidoOrigem, string produto, int hb, string grupoMaterial, string subGrupoMaterial, string corMaterial, int codMatPedido)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_PEDIDO_RESERVADO(pedidoIntranet, pedidoOrigem, produto, hb, grupoMaterial, subGrupoMaterial, corMaterial, codMatPedido)
                    select i).ToList();
        }

        public List<SP_OBTER_MATERIAL_HB_LIBERACAOResult> ObterMaterialHBLib(string colecao, int hb, string produto, string nome)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_HB_LIBERACAO(colecao, hb, produto, nome)
                    select i).ToList();
        }
        [Obsolete("NAO UTILIZAR")]
        public List<SP_OBTER_MATERIAL_HB_FALTResult> ObterMaterialHBFaltante(string colecao, int hb, string produto, string nome)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_HB_FALT(colecao, hb, produto, nome)
                    select i).ToList();
        }

        public List<SP_OBTER_MATERIAL_HB_FALTV2Result> ObterMaterialHBFaltanteV2(string colecao, string hb, string produto, string grupoMaterial, string subGrupoMaterial)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_HB_FALTV2(colecao, hb, produto, grupoMaterial, subGrupoMaterial)
                    select i).ToList();
        }

        public List<SP_OBTER_MATERIAL_ESTOQUEResult> ObterMaterialEstoque(string material, string grupoMaterial, string subGrupoMaterial, string corMaterial, string corFornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_ESTOQUE(material, grupoMaterial, subGrupoMaterial, corMaterial, corFornecedor)
                    select i).ToList();
        }

        public SP_OBTER_MATERIAL_ESTOQUE_SALDO_PEDIDOResult ObterMaterialEstoqueSaldoPedido(string material, string corMaterial, int codigoCarrinho)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_ESTOQUE_SALDO_PEDIDO(material, corMaterial, codigoCarrinho)
                    select i).SingleOrDefault();
        }


        /*DESENV_MATERIAL_CARRINHO_CAB */
        public List<DESENV_MATERIAL_CARRINHO_LINX_CAB> ObterCarrinhoMaterialLinxCabPorCarrinhoCab(int codigoCarrinhoCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_LINX_CABs
                    where m.DESENV_MATERIAL_CARRINHO_CAB == codigoCarrinhoCab
                    orderby m.DATA_ABERTURA descending
                    select m).ToList();
        }
        public DESENV_MATERIAL_CARRINHO_LINX_CAB ObterCarrinhoMaterialLinxCab(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_CARRINHO_LINX_CABs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirCarrinhoMaterialLinxCab(DESENV_MATERIAL_CARRINHO_LINX_CAB carrinhoLinxCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_CARRINHO_LINX_CABs.InsertOnSubmit(carrinhoLinxCab);
                dbINTRA.SubmitChanges();

                return carrinhoLinxCab.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoMaterialLinxCab(DESENV_MATERIAL_CARRINHO_LINX_CAB carrinhoLinxCab)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_CARRINHO_LINX_CAB _carrinhoCab = ObterCarrinhoMaterialLinxCab(carrinhoLinxCab.CODIGO);

            if (_carrinhoCab != null)
            {
                _carrinhoCab.CODIGO = carrinhoLinxCab.CODIGO;
                _carrinhoCab.DATA_FECHAMENTO = carrinhoLinxCab.DATA_FECHAMENTO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCarrinhoMaterialLinxCab(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_CARRINHO_LINX_CAB _material = ObterCarrinhoMaterialLinxCab(codigo);
                if (_material != null)
                {
                    dbINTRA.DESENV_MATERIAL_CARRINHO_LINX_CABs.DeleteOnSubmit(_material);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_CARRINHO_CAB

        /*DESENV_MATERIAL_PEDIDO */
        public List<DESENV_MATERIAL_PEDIDO> ObterMaterialPedidoExp()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_PEDIDOs
                    orderby m.DATA_ENTRADA ascending
                    select m).ToList();
        }
        public DESENV_MATERIAL_PEDIDO ObterMaterialPedidoExp(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_PEDIDOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirMaterialPedidoExp(DESENV_MATERIAL_PEDIDO matPedidoExp)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_PEDIDOs.InsertOnSubmit(matPedidoExp);
                dbINTRA.SubmitChanges();

                return matPedidoExp.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialPedidoExp(DESENV_MATERIAL_PEDIDO matPedidoExp)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_PEDIDO _matPedidoExp = ObterMaterialPedidoExp(matPedidoExp.CODIGO);

            if (_matPedidoExp != null)
            {
                _matPedidoExp.CODIGO = matPedidoExp.CODIGO;
                _matPedidoExp.QTDE_RETIRADA = matPedidoExp.QTDE_RETIRADA;
                _matPedidoExp.QTDE_SOBRA = matPedidoExp.QTDE_SOBRA;
                _matPedidoExp.DATA_BAIXA = matPedidoExp.DATA_BAIXA;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirMaterialPedidoExp(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_PEDIDO _matPedidoExp = ObterMaterialPedidoExp(codigo);
                if (_matPedidoExp != null)
                {
                    dbINTRA.DESENV_MATERIAL_PEDIDOs.DeleteOnSubmit(_matPedidoExp);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_PEDIDO


        public void InserirMaterialEstoque(DESENV_MATERIAL_ESTOQUE matEstoque)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_ESTOQUEs.InsertOnSubmit(matEstoque);
                dbINTRA.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DESENV_MATERIAL_PEDIDO_FALTA ObterMaterialPedidoFaltante(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_PEDIDO_FALTAs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirMaterialPedidoFaltante(DESENV_MATERIAL_PEDIDO_FALTA matPedidoFalta)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_PEDIDO_FALTAs.InsertOnSubmit(matPedidoFalta);
                dbINTRA.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialPedidoFaltante(DESENV_MATERIAL_PEDIDO_FALTA matPedidoFalta)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_PEDIDO_FALTA _matPedidoFalta = ObterMaterialPedidoFaltante(matPedidoFalta.CODIGO);

            if (_matPedidoFalta != null)
            {
                _matPedidoFalta.DATA_BAIXA = matPedidoFalta.DATA_BAIXA;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public SP_OBTER_PRODUTO_PEDIDO_DIFFDIASResult ObterPedidoZipperDiffDias(string colecao, string produto, string cor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_PEDIDO_DIFFDIAS(colecao, produto, cor) select i).FirstOrDefault();
        }


        public List<SP_OBTER_CALENDARIO_FACCAOResult> ObterCalendarioControleProducao(string ano, string mes, string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_CALENDARIO_FACCAO(ano, mes, colecao) select i).ToList();
        }

        public List<SP_OBTER_CALENDARIO_PLANEJAMENTOResult> ObterCalendarioControlePlanejamento(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from i in dbINTRA.SP_OBTER_CALENDARIO_PLANEJAMENTO(colecao) select i).ToList();
        }
        public List<SP_OBTER_CALENDARIO_PRODUCAOResult> ObterCalendarioControleProducao(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from i in dbINTRA.SP_OBTER_CALENDARIO_PRODUCAO(colecao) select i).ToList();
        }
        public List<SP_OBTER_CALENDARIO_PRODUCAO_FALTAResult> ObterCalendarioControleProducaoFaltante(int qtdeCorteFal, int qtdeMediaDia)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from i in dbINTRA.SP_OBTER_CALENDARIO_PRODUCAO_FALTA(qtdeCorteFal, qtdeMediaDia) select i).ToList();
        }

        public List<VW_OBTER_PREPEDIDO_TECIDOCOR> ObterPrePedidoTecidoCor(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_PREPEDIDO_TECIDOCORs
                    where m.COLECAO == colecao
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_MODELCRIACAO> ObterCalProducaoMODESTAMPA(string colecao, int modDesign)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            if (modDesign >= 0)
                return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_MODELCRIACAOs
                        where m.COLECAO == colecao
                        && m.MODDESIGN == modDesign
                        select m).ToList();
            else
                return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_MODELCRIACAOs
                        where m.COLECAO == colecao
                        select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_LIB1PECA> ObterCalProducao1PECA(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_LIB1PECAs
                    where m.COLECAO == colecao
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_MODELAGEM> ObterCalProducaoMODELAGEM(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_MODELAGEMs
                    where m.COLECAO == colecao
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_PRERISCO> ObterCalProducaoPRERISCO(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_PRERISCOs
                    where m.COLECAO == colecao
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_RISCO> ObterCalProducaoRISCO(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_RISCOs
                    where m.COLECAO == colecao
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_CORTE> ObterCalProducaoCORTE(string colecao)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_CORTEs
                    where m.COLECAO == colecao
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_FACCAO> ObterCalProducaoEncaixeFACCAO(string colecao, int codigoProcesso)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_FACCAOs
                    where m.COLECAO == colecao
                    && m.PROD_PROCESSO == codigoProcesso
                    select m).ToList();
        }
        public List<VW_OBTER_CALPRODUCAO_FACCAO_FAL> ObterCalProducaoFACCAOFAL(string colecao, int codigoServico)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            return (from m in dbINTRA.VW_OBTER_CALPRODUCAO_FACCAO_FALs
                    where m.COLECAO == colecao
                    && m.PROD_SERVICO == codigoServico
                    && m.QTDE_TOTAL > 0
                    select m).ToList();
        }

        public List<SP_OBTER_PRODUTO_ACABADO_CARLINXResult> ObterComprasCarrinhoLinx(int codigoUsuario)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PRODUTO_ACABADO_CARLINX(codigoUsuario) select i).ToList();
        }

        public List<DESENV_PEDIDO_CARRINHO> ObterCarrinhoPedidoPorUsuario(int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDO_CARRINHOs where m.USUARIO == codigoUsuario orderby m.DESENV_PEDIDO1.NUMERO_PEDIDO select m).ToList();
        }
        public DESENV_PEDIDO_CARRINHO ObterCarrinhoPedido(int codigoPedido, int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDO_CARRINHOs
                    where
                        m.DESENV_PEDIDO == codigoPedido
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public DESENV_PEDIDO_CARRINHO ObterCarrinhoPedido(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDO_CARRINHOs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoPedido(DESENV_PEDIDO_CARRINHO carrinho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PEDIDO_CARRINHOs.InsertOnSubmit(carrinho);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoPedido(DESENV_PEDIDO_CARRINHO carrinho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_CARRINHO _carrinho = ObterCarrinhoPedido(carrinho.CODIGO);

            if (_carrinho != null)
            {
                _carrinho.QTDE = carrinho.QTDE;
                _carrinho.PRECO = carrinho.PRECO;

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
        public void ExcluirCarrinhoPedidoPorUsuario(int codigoUsuario)
        {
            var carrinho = ObterCarrinhoPedidoPorUsuario(codigoUsuario);

            foreach (var c in carrinho)
                ExcluirCarrinhoPedido(c.DESENV_PEDIDO, codigoUsuario);
        }
        public void ExcluirCarrinhoPedido(int codigoPedido, int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PEDIDO_CARRINHO carrinho = ObterCarrinhoPedido(codigoPedido, codigoUsuario);
                if (carrinho != null)
                {
                    db.DESENV_PEDIDO_CARRINHOs.DeleteOnSubmit(carrinho);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoPedidoPorCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PEDIDO_CARRINHO carrinho = ObterCarrinhoPedido(codigo);
                if (carrinho != null)
                {
                    db.DESENV_PEDIDO_CARRINHOs.DeleteOnSubmit(carrinho);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_FLUXOPGTO_TECIDOResult> ObterFluxoPgtoTecido(string fornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_FLUXOPGTO_TECIDO(fornecedor) select i).ToList();
        }

        public List<SP_OBTER_FORNECEDOR_VENDAResult> ObterFornecedorVendaProduto(string tipo)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_FORNECEDOR_VENDA(tipo) select i).ToList();
        }


        public List<DESENV_CATEGORIA> ObterCategoria()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_CATEGORIAs
                    orderby m.NOME ascending
                    select m).ToList();
        }
        public List<DESENV_CATEGORIA> ObterCategoria(string colecao, string griffe)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_CATEGORIAs
                    where m.COLECAO == colecao && m.GRIFFE == griffe
                    orderby m.ORDEM ascending
                    select m).ToList();
        }
        public DESENV_CATEGORIA ObterCategoria(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_CATEGORIAs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirCategoria(DESENV_CATEGORIA categoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_CATEGORIAs.InsertOnSubmit(categoria);
                dbINTRA.SubmitChanges();

                return categoria.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCategoria(DESENV_CATEGORIA categoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_CATEGORIA _categoria = ObterCategoria(categoria.CODIGO);

            if (_categoria != null)
            {
                _categoria.CODIGO = categoria.CODIGO;
                _categoria.COLECAO = categoria.COLECAO;
                _categoria.GRIFFE = categoria.GRIFFE;
                _categoria.NOME = categoria.NOME;
                _categoria.ORDEM = categoria.ORDEM;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirCategoria(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_CATEGORIA _categoria = ObterCategoria(codigo);
                if (_categoria != null)
                {
                    dbINTRA.DESENV_CATEGORIAs.DeleteOnSubmit(_categoria);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<DESENV_PRODUTO_CATEGORIA> ObterProdutoCategoria(string colecao, string griffe, int codigoCategoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CATEGORIAs
                    where m.DESENV_CATEGORIA1.COLECAO == colecao
                    && m.DESENV_CATEGORIA1.GRIFFE == griffe
                    && m.DESENV_CATEGORIA == codigoCategoria
                    orderby m.DESENV_PRODUTO ascending
                    select m).ToList();
        }
        public DESENV_PRODUTO_CATEGORIA ObterProdutoCategoriaNoCarrinho(string colecao, string griffe, int codigoCategoria, int codigoProduto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CATEGORIAs
                    where m.DESENV_CATEGORIA1.COLECAO == colecao
                    && m.DESENV_CATEGORIA1.GRIFFE == griffe
                    && m.DESENV_CATEGORIA == codigoCategoria
                    && m.DESENV_PRODUTO == codigoProduto
                    select m).SingleOrDefault();
        }
        public DESENV_PRODUTO_CATEGORIA ObterProdutoCategoria(int codigoCategoria, int codigoProduto)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CATEGORIAs
                    where
                        m.DESENV_CATEGORIA == codigoCategoria
                    && m.DESENV_PRODUTO == codigoProduto
                    select m).SingleOrDefault();
        }
        public List<DESENV_PRODUTO_CATEGORIA> ObterProdutoCategoria()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CATEGORIAs
                    orderby m.DESENV_PRODUTO ascending
                    select m).ToList();
        }
        public DESENV_PRODUTO_CATEGORIA ObterProdutoCategoria(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_PRODUTO_CATEGORIAs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirProdutoCategoria(DESENV_PRODUTO_CATEGORIA produtoCategoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_PRODUTO_CATEGORIAs.InsertOnSubmit(produtoCategoria);
                dbINTRA.SubmitChanges();

                return produtoCategoria.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoCategoria(DESENV_PRODUTO_CATEGORIA produtoCategoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_PRODUTO_CATEGORIA _produtoCategoria = ObterProdutoCategoria(produtoCategoria.CODIGO);

            if (_produtoCategoria != null)
            {
                _produtoCategoria.CODIGO = produtoCategoria.CODIGO;
                _produtoCategoria.DESENV_CATEGORIA = produtoCategoria.DESENV_CATEGORIA;
                _produtoCategoria.DESENV_PRODUTO = produtoCategoria.DESENV_PRODUTO;
                _produtoCategoria.ORDEM = produtoCategoria.ORDEM;
                _produtoCategoria.DATA_INCLUSAO = produtoCategoria.DATA_INCLUSAO;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirProdutoCategoria(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PRODUTO_CATEGORIA produtoCategoria = ObterProdutoCategoria(codigo);
                if (produtoCategoria != null)
                {
                    dbINTRA.DESENV_PRODUTO_CATEGORIAs.DeleteOnSubmit(produtoCategoria);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoCategoriaPorCategoria(string colecao, string griffe, int codigoCategoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var car = ObterProdutoCategoria(colecao, griffe, codigoCategoria);
                foreach (var c in car)
                {
                    if (c != null)
                    {
                        dbINTRA.DESENV_PRODUTO_CATEGORIAs.DeleteOnSubmit(c);
                        dbINTRA.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_DESENV_ANALISEPRODUTOResult> ObterCarrinhoProdutoAnalise(string colecao, string griffe, int codigoCategoria)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_DESENV_ANALISEPRODUTO(colecao, griffe, codigoCategoria) select i).ToList();
        }
        public List<SP_OBTER_DESENV_ANALISEPRODUTO_FILTROResult> ObterCarrinhoProdutoAnaliseFiltro(string colecao, string griffe, string grupoProduto, string produto, string tecido, string cor, string corFornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_DESENV_ANALISEPRODUTO_FILTRO(colecao, griffe, grupoProduto, produto, tecido, cor, corFornecedor) select i).ToList();
        }


        public List<SP_OBTER_MATERIAL_TINGIMENTOResult> ObterMaterialTingimento(string material, string grupoMaterial, string subGrupoMaterial, string corMaterial)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_TINGIMENTO(material, grupoMaterial, subGrupoMaterial, corMaterial) select i).ToList();
        }
        public List<SP_OBTER_MATERIAL_TINGIMENTO_RECEBResult> ObterMaterialTingimentoReceb(string material, string grupoMaterial, string subGrupoMaterial, string corMaterial)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_MATERIAL_TINGIMENTO_RECEB(material, grupoMaterial, subGrupoMaterial, corMaterial) select i).ToList();
        }


        public List<DESENV_MATERIAL_TING> ObterMaterialTingimento()
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_TINGs
                    select m).ToList();
        }
        public DESENV_MATERIAL_TING ObterMaterialTingimento(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbINTRA.DESENV_MATERIAL_TINGs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public int InserirMaterialTingimento(DESENV_MATERIAL_TING ting)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbINTRA.DESENV_MATERIAL_TINGs.InsertOnSubmit(ting);
                dbINTRA.SubmitChanges();

                return ting.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialTingimento(DESENV_MATERIAL_TING ting)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            DESENV_MATERIAL_TING _ting = ObterMaterialTingimento(ting.CODIGO);

            if (_ting != null)
            {
                _ting.CODIGO = ting.CODIGO;
                _ting.PROD_HB_PROD_AVIAMENTO = ting.PROD_HB_PROD_AVIAMENTO;
                _ting.MATERIAL = ting.MATERIAL;
                _ting.COR_MATERIAL = ting.COR_MATERIAL;
                _ting.QTDE_SOL = ting.QTDE_SOL;
                _ting.QTDE_RECEB = ting.QTDE_RECEB;
                _ting.DATA_RECEB = ting.DATA_RECEB;
                _ting.USUARIO_RECEB = ting.USUARIO_RECEB;

                try
                {
                    dbINTRA.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirMaterialTing(int codigo)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_MATERIAL_TING _ting = ObterMaterialTingimento(codigo);
                if (_ting != null)
                {
                    dbINTRA.DESENV_MATERIAL_TINGs.DeleteOnSubmit(_ting);
                    dbINTRA.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_PEDIDOSUB_RESERVADOResult> ObterPedidoSubReservado(int numeroPedido, string grupoMaterial, string subGrupoMaterial, string corMaterial, string corFornecedor)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_PEDIDOSUB_RESERVADO(numeroPedido, grupoMaterial, subGrupoMaterial, corMaterial, corFornecedor) select i).ToList();
        }

        public List<DESENV_PEDIDO_CARRINHO_SUB> ObterCarrinhoPedidoSubPorUsuario(int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDO_CARRINHO_SUBs where m.USUARIO == codigoUsuario orderby m.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.NUMERO_PEDIDO select m).ToList();
        }
        public DESENV_PEDIDO_CARRINHO_SUB ObterCarrinhoPedidoSub(int codigoPedidoSub, int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDO_CARRINHO_SUBs
                    where
                        m.DESENV_PEDIDO_SUB == codigoPedidoSub
                        && m.USUARIO == codigoUsuario
                    select m).SingleOrDefault();
        }
        public DESENV_PEDIDO_CARRINHO_SUB ObterCarrinhoPedidoSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PEDIDO_CARRINHO_SUBs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirCarrinhoPedidoSub(DESENV_PEDIDO_CARRINHO_SUB carrinhoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PEDIDO_CARRINHO_SUBs.InsertOnSubmit(carrinhoSub);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCarrinhoPedidoSub(DESENV_PEDIDO_CARRINHO_SUB carrinhoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PEDIDO_CARRINHO_SUB _carrinhoSub = ObterCarrinhoPedidoSub(carrinhoSub.CODIGO);

            if (_carrinhoSub != null)
            {
                _carrinhoSub.QTDE = carrinhoSub.QTDE;
                _carrinhoSub.PRECO = carrinhoSub.PRECO;

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
        public void ExcluirCarrinhoPedidoSubPorUsuario(int codigoUsuario)
        {
            var carrinhoSub = ObterCarrinhoPedidoSubPorUsuario(codigoUsuario);

            foreach (var c in carrinhoSub)
                ExcluirCarrinhoPedidoSub(c.DESENV_PEDIDO_SUB, codigoUsuario);
        }
        public void ExcluirCarrinhoPedidoSub(int codigoPedidoSub, int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PEDIDO_CARRINHO_SUB carrinhoSub = ObterCarrinhoPedidoSub(codigoPedidoSub, codigoUsuario);
                if (carrinhoSub != null)
                {
                    db.DESENV_PEDIDO_CARRINHO_SUBs.DeleteOnSubmit(carrinhoSub);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirCarrinhoPedidoSubPorCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PEDIDO_CARRINHO_SUB carrinhoSub = ObterCarrinhoPedidoSub(codigo);
                if (carrinhoSub != null)
                {
                    db.DESENV_PEDIDO_CARRINHO_SUBs.DeleteOnSubmit(carrinhoSub);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /*ASSOCIACAO DE PREPEDIDO COM SUB PEDIDOS*/
        public List<DESENV_PREPEDIDO_SUB> ObterPrepedidoSub()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PREPEDIDO_SUBs orderby m.DESENV_PEDIDO_SUB1.DESENV_PEDIDO1.NUMERO_PEDIDO select m).ToList();
        }
        public DESENV_PREPEDIDO_SUB ObterPrepedidoSub(int codigoPrepedido, int codigoPedidoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PREPEDIDO_SUBs
                    where
                        m.DESENV_PEDIDO_SUB == codigoPedidoSub
                        && m.DESENV_PREPEDIDO == codigoPrepedido
                    select m).SingleOrDefault();
        }
        public DESENV_PREPEDIDO_SUB ObterPrepedidoSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.DESENV_PREPEDIDO_SUBs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirPrepedidoSub(DESENV_PREPEDIDO_SUB prepedidoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.DESENV_PREPEDIDO_SUBs.InsertOnSubmit(prepedidoSub);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPrepedidoSub(DESENV_PREPEDIDO_SUB prepedidoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            DESENV_PREPEDIDO_SUB _prepedidoSub = ObterPrepedidoSub(prepedidoSub.CODIGO);

            if (_prepedidoSub != null)
            {
                _prepedidoSub.CODIGO = prepedidoSub.CODIGO;
                _prepedidoSub.DESENV_PREPEDIDO = prepedidoSub.DESENV_PREPEDIDO;
                _prepedidoSub.DESENV_PEDIDO_SUB = prepedidoSub.DESENV_PEDIDO_SUB;
                _prepedidoSub.DATA_INCLUSAO = prepedidoSub.DATA_INCLUSAO;

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
        public void ExcluirPrepedidoSub(int codigoPrepedido, int codigoPedidoSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PREPEDIDO_SUB prepedidoSub = ObterPrepedidoSub(codigoPrepedido, codigoPedidoSub);
                if (prepedidoSub != null)
                {
                    db.DESENV_PREPEDIDO_SUBs.DeleteOnSubmit(prepedidoSub);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirPrePedidoSubPorCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                DESENV_PREPEDIDO_SUB prepedidoSub = ObterPrepedidoSub(codigo);
                if (prepedidoSub != null)
                {
                    db.DESENV_PREPEDIDO_SUBs.DeleteOnSubmit(prepedidoSub);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_OBTER_PAINEL_ENTREGA_LOJAResult ObterPainelEntregaLoja(DateTime dataIni, DateTime dataFim, string colecaoLinx, string griffe, string grupoProduto, string codCategoria, string fabPropria)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PAINEL_ENTREGA_LOJA(dataIni, dataFim, colecaoLinx, griffe, grupoProduto, codCategoria, fabPropria) select i).SingleOrDefault();
        }
        public List<SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTOResult> ObterPainelEntregaLojaProduto(DateTime dataIni, DateTime dataFim, string colecaoLinx, string griffe, string grupoProduto, string codCategoria, string fabPropria)
        {
            dbLINX = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLINX.SP_OBTER_PAINEL_ENTREGA_LOJA_PRODUTO(dataIni, dataFim, colecaoLinx, griffe, grupoProduto, codCategoria, fabPropria) select i).ToList();
        }

        public List<dynamic> ObterPainelEntregaFaccCorteLoja(string colecaoLinxCon)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbINTRA.CommandTimeout = 0;
            var conn = dbINTRA.Connection;

            var p = new DynamicParameters();
            p.Add("@P_COLECAO", colecaoLinxCon);

            var faccCorteLoja = conn.Query("SP_OBTER_PAINEL_FACCCORTELOJA", param: p,
                commandType: CommandType.StoredProcedure).ToList();

            return faccCorteLoja;
        }


        public List<SP_OBTER_DIASUTEISResult> ObterDiasUteis(int ano, int mes)
        {
            dbINTRA = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbINTRA.SP_OBTER_DIASUTEIS(ano, mes) select i).ToList();
        }



    }
}
