using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class RepresentanteController
    {
        DCDataContext db;

        public List<VENDAS_LOTE> BuscaVendas()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from v in db.VENDAS_LOTEs orderby v.PEDIDO_EXTERNO select v).ToList();
        }

        //public List<VENDA> BuscaVendas()

        public VENDAS_LOTE ObterVenda (string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.VENDAS_LOTEs where m.PEDIDO_EXTERNO == codigo select m).SingleOrDefault();
        }

        public VENDAS_LOTE_PROD ObterVendaProduto (string codigoVenda, string codigoProduto, string corProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.VENDAS_LOTE_PRODs 
                    where m.PEDIDO_EXTERNO == codigoVenda && 
                    m.PRODUTO == codigoProduto && 
                    m.COR_PRODUTO == corProduto  
                    select m).SingleOrDefault();
        }

        public void InserirVenda(VENDAS_LOTE venda)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.VENDAS_LOTEs.InsertOnSubmit(venda);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaVenda(VENDAS_LOTE vendaNew)
        {
            db = new DCDataContext(Constante.ConnectionString);

            VENDAS_LOTE vendaOld = ObterVenda(vendaNew.PEDIDO_EXTERNO);

            if (vendaOld != null)
            {
                vendaOld.TIPO_DO_DIGITADOR = vendaNew.TIPO_DO_DIGITADOR;
                vendaOld.MOEDA = vendaNew.MOEDA;
                vendaOld.CODIGO_TAB_PRECO = vendaNew.CODIGO_TAB_PRECO;
                vendaOld.TIPO = vendaNew.TIPO;
                vendaOld.CONDICAO_PGTO = vendaNew.CONDICAO_PGTO;
                vendaOld.COLECAO = vendaNew.COLECAO;
                vendaOld.CLIENTE_ATACADO = vendaNew.CLIENTE_ATACADO;
                vendaOld.REPRESENTANTE = vendaNew.REPRESENTANTE;
                vendaOld.PEDIDO = vendaNew.PEDIDO;
                vendaOld.EMISSAO = vendaNew.EMISSAO;
                vendaOld.DATA_ENVIO = vendaNew.DATA_ENVIO;
                vendaOld.DATA_RECEBIMENTO = vendaNew.DATA_RECEBIMENTO;
                vendaOld.PEDIDO_CLIENTE = vendaNew.PEDIDO_CLIENTE;
                vendaOld.DESCONTO = vendaNew.DESCONTO;
                vendaOld.ENCARGO = vendaNew.ENCARGO;
                vendaOld.TOT_QTDE_ORIGINAL = vendaNew.TOT_QTDE_ORIGINAL;
                vendaOld.TOT_VALOR_ORIGINAL = vendaNew.TOT_VALOR_ORIGINAL;
                vendaOld.VALOR_IPI = vendaNew.VALOR_IPI;
                vendaOld.CTRL_MULT_ENTREGAS = vendaNew.CTRL_MULT_ENTREGAS;
                vendaOld.ENTREGA_CIF = vendaNew.ENTREGA_CIF;
                vendaOld.ENTREGA_ACEITAVEL = vendaNew.ENTREGA_ACEITAVEL;
                vendaOld.DESCONTO_SOBRE_1 = vendaNew.DESCONTO_SOBRE_1;
                vendaOld.DESCONTO_SOBRE_2 = vendaNew.DESCONTO_SOBRE_2;
                vendaOld.DESCONTO_SOBRE_3 = vendaNew.DESCONTO_SOBRE_3;
                vendaOld.DESCONTO_SOBRE_4 = vendaNew.DESCONTO_SOBRE_4;
                vendaOld.FILIAL = vendaNew.FILIAL;
                vendaOld.TRANSPORTADORA = vendaNew.TRANSPORTADORA;
                vendaOld.GERENTE = vendaNew.GERENTE;
                vendaOld.APROVACAO = vendaNew.APROVACAO;
                vendaOld.APROVADO_POR = vendaNew.APROVADO_POR;
                vendaOld.CONFERIDO = vendaNew.CONFERIDO;
                vendaOld.CONFERIDO_POR = vendaNew.CONFERIDO_POR;
                vendaOld.COMISSAO = vendaNew.COMISSAO;
                vendaOld.COMISSAO_GERENTE = vendaNew.COMISSAO_GERENTE;
                vendaOld.PORCENTAGEM_ACERTO = vendaNew.PORCENTAGEM_ACERTO;
                vendaOld.PRIORIDADE = vendaNew.PRIORIDADE;
                vendaOld.ACEITA_JUNTAR_PED = vendaNew.ACEITA_JUNTAR_PED;
                vendaOld.STATUS = vendaNew.STATUS;
                vendaOld.DATA_PARA_TRANSFERENCIA = vendaNew.DATA_PARA_TRANSFERENCIA;
                vendaOld.OBS = vendaNew.OBS;
                vendaOld.TIPO_FRETE = vendaNew.TIPO_FRETE;
                vendaOld.TABELA_FILHA = vendaNew.TABELA_FILHA;
                vendaOld.MULTI_DESCONTO_ACUMULAR = vendaNew.MULTI_DESCONTO_ACUMULAR;
                vendaOld.RECARGO = vendaNew.RECARGO;
                vendaOld.OBS_TRANSPORTE = vendaNew.OBS_TRANSPORTE;
                vendaOld.ACEITA_PECAS_COM_CORTE = vendaNew.ACEITA_PECAS_COM_CORTE;
                vendaOld.ACEITA_PECAS_PEQUENAS = vendaNew.ACEITA_PECAS_PEQUENAS;
                vendaOld.DATA_FATURAMENTO_RELATIVO = vendaNew.DATA_FATURAMENTO_RELATIVO;
                vendaOld.EXPEDICAO_COMPLETO_CARTELA = vendaNew.EXPEDICAO_COMPLETO_CARTELA;
                vendaOld.EXPEDICAO_COMPLETO_COORDENADO = vendaNew.EXPEDICAO_COMPLETO_COORDENADO;
                vendaOld.EXPEDICAO_COMPLETO_COR = vendaNew.EXPEDICAO_COMPLETO_COR;
                vendaOld.EXPEDICAO_COMPLETO_PACK = vendaNew.EXPEDICAO_COMPLETO_PACK;
                vendaOld.EXPEDICAO_COMPLETO_PEDIDO = vendaNew.EXPEDICAO_COMPLETO_PEDIDO;
                vendaOld.EXPEDICAO_COMPLETO_TAMANHOS = vendaNew.EXPEDICAO_COMPLETO_TAMANHOS;
                vendaOld.EXPEDICAO_NAO_JUNTAR_PRODUTO_CAIXA = vendaNew.EXPEDICAO_NAO_JUNTAR_PRODUTO_CAIXA;
                vendaOld.EXPEDICAO_PORCENTAGEM_MAIOR = vendaNew.EXPEDICAO_PORCENTAGEM_MAIOR;
                vendaOld.EXPEDICAO_PORCENTAGEM_MINIMA = vendaNew.EXPEDICAO_PORCENTAGEM_MINIMA;
                vendaOld.EXPEDICAO_PORCENTAGEM_TIPO = vendaNew.EXPEDICAO_PORCENTAGEM_TIPO;
                vendaOld.FILIAL_DIGITACAO = vendaNew.FILIAL_DIGITACAO;
                vendaOld.FRETE_CORTESIA = vendaNew.FRETE_CORTESIA;
                vendaOld.INDICA_LOCAL_SEPARACAO = vendaNew.INDICA_LOCAL_SEPARACAO;
                vendaOld.NOME_CLIFOR_ENTREGA = vendaNew.NOME_CLIFOR_ENTREGA;
                vendaOld.NUMERO_ENTREGA = vendaNew.NUMERO_ENTREGA;
                vendaOld.PEDIDO_EXTERNO_ORIGEM = vendaNew.PEDIDO_EXTERNO_ORIGEM;
                vendaOld.PERIODO_PCP = vendaNew.PERIODO_PCP;
                vendaOld.TIPO_CAIXA = vendaNew.TIPO_CAIXA;
                vendaOld.TIPO_RATEIO = vendaNew.TIPO_RATEIO;
                vendaOld.TRANSP_REDESPACHO = vendaNew.TRANSP_REDESPACHO;
                vendaOld.PORC_DESCONTO = vendaNew.PORC_DESCONTO;
                vendaOld.NATUREZA_SAIDA = vendaNew.NATUREZA_SAIDA;
                vendaOld.COMISSAO_VALOR_GERENTE = vendaNew.COMISSAO_VALOR_GERENTE;
                vendaOld.COMISSAO_VALOR = vendaNew.COMISSAO_VALOR;
                vendaOld.PORC_DESCONTO_COND_PGTO = vendaNew.PORC_DESCONTO_COND_PGTO;
                vendaOld.DESCONTO_COND_PGTO = vendaNew.DESCONTO_COND_PGTO;
                vendaOld.PORC_ENCARGO = vendaNew.PORC_ENCARGO;
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserirVendaProduto(VENDAS_LOTE_PROD produto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            try
            {
                db.VENDAS_LOTE_PRODs.InsertOnSubmit(produto);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaVendaProduto(VENDAS_LOTE_PROD produtoNew)
        {
            db = new DCDataContext(Constante.ConnectionString);

            VENDAS_LOTE_PROD produtoOld = ObterVendaProduto(produtoNew.PEDIDO_EXTERNO, produtoNew.PRODUTO, produtoNew.COR_PRODUTO);

            if (produtoOld != null)
            {
                produtoOld.ENTREGA = produtoNew.ENTREGA;
                produtoOld.SEQUENCIAL_DIGITACAO = produtoNew.SEQUENCIAL_DIGITACAO;
                produtoOld.PACKS = produtoNew.PACKS;
                produtoOld.NUMERO_CONJUNTO = produtoNew.NUMERO_CONJUNTO;
                produtoOld.NUMERO_ENTREGA = produtoNew.NUMERO_ENTREGA;
                produtoOld.LIMITE_ENTREGA = produtoNew.LIMITE_ENTREGA;
                produtoOld.STATUS_VENDA_ATUAL = produtoNew.STATUS_VENDA_ATUAL;
                produtoOld.QTDE_ORIGINAL = produtoNew.QTDE_ORIGINAL;
                produtoOld.VALOR_ORIGINAL = produtoNew.VALOR_ORIGINAL;
                produtoOld.PRECO1 = produtoNew.PRECO1;
                produtoOld.PRECO2 = produtoNew.PRECO2;
                produtoOld.PRECO3 = produtoNew.PRECO3;
                produtoOld.PRECO4 = produtoNew.PRECO4;
                produtoOld.DESCONTO_ITEM = produtoNew.DESCONTO_ITEM;
                produtoOld.IPI = produtoNew.IPI;
                produtoOld.VO2 = produtoNew.VO1;
                produtoOld.VO2 = produtoNew.VO2;
                produtoOld.VO3 = produtoNew.VO3;
                produtoOld.VO4 = produtoNew.VO4;
                produtoOld.VO5 = produtoNew.VO5;
                produtoOld.VO6 = produtoNew.VO6;
                produtoOld.VO2 = produtoNew.VO7;
                produtoOld.VO2 = produtoNew.VO8;
                produtoOld.VO3 = produtoNew.VO9;
                produtoOld.VO4 = produtoNew.VO10;
                produtoOld.VO5 = produtoNew.VO11;
                produtoOld.VO6 = produtoNew.VO12;
                produtoOld.VO2 = produtoNew.VO13;
                produtoOld.VO2 = produtoNew.VO14;
                produtoOld.VO3 = produtoNew.VO15;
                produtoOld.VO4 = produtoNew.VO16;
                produtoOld.VO5 = produtoNew.VO17;
                produtoOld.VO6 = produtoNew.VO18;
                produtoOld.VO2 = produtoNew.VO19;
                produtoOld.VO2 = produtoNew.VO20;
                produtoOld.VO3 = produtoNew.VO21;
                produtoOld.VO4 = produtoNew.VO22;
                produtoOld.VO5 = produtoNew.VO23;
                produtoOld.VO6 = produtoNew.VO24;
                produtoOld.VO2 = produtoNew.VO25;
                produtoOld.VO2 = produtoNew.VO26;
                produtoOld.VO3 = produtoNew.VO27;
                produtoOld.VO4 = produtoNew.VO28;
                produtoOld.VO5 = produtoNew.VO29;
                produtoOld.VO6 = produtoNew.VO30;
                produtoOld.VO2 = produtoNew.VO31;
                produtoOld.VO2 = produtoNew.VO32;
                produtoOld.VO3 = produtoNew.VO33;
                produtoOld.VO4 = produtoNew.VO34;
                produtoOld.VO5 = produtoNew.VO35;
                produtoOld.VO6 = produtoNew.VO36;
                produtoOld.VO2 = produtoNew.VO37;
                produtoOld.VO2 = produtoNew.VO38;
                produtoOld.VO3 = produtoNew.VO39;
                produtoOld.VO4 = produtoNew.VO40;
                produtoOld.VO5 = produtoNew.VO41;
                produtoOld.VO6 = produtoNew.VO42;
                produtoOld.VO2 = produtoNew.VO43;
                produtoOld.VO2 = produtoNew.VO44;
                produtoOld.VO3 = produtoNew.VO45;
                produtoOld.VO4 = produtoNew.VO46;
                produtoOld.VO5 = produtoNew.VO47;
                produtoOld.VO6 = produtoNew.VO48;
                produtoOld.DATA_PARA_TRANSFERENCIA = produtoNew.DATA_PARA_TRANSFERENCIA;
                produtoOld.CODIGO_LOCAL_ENTREGA = produtoNew.CODIGO_LOCAL_ENTREGA;
                produtoOld.DESC_VENDA_CLIENTE = produtoNew.DESC_VENDA_CLIENTE;
                produtoOld.ITEM_PEDIDO = produtoNew.ITEM_PEDIDO;
                produtoOld.NUMERO_CAIXAS = produtoNew.NUMERO_CAIXAS;
                produtoOld.TIPO_CAIXA = produtoNew.TIPO_CAIXA;
                produtoOld.COMISSAO_ITEM = produtoNew.COMISSAO_ITEM;
                produtoOld.COMISSAO_ITEM_GERENTE = produtoNew.COMISSAO_ITEM_GERENTE;
                produtoOld.ID_MODIFICACAO = produtoNew.ID_MODIFICACAO;
                produtoOld.COMISSAO_VALOR_GERENTE = produtoNew.COMISSAO_VALOR_GERENTE;
                produtoOld.COMISSAO_VALOR = produtoNew.COMISSAO_VALOR;
            }

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CLIENTE_REPRE ObterRepresentante(string nomeCliente)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CLIENTE_REPREs where m.CLIENTE_ATACADO == nomeCliente select m).SingleOrDefault();
        }

        public string ObterNomeRepresentante(string cliFor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CADASTRO_CLI_FORs where m.CLIFOR == cliFor select m.NOME_CLIFOR).FirstOrDefault();
        }

        public SP_OBTER_CLIENTE_ATACADOResult ObterClienteAtacado(string cliFor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.SP_OBTER_CLIENTE_ATACADO(cliFor).ToList().FirstOrDefault();
        }

        public SP_OBTER_PRODUTO_PRECO_INFOResult ObterProdutoPrecoInfo(string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.SP_OBTER_PRODUTO_PRECO_INFO(produto).ToList().FirstOrDefault();
        }

        public List<SP_OBTER_GRADE_ESTOQUE_CORResult> ObterGradeEstoqueCor(string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.SP_OBTER_GRADE_ESTOQUE_COR(produto).ToList();
        }
    }
}
