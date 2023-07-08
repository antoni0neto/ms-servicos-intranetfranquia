using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class BaseController
    {
        DCDataContext db;
        INTRADataContext dbIntra;
        LINXDataContext dbLinx;


        public VENDA ObterPedido(string pedido)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.VENDAs where p.PEDIDO == pedido select p).SingleOrDefault();
        }
        public VENDA ObterPedidoPorPedidoExterno(string pedidoExterno)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.VENDAs where p.PEDIDO_EXTERNO == pedidoExterno select p).SingleOrDefault();
        }

        public void AtualizarStatusPedido(string pedido, char aprovacao = 'A')
        {
            db = new DCDataContext(Constante.ConnectionString);

            VENDA venda = ObterPedido(pedido);

            if (venda != null)
            {
                venda.APROVACAO = aprovacao;
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
        public void AtualizarPedidoCondPagamento(VENDA vendaPedido)
        {
            db = new DCDataContext(Constante.ConnectionString);

            VENDA venda = ObterPedido(vendaPedido.PEDIDO);

            if (venda != null)
            {
                venda.CONDICAO_PGTO = vendaPedido.CONDICAO_PGTO;
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

        public List<TRANSPORTADORA> ObterTransportadoras()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from c in dbLinx.TRANSPORTADORAs
                    orderby c.TRANSPORTADORA1
                    select c).ToList();
        }
        public List<REGIOES_VENDA> ObterRegioesVenda()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from c in dbLinx.REGIOES_VENDAs
                    orderby c.REGIAO
                    select c).ToList();
        }
        public List<TABELAS_PRECO> ObterTabelaPreco()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from c in dbLinx.TABELAS_PRECOs
                    orderby c.CODIGO_TAB_PRECO
                    select c).ToList();
        }
        public List<VENDAS_TIPO> ObterVendaTipos()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from c in dbLinx.VENDAS_TIPOs
                    orderby c.TIPO
                    select c).ToList();
        }
        public List<CTB_LX_DOCUMENTO_TIPO> ObterDocumentoTipos()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from c in dbLinx.CTB_LX_DOCUMENTO_TIPOs
                    orderby c.TIPO_DOCUMENTO
                    select c).ToList();
        }

        public List<LCF_LX_UF> ObterUF()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from c in dbLinx.LCF_LX_UFs
                    where c.ID_PAIS == 1
                    orderby c.UF
                    select c).ToList();
        }

        public List<VENDAS_PRODUTO> ObterProdutoPorPedidoLinx(string pedido)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from c in db.VENDAS_PRODUTOs
                    where c.PEDIDO == pedido
                    orderby c.PRODUTO
                    select c).ToList();
        }

        public FATURAMENTO ObterNFFaturamento(string filial, string nf_saida, string serie, DateTime emissao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.FATURAMENTOs
                    where m.FILIAL.Trim() == filial.Trim() &&
                            m.NF_SAIDA.Trim() == nf_saida.Trim() &&
                            m.SERIE_NF.Trim() == serie.Trim() &&
                            m.EMISSAO == emissao
                    select m).SingleOrDefault();
        }

        [Obsolete("SEM USO")]
        public SP_OBTER_SALARIO_FUNCIONARIOResult ObterSalarioFuncionarioX(string cpf)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_SALARIO_FUNCIONARIO(cpf) select d).SingleOrDefault();
        }

        public SP_OBTER_PRODUTO_TAMANHOResult ObterProdutoTamanho(string produto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_PRODUTO_TAMANHO(produto) select d).SingleOrDefault();
        }

        public SP_OBTER_ECOM_AGORAREDEResult ObterEcomAgoraRede()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_ECOM_AGORAREDE() select d).SingleOrDefault();
        }
        public List<SP_AGORA_REDEResult> ObterAgoraRede(string codigoFilial, DateTime? data)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_AGORA_REDE(codigoFilial, data) select d).ToList();
        }

        public SP_OBTER_PARAM_LINXResult ObterParametroLinx(string parametro)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_PARAM_LINX(parametro) select d).SingleOrDefault();
        }

        public List<SP_OBTER_CUPOM_FISCALResult> ObterCupomFiscal(string cupomFiscal, string codigoFilial, string produto, DateTime? dataVenda)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from d in db.SP_OBTER_CUPOM_FISCAL(cupomFiscal, codigoFilial, produto, dataVenda) select d).ToList();
        }

        public SP_OBTER_DADOS_XMLNFEResult ObterDadosXMLNfe(string cnpj, string chaveNFe, string protocoloCancNfe, int query)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.SP_OBTER_DADOS_XMLNFE(cnpj, chaveNFe, protocoloCancNfe, query) select d).SingleOrDefault();
        }

        public List<SP_OBTER_ANOSEMANAResult> ObterAnoSemana(int? anoSemana)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from d in dbIntra.SP_OBTER_ANOSEMANA(anoSemana) select d).ToList();
        }

        public List<SP_OBTER_PRODUTO_NF_RET_DEFResult> ObterProdutoNotaRetirada(int codigoNotaRetirada)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from d in dbIntra.SP_OBTER_PRODUTO_NF_RET_DEF(codigoNotaRetirada) select d).ToList();
        }

        public CADASTRO_CLI_FOR ObterCadastroCLIFORPorCNPJ(string cnpj)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from d in db.CADASTRO_CLI_FORs where d.CGC_CPF.Trim() == cnpj.Trim() && d.INATIVO == false select d).FirstOrDefault();
        }

        public CADASTRO_CLI_FOR ObterCadastroCLIFOR(string nomeCLIFOR)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from d in db.CADASTRO_CLI_FORs where d.NOME_CLIFOR.Trim() == nomeCLIFOR.Trim() select d).SingleOrDefault();
        }

        public CADASTRO_CLI_FOR ObterCadastroCLIFORCodigo(string codCLIFOR)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from d in db.CADASTRO_CLI_FORs where d.COD_CLIFOR.Trim() == codCLIFOR.Trim() select d).SingleOrDefault();
        }

        public List<CADASTRO_CLI_FOR> ObterCadastroClientes(string nomeCliente, string codigoCliente, string cnpjCliente)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from c in db.CADASTRO_CLI_FORs
                    where c.NOME_CLIFOR.Contains(nomeCliente) && c.CLIFOR.Contains(codigoCliente) && c.CGC_CPF.Contains(cnpjCliente)
                    orderby c.NOME_CLIFOR
                    select c).ToList();
        }

        public List<CADASTRO_CLI_FOR> ObterFilialCadastroCLIFOR()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from d in db.CADASTRO_CLI_FORs
                    where d.INDICA_FILIAL == true
                    select d).ToList();
        }

        public List<LOJA_FORMAS_PGTO> ObterFormaPgtoLojaValeMercadoria()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.LOJA_FORMAS_PGTOs
                    where d.COD_FORMA_PGTO == "15" ||
                            d.COD_FORMA_PGTO == "16" ||
                            d.COD_FORMA_PGTO == "17" ||
                            d.COD_FORMA_PGTO == "18" ||
                            d.COD_FORMA_PGTO == "19"
                    select d).ToList();
        }
        public List<LOJA_FORMAS_PGTO> ObterFormaPgtoLojaCliente()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from d in dbLinx.LOJA_FORMAS_PGTOs
                    where d.COD_FORMA_PGTO != "15" &&
                            d.COD_FORMA_PGTO != "16" &&
                            d.COD_FORMA_PGTO != "17" &&
                            d.COD_FORMA_PGTO != "18" &&
                            d.COD_FORMA_PGTO != "19" &&

                            d.COD_FORMA_PGTO != "02" &&
                            d.COD_FORMA_PGTO != "03" &&
                            d.COD_FORMA_PGTO != "06" &&
                            d.COD_FORMA_PGTO != "07" &&
                            d.COD_FORMA_PGTO != "08" &&
                            d.COD_FORMA_PGTO != "09" &&

                            d.FORMA_PGTO != "DESATIVADO VALE                         "
                    select d).ToList();
        }

        public List<DATA_ANO> ObterDataAno()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from d in db.DATA_ANOs orderby d.ANO select d).ToList();
        }

        public List<SP_APP_OBTER_DESEMPENHOResult> WS_ObterDesempenhoFilial(string dataIni, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from f in db.SP_APP_OBTER_DESEMPENHO(Convert.ToDateTime(dataIni), Convert.ToDateTime(dataFim), "") select f).ToList();
        }
        public SP_APP_OBTER_DESEMPENHOResult WS_ObterDesempenhoFilial(string dataIni, string dataFim, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from f in db.SP_APP_OBTER_DESEMPENHO(Convert.ToDateTime(dataIni), Convert.ToDateTime(dataFim), filial) select f).SingleOrDefault();
        }

        public List<FILIAI1> ObterFiliaisLojaIntranet()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.FILIAI1s where f.TIPO_FILIAL == "LOJA" select f).OrderBy(p => p.FILIAL).ToList();
        }

        public List<FILIAI1> ObterFiliaisIntranet(int usuarioId)
        {
            var usuario = BuscaUsuario(usuarioId);
            if (usuario == null)
                return null;

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (usuario.CODIGO_PERFIL == 1)
            {
                return (from f in db.FILIAI1s
                        join l in db.USUARIOLOJAs on f.COD_FILIAL equals l.CODIGO_LOJA
                        join u in db.USUARIOs on l.CODIGO_USUARIO equals u.CODIGO_USUARIO
                        where f.TIPO_FILIAL == "LOJA" && u.CODIGO_PERFIL == 3
                        orderby l.CODIGO_USUARIO ascending, f.FILIAL ascending
                        select f).ToList();
            }
            else
            {
                return (from f in db.FILIAI1s
                        join l in db.USUARIOLOJAs on f.COD_FILIAL equals l.CODIGO_LOJA
                        join u in db.USUARIOs on l.CODIGO_USUARIO equals u.CODIGO_USUARIO
                        where f.TIPO_FILIAL == "LOJA" && u.CODIGO_USUARIO == usuarioId
                        orderby l.CODIGO_USUARIO ascending, f.FILIAL ascending
                        select f).ToList();
            }
        }

        public FILIAI1 ObterFilialIntranet(string cod_filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.FILIAI1s where f.COD_FILIAL == cod_filial select f).SingleOrDefault();
        }

        public List<FORMA_PGTO> ObterFormaPgto()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.FORMA_PGTOs where !SqlMethods.Like(i.DESC_COND_PGTO.ToUpper().Trim(), "%ATACADO%") orderby i.DESC_COND_PGTO select i).ToList();
        }

        public FORMA_PGTO ObterFormaPgto(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.FORMA_PGTOs where i.CONDICAO_PGTO == codigo select i).SingleOrDefault();
        }

        public List<SP_OBTER_TICKETS_DESCONTOResult> ObterTicketsDesconto(string filial, string dataIni, string dataFim)
        {
            string vDataIni = AjustaData(dataIni);
            string vDataFim = AjustaData(dataFim);

            if (filial == "999999")
                filial = null;

            db = new DCDataContext(Constante.ConnectionString);
            return (from f in db.SP_OBTER_TICKETS_DESCONTO(filial, vDataIni, vDataFim) select f).ToList();
        }

        public List<FN_OBTER_DESCONTOResult> ObterTicketsDescontoDetalhe(string filial, string dataIni, string dataFim, char valeMercadoria)
        {
            string vDataIni = AjustaData(dataIni);
            string vDataFim = AjustaData(dataFim);

            if (filial == "999999")
                filial = null;

            db = new DCDataContext(Constante.ConnectionString);
            return (from f in db.FN_OBTER_DESCONTO(filial, vDataIni, vDataFim, valeMercadoria) select f).ToList();
        }

        public string ObterTipoPgtoTicket(string filial, string terminal, string lancamento_caixa)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.FN_OBTER_TIPO_PGTO(filial, terminal, lancamento_caixa);
        }

        public List<SP_BUSCAR_DESPESA_ALUGUEL_ANUALResult> BuscarDespesaAluguelAnual(string filial, int codigo_despesa, string ano)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from d in db.SP_BUSCAR_DESPESA_ALUGUEL_ANUAL(filial, codigo_despesa, ano) select d).ToList();
        }

        public List<SP_BUSCAR_DESPESA_ALUGUEL_REDEResult> BuscarDespesaAluguelRede(string ano)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.SP_BUSCAR_DESPESA_ALUGUEL_REDE(ano) select d).ToList();
        }

        //SE TIVER FILIAL PASSAR APENAS O ANO PARA TRAZER POR ANUAL
        //SE NAO TEM FILIAL, PASSAR COMPETENCIA "012014" PARA TRAZER MENSAL POR FILIAL
        public List<SP_BUSCAR_FATURAMENTO_ANUALResult> BuscarFaturamentoAnual(string filial, string mesano)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.SP_BUSCAR_FATURAMENTO_ANUAL(filial, mesano) select d).ToList();
        }

        public List<SP_BUSCAR_FATURAMENTO_REDEResult> BuscarFaturamentoRede(string ano)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.SP_BUSCAR_FATURAMENTO_REDE(ano) select d).ToList();
        }

        public void AtualizaParametrosFechamento(DateTime _data, string _param)
        {
            db = new DCDataContext(Constante.ConnectionString);

            PARAMETRO parametro = BuscarParametro(_param);

            if (parametro != null)
            {
                parametro.PARAMETRO1 = _param;
                parametro.VALOR_ATUAL = _data.ToString("dd/MM/yyyy");
                parametro.ULT_ATUALIZACAO = DateTime.Now;
                parametro.PENULT_ATUALIZACAO = parametro.ULT_ATUALIZACAO;
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

        public PARAMETRO BuscarParametro(string _param)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PARAMETROs where p.PARAMETRO1 == _param select p).SingleOrDefault();
        }

        public List<PARAMETRO> BuscarParametroFechaPeriodo()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PARAMETROs where SqlMethods.Like(p.PARAMETRO1, "FECHA_PERIODO%") select p).ToList();
        }

        public DEPOSITO_BANCO BuscarDadosBanco(int codigo_banco)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from s in db.DEPOSITO_BANCOs where s.CODIGO_BANCO == codigo_banco select s).SingleOrDefault();
        }

        public List<DEPOSITO_BANCO> BuscarDadosBanco()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from s in db.DEPOSITO_BANCOs where s.STATUS == 'A' orderby s.NOME select s).ToList();
        }

        public List<DEPOSITO_BANCO> ListarNomeBanco()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from s in db.DEPOSITO_BANCOs where s.STATUS == 'A' orderby s.NOME select s).ToList();
        }

        public SP_INTRANET_ESTORNA_LANCAMENTO_LINXResult ProcessarEstornoLancamentoContabil(int _emp, int _numeroLancamento)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from s in db.SP_INTRANET_ESTORNA_LANCAMENTO_LINX(_emp, _numeroLancamento) select s).SingleOrDefault();
        }

        public SP_INTRANET_REGISTRA_LANCAMENTO_LINXResult ProcessarLancamentoContabil(LancamentoContabilEntity lanc)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from s in db.SP_INTRANET_REGISTRA_LANCAMENTO_LINX(lanc.Empresa, lanc.Filial, lanc.Data_Lancamento, lanc.Conta_Saida, lanc.Tipo_Lancamento_Saida,
                        lanc.Conta_Entrada, lanc.Tipo_Lancamento_Entrada, lanc.Valor_Deposito, lanc.Desc_Despesa, lanc.Historico_Lancamento)
                    select s).SingleOrDefault();
        }

        public SP_BUSCAR_DADOS_BANCOResult BuscarBancoPorFilial(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from s in db.SP_BUSCAR_DADOS_BANCO(filial) select s).Take(1).SingleOrDefault();
        }

        public string BuscaOrigemProduto(string codigoProduto)
        {
            string origem = "";

            PRODUTO produto = BuscaProduto(codigoProduto.Substring(0, 5));

            if (produto != null)
            {
                if (produto.COD_CATEGORIA.Equals("01"))
                {
                    /* ALTERAÇÃO 16/01/2015 - SOLICITAÇÃO EUGENIO E FABIO */
                    /* ORIGEM QUE ERA CMAX AGORA É LUGZI */
                    if (Convert.ToInt32(produto.PRODUTO1) < 30000 && produto.TIPO_PRODUTO.Trim().Equals("C-MAX"))
                    {
                        origem = "lugzi";
                        //origem = "cmax";
                    }

                    if (Convert.ToInt32(produto.PRODUTO1) < 30000 &&
                        produto.TIPO_PRODUTO.Trim().Substring(0, 3).Equals("IMP"))
                        origem = "hbf";

                    if (produto.PRODUTO1.Trim().Substring(0, 1).Equals("3") ||
                        produto.PRODUTO1.Trim().Substring(0, 1).Equals("5") ||
                        produto.PRODUTO1.Trim().Substring(0, 1).Equals("7") ||
                        produto.PRODUTO1.Trim().Substring(0, 1).Equals("9"))
                        origem = "hbf";

                    if ((produto.PRODUTO1.Trim().Substring(0, 1).Equals("4") ||
                        produto.PRODUTO1.Trim().Substring(0, 1).Equals("6") ||
                        produto.PRODUTO1.Trim().Substring(0, 1).Equals("8")) &&
                            (produto.TIPO_PRODUTO.Trim().Equals("NACIONAL") || produto.TIPO_PRODUTO.Trim().ToUpper().Contains("TERCEIRO")))
                        origem = "lugzi";

                    if ((produto.PRODUTO1.Trim().Substring(0, 1).Equals("4") && produto.TIPO_PRODUTO.Trim().Equals("C-MAX")))
                    {
                        origem = "lugzi";
                        //origem = "cmax";
                    }


                }

                if (produto.COD_CATEGORIA.Equals("02"))
                {
                    if (produto.SUBGRUPO_PRODUTO.Trim().Equals("CALCADOS"))
                        origem = "calcados";
                    else
                        origem = "outros";
                }
            }

            return origem;
        }


        public List<SP_BUSCAR_DESPESAS_DETALHEResult> BuscarDespesasDetalhe(string p_filial, string p_dataini, string p_datafim, string p_despesa)
        {
            p_dataini = AjustaData(p_dataini);
            p_datafim = AjustaData(p_datafim);

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ddet in db.SP_BUSCAR_DESPESAS_DETALHE(p_filial, p_dataini, p_datafim, p_despesa) select ddet).ToList();
        }

        public List<Sp_Busca_Pendencias_Nota_DefeitoResult> BuscaItensNotaDefeito()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from bpnd in db.Sp_Busca_Pendencias_Nota_Defeito() select bpnd).ToList();
        }

        public List<Sp_Busca_Defeito_RetornoResult> BuscaDefeitoRetorno(string griffe, string grupo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dr in db.Sp_Busca_Defeito_Retorno(griffe.Trim(), grupo.Trim()) select dr).ToList();
        }

        public NOTA_DEFEITO_RETORNO BuscaNotaDefeitoRetorno(int codigoNotaDefeitoRetorno)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ndr in db.NOTA_DEFEITO_RETORNOs where ndr.CODIGO_NOTA_DEFEITO_RETORNO == codigoNotaDefeitoRetorno select ndr).SingleOrDefault();
        }

        public COLECOE BuscaColecaoPorProduto(int produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            string colecao = (from pdr in db.PRODUTOs where pdr.PRODUTO1 == produto.ToString() select pdr.COLECAO).SingleOrDefault();

            if (colecao != null)
            {
                return (from c in db.COLECOEs where c.COLECAO.Equals(colecao) select c).SingleOrDefault();
            }

            return null;
        }

        public List<NOTA_DEFEITO_RETORNO> BuscaNotaDefeitoRetorno(string destino, string griffe, string grupo, string codigoProduto, string cor, string tamanho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ndr in db.NOTA_DEFEITO_RETORNOs where ndr.CODIGO_DESTINO == Convert.ToInt32(destino) && ndr.GRIFFE.Trim().Equals(griffe.Trim()) && ndr.GRUPO.Trim().Equals(grupo.Trim()) && ndr.CODIGO_BARRA_PRODUTO.Trim().Equals(codigoProduto.Trim() + cor.Trim() + tamanho.Trim()) && ndr.FLAG_BAIXADO == false select ndr).ToList();
        }

        public Sp_Busca_Vendas_PeriodoResult BuscaVendasPeriodo(string categoria, string colecao, string griffe, string grupo, string anoSemanaInicio, string anoSemanaFim)
        {
            string dataInicio = AjustaData(BuscaDataInicio(anoSemanaInicio));
            string dataFim = AjustaData(BuscaDataFim(anoSemanaFim));

            db = new DCDataContext(Constante.ConnectionString);

            return (from vp in db.Sp_Busca_Vendas_Periodo(categoria, colecao, griffe, grupo, dataInicio, dataFim) select vp).SingleOrDefault();
        }

        public List<SP_BUSCAR_EXTRATO_LOJAResult> BuscarExtratoLoja(string filial, DateTime dataInicio, DateTime dataFim)
        {
            string _dataInicio = AjustaData(dataInicio.ToShortDateString());
            string _dataFim = AjustaData(dataFim.ToShortDateString());

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ff in db.SP_BUSCAR_EXTRATO_LOJA(filial, _dataInicio, _dataFim) select ff).ToList();
        }

        public List<SP_BUSCAR_SALDO_FUNDO_FIXOResult> BuscarSaldoFundoFixo(string filial, DateTime dataInicio, DateTime dataFim)
        {

            string _dataInicio = AjustaData(dataInicio.ToShortDateString());
            string _dataFim = AjustaData(dataFim.ToShortDateString());

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ff in db.SP_BUSCAR_SALDO_FUNDO_FIXO(filial, _dataInicio, _dataFim) select ff).ToList();
        }

        public SP_BUSCA_VALOR_VENDAS_PERIODOResult BuscarValorVendasPeriodo(string categoria, string colecao, string griffe, string grupo, string anoSemanaInicio, string anoSemanaFim)
        {
            string dataInicio = AjustaData(BuscaDataInicio(anoSemanaInicio));
            string dataFim = AjustaData(BuscaDataFim(anoSemanaFim));

            db = new DCDataContext(Constante.ConnectionString);

            return (from vp in db.SP_BUSCA_VALOR_VENDAS_PERIODO(categoria, colecao, griffe, grupo, dataInicio, dataFim) select vp).SingleOrDefault();
        }

        public List<ContaCorrente> BuscaContaCorrente(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            List<ContaCorrente> ListaContaCorrente = new List<ContaCorrente>();

            for (DateTime data = dataInicio; data.Date > dataFim.Date; data.AddDays(1))
            {
                ContaCorrente itemContaCorrente = new ContaCorrente();

                ListaContaCorrente.Add(itemContaCorrente);
            }

            return ListaContaCorrente;
        }

        public List<FUNDO_FIXO_BAIXA> BuscarFundoFixoBaixa()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ff in db.FUNDO_FIXO_BAIXAs orderby ff.CODIGO_BAIXA ascending select ff).ToList();
        }

        public FUNDO_FIXO_BAIXA BuscarFundoFixoBaixa(int codigo_baixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ff in db.FUNDO_FIXO_BAIXAs where ff.CODIGO_BAIXA == codigo_baixa select ff).SingleOrDefault();
        }

        public List<FUNDO_FIXO_HISTORICO_FECHAMENTO> BuscaSaldosFundoFixo(int codigoFilial, DateTime dataInicio, DateTime dataFim, int codigoBaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (codigoBaixa == 0)
                return (from saff in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where saff.CODIGO_FILIAL == codigoFilial & saff.DATA >= dataInicio & saff.DATA <= dataFim orderby saff.DATA descending select saff).ToList();
            else
                return (from saff in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where saff.CODIGO_FILIAL == codigoFilial & saff.DATA >= dataInicio & saff.DATA <= dataFim & saff.CODIGO_BAIXA == codigoBaixa orderby saff.DATA descending select saff).ToList();
        }

        public FUNDO_FIXO_HISTORICO_FECHAMENTO BuscaFundoFixoHistoricoFechamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from saff in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where saff.CODIGO == codigo select saff).SingleOrDefault();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositoSemana(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_HISTORICOs
                    where ffdb.CODIGO_FILIAL == codigoFilial &
                          ffdb.DATA.Date >= dataInicio.Date &
                          ffdb.DATA.Date <= dataFim.Date &
                          ffdb.CODIGO_SALDO == 0
                    orderby ffdb.DATA
                    select ffdb).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositoSemanaCheck(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_HISTORICOs
                    where ffdb.CODIGO_FILIAL == codigoFilial &
                          ffdb.DATA.Date >= dataInicio.Date &
                          ffdb.DATA.Date <= dataFim.Date &
                          ffdb.CODIGO_SALDO == 0
                    orderby ffdb.DATA
                    select ffdb).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositoSemanaEfetuado(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_HISTORICOs
                    where ffdb.CODIGO_FILIAL == codigoFilial &
                          ffdb.DATA.Date >= dataInicio.Date &
                          ffdb.DATA.Date <= dataFim.Date
                    orderby ffdb.DATA
                    select ffdb).ToList();
        }

        public DEPOSITO_SEMANA BuscaDepositoSemana(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_SEMANAs where ffdb.CODIGO_DEPOSITO == codigo select ffdb).SingleOrDefault();
        }

        public List<DEPOSITO_DOCUMENTO> BuscaDepositoDocumento(int codigoDeposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dd in db.DEPOSITO_DOCUMENTOs where dd.CODIGO_DEPOSITO == codigoDeposito select dd).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositosRealizados(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_HISTORICOs
                    where ffdb.CODIGO_FILIAL == codigoFilial &
                          ffdb.DATA.Date >= dataInicio.Date &
                          ffdb.DATA.Date <= dataFim.Date &
                          (ffdb.CODIGO_SALDO > 0 || ffdb.VALOR_DEPOSITO == 0)
                    orderby ffdb.DATA
                    select ffdb).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositoRealizado(int codigoFilial, DateTime dataInicio)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_HISTORICOs
                    where ffdb.CODIGO_FILIAL == codigoFilial
                          && ffdb.DATA.Date < dataInicio.Date
                    select ffdb).ToList();
        }
        public DEPOSITO_HISTORICO ObterDepositoAbertoInicial(int codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffdb in db.DEPOSITO_HISTORICOs
                    where ffdb.CODIGO_FILIAL == codigoFilial
                    && ffdb.CODIGO_SALDO == 0
                    && ffdb.DATA >= DateTime.Now.AddDays(-60)
                    orderby ffdb.DATA ascending
                    select ffdb).Take(1).SingleOrDefault();
        }



        public DEPOSITO_SEMANA BuscaUltimoDeposito(int codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffds in db.DEPOSITO_SEMANAs where ffds.CODIGO_FILIAL == codigoFilial orderby ffds.CODIGO_DEPOSITO descending select ffds).Take(1).SingleOrDefault();
        }

        public List<DEPOSITO_SEMANA> BuscaDepositoNoPeriodo(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffds in db.DEPOSITO_SEMANAs where ffds.CODIGO_FILIAL == codigoFilial & ffds.DATA_FIM.Date >= dataInicio.Date & ffds.DATA_INICIO.Date <= dataFim.Date select ffds).ToList();
        }

        public List<DEPOSITO_SEMANA> BuscaDepositosRealizadosFilial(int codigoFilial, int codigoBaixa, DateTime dataAtualInicio, DateTime dataAtualFim)
        {
            string data = "01/04/2014";

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffds in db.DEPOSITO_SEMANAs
                    where ffds.CODIGO_BAIXA == codigoBaixa
                    && ffds.CODIGO_FILIAL == codigoFilial
                    && ffds.DATA_INICIO > Convert.ToDateTime(data)
                    && ffds.DATA_FIM.Date >= dataAtualInicio.Date
                    && ffds.DATA_FIM.Date <= dataAtualFim.Date
                    orderby ffds.CODIGO_FILIAL, ffds.DATA_INICIO descending
                    select ffds).ToList();

        }

        public List<DEPOSITO_SEMANA> BuscaDepositosRealizadosFilial(int codigoFilial, int codigoBaixa)
        {
            string data = "01/04/2014";

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (codigoFilial == 0)
                return (from ffds in db.DEPOSITO_SEMANAs where ffds.CODIGO_BAIXA == codigoBaixa & ffds.DATA_INICIO > Convert.ToDateTime(data) orderby ffds.CODIGO_FILIAL, ffds.DATA_INICIO descending select ffds).ToList();
            else
                return (from ffds in db.DEPOSITO_SEMANAs where ffds.CODIGO_FILIAL == codigoFilial & ffds.CODIGO_BAIXA == codigoBaixa & ffds.DATA_INICIO > Convert.ToDateTime(data) orderby ffds.DATA_INICIO descending select ffds).ToList();
        }

        public List<DEPOSITO_SEMANA> BuscaDepositosRealizados(int codigoBaixa, string filial, DateTime dataInicio, DateTime dataFim)
        {
            string data = "01/04/2014";

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffds in db.DEPOSITO_SEMANAs
                    where ffds.CODIGO_BAIXA == codigoBaixa && ffds.DATA_INICIO > Convert.ToDateTime(data)
                    && ffds.CODIGO_FILIAL == Convert.ToInt32(filial) && ffds.DATA_INICIO.Date >= dataInicio.Date && ffds.DATA_FIM.Date <= dataFim.Date
                    orderby ffds.CODIGO_FILIAL, ffds.DATA_INICIO
                    descending
                    select ffds).ToList();
        }

        public List<DEPOSITO_SEMANA> BuscaDepositosRealizados(int codigoBaixa)
        {
            string data = "01/04/2014";

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffds in db.DEPOSITO_SEMANAs where ffds.CODIGO_BAIXA == codigoBaixa & ffds.DATA_INICIO > Convert.ToDateTime(data) orderby ffds.CODIGO_FILIAL, ffds.DATA_INICIO descending select ffds).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaCofreNoPeriodo(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            DEPOSITO_SEMANA ultimoDeposito = BuscaUltimoDeposito(codigoFilial);

            if (ultimoDeposito != null)
            {
                db = new DCDataContext(Constante.ConnectionStringIntranet);

                //return (from ffhb in db.DEPOSITO_HISTORICOs where ffhb.CODIGO_FILIAL == codigoFilial & ffhb.DATA.Date > ultimoDeposito.DATA_FIM.Date select ffhb).ToList();
                return (from ffhb in db.DEPOSITO_HISTORICOs where ffhb.CODIGO_FILIAL == codigoFilial & ffhb.DATA.Date >= dataInicio & ffhb.DATA.Date <= dataFim & ffhb.CODIGO_SALDO == 0 select ffhb).ToList();
            }

            return null;
        }

        public List<DEPOSITO_DOCUMENTO> BuscaLancamentosLinx(int codigoBaixa, string filial, DateTime dataInicio, DateTime dataFim)
        {
            string data = "01/04/2014";

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.DEPOSITO_DOCUMENTOs
                    join f in db.DEPOSITO_SEMANAs
                    on d.CODIGO_DEPOSITO equals f.CODIGO_DEPOSITO
                    where f.CODIGO_BAIXA == codigoBaixa &&
                            f.DATA_INICIO > Convert.ToDateTime(data) &&
                            f.CODIGO_FILIAL == Convert.ToInt32(filial) &&
                            f.DATA_INICIO.Date >= dataInicio.Date &&
                            f.DATA_FIM.Date <= dataFim.Date
                    orderby f.CODIGO_FILIAL, f.DATA_INICIO
                    descending
                    select d).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_DESPESA> BuscaDespesasFechamento(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dff in db.FUNDO_FIXO_HISTORICO_DESPESAs where dff.CODIGO_FECHAMENTO == Convert.ToInt32(codigo) select dff).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_DESPESA> BuscaDespesasFechamentoSemLancamento(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dff in db.FUNDO_FIXO_HISTORICO_DESPESAs where dff.CODIGO_FECHAMENTO == Convert.ToInt32(codigo) && dff.NUMERO_LANCAMENTO == null select dff).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_RECEITA> BuscaReceitasFechamento(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dff in db.FUNDO_FIXO_HISTORICO_RECEITAs where dff.CODIGO_FECHAMENTO == Convert.ToInt32(codigo) select dff).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_RECEITA> BuscaReceitasFechamentoSemLancamento(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dff in db.FUNDO_FIXO_HISTORICO_RECEITAs where dff.CODIGO_FECHAMENTO == Convert.ToInt32(codigo) && dff.NUMERO_LANCAMENTO == null select dff).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_DESPESA> BuscaDespesasFundoFixo(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dff in db.FUNDO_FIXO_HISTORICO_DESPESAs where dff.CODIGO_FILIAL == codigoFilial & dff.DATA >= dataInicio & dff.DATA <= dataFim select dff).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_RECEITA> BuscaReceitasFundoFixo(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from rff in db.FUNDO_FIXO_HISTORICO_RECEITAs where rff.CODIGO_FILIAL == codigoFilial & rff.DATA > dataInicio & rff.DATA < dataFim select rff).ToList();
        }
        /*
                public List<FUNDO_FIXO_HISTORICO_FECHAMENTO> BuscaSaldoAtualFundoFixo(int codigoFilial, DateTime dataFim)
                {
                    db = new DCDataContext(Constante.ConnectionStringIntranet);

                    return (from saff in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where saff.CODIGO_FILIAL == codigoFilial & saff.DATA > dataFim select saff).ToList();
                }
        */
        public List<NOTA_TRANSFERENCIA> BuscaNotasTransferencia(int CODIGO_LOJA, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nt in db.NOTA_TRANSFERENCIAs where nt.CODIGO_FILIAL == CODIGO_LOJA & nt.DATA_INICIO < dataFim & nt.DATA_INICIO > dataInicio & nt.DATA_FIM == null orderby nt.DATA_INICIO ascending select nt).ToList();
        }

        public NOTA_TRANSFERENCIA BuscaNotaTransferencia(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nt in db.NOTA_TRANSFERENCIAs where nt.CODIGO == codigoNotaTransferencia select nt).SingleOrDefault();
        }

        public List<Sp_Busca_Nota_Transferencia_ItemResult> BuscaNotasTransferenciaItemAgrupado(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nti in db.Sp_Busca_Nota_Transferencia_Item(codigoNotaTransferencia) select nti).ToList();
        }

        public List<p_busca_mais_vendidosResult> BuscaMaisVendidos(string anoSemana, string codCategoria, string colecao, string codigoFilial)
        {
            string dataInicioSemana = AjustaData(BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(BuscaDataFim(anoSemana));

            db = new DCDataContext(Constante.ConnectionStringJonas);

            return (from mv in db.p_busca_mais_vendidos(dataInicioSemana, dataFimSemana, codCategoria, colecao, codigoFilial) select mv).ToList();
        }

        public List<SP_OBTER_MAIS_VENDIDOSResult> ObterMaisVendidos(DateTime dataIni, DateTime dataFim, string colecao, string codigoFilial, string griffe)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from mv in dbLinx.SP_OBTER_MAIS_VENDIDOS(dataIni, dataFim, codigoFilial, colecao, griffe) select mv).ToList();
        }

        public List<SP_OBTER_MAIS_VENDIDOS_LOJAResult> ObterMaisVendidosLoja(DateTime dataIni, DateTime dataFim, string colecao, string codigoFilial, string griffe)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return (from mv in dbLinx.SP_OBTER_MAIS_VENDIDOS_LOJA(dataIni, dataFim, codigoFilial, colecao, griffe) select mv).ToList();
        }

        public List<p_busca_mais_vendidos_lojaResult> BuscaMaisVendidosLoja(string anoSemana, string categoria, string colecao, string filial)
        {
            string dataInicioSemana = AjustaData(BuscaDataInicio(anoSemana));
            string dataFimSemana = AjustaData(BuscaDataFim(anoSemana));

            db = new DCDataContext(Constante.ConnectionStringJonas);

            return (from mv in db.p_busca_mais_vendidos_loja(dataInicioSemana, dataFimSemana, categoria, colecao, filial) select mv).ToList();
        }

        public List<NOTA_TRANSFERENCIA_ITEM> BuscaNotasTransferenciaItem(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nti in db.NOTA_TRANSFERENCIA_ITEMs where nti.CODIGO_NOTA_TRANSFER == codigoNotaTransferencia select nti).ToList();
        }

        public NOTA_RETIRADA_ITEM BuscaNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nti in db.NOTA_RETIRADA_ITEMs where nti.CODIGO_NOTA_RETIRADA_ITEM == codigoNotaRetiradaItem select nti).SingleOrDefault();
        }

        public NOTA_RETIRADA_DESTINO BuscaNotaRetiradaDestino(int codigoNotaRetiradaDestino)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nrd in db.NOTA_RETIRADA_DESTINOs where nrd.CODIGO == codigoNotaRetiradaDestino select nrd).SingleOrDefault();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaItens(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nti in db.NOTA_RETIRADA_ITEMs where nti.CODIGO_NOTA_RETIRADA == codigoNotaRetirada select nti).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaItensSemRetorno()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nti in db.NOTA_RETIRADA_ITEMs where nti.DATA_RETORNO == null && nti.CODIGO_DESTINO != null select nti).ToList();
        }

        public List<IMAGEM> BuscaComprovantes()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.IMAGEMs where i.ATIVO == true orderby i.DATA_DIGITADA ascending select i).ToList();
        }

        public COTAS_SEMANA BuscaCotasSemana(int COLECAO, int CATEGORIA, string GRIFFE, string GRUPO, int ANO_SEMANA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cs in db.COTAS_SEMANAs
                    where cs.COLECAO == COLECAO &
                          cs.CATEGORIA == CATEGORIA &
                          cs.GRIFFE.Equals(GRIFFE) &
                          cs.GRUPO.Equals(GRUPO) &
                          cs.ANO_SEMANA == ANO_SEMANA
                    select cs).SingleOrDefault();
        }

        public int BuscaCotasAcumuladaQtde(string categoria, string colecao, string grupo, string griffe, int anoSemanaInicio, int anoSemanaFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            int? qtde = (from cs in db.COTAS_SEMANAs
                         where cs.COLECAO.Equals(colecao) &
                               cs.CATEGORIA.Equals(categoria) &
                               cs.GRIFFE.Equals(griffe) &
                               cs.GRUPO.Equals(grupo) &
                               cs.ANO_SEMANA >= anoSemanaInicio &
                               cs.ANO_SEMANA <= anoSemanaFim
                         select cs.QTDE).Sum();

            if (qtde == null)
                qtde = 0;

            return Convert.ToInt32(qtde);
        }

        public decimal BuscaCotasAcumuladaValor(string categoria, string colecao, string grupo, string griffe, int anoSemanaInicio, int anoSemanaFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            decimal? valor_medio = (from cs in db.COTAS_SEMANAs
                                    where cs.COLECAO.Equals(colecao) &
                                          cs.CATEGORIA.Equals(categoria) &
                                          cs.GRIFFE.Equals(griffe) &
                                          cs.GRUPO.Equals(grupo) &
                                          cs.ANO_SEMANA >= anoSemanaInicio &
                                          cs.ANO_SEMANA <= anoSemanaFim
                                    select cs.PRECO_MEDIO).FirstOrDefault();

            if (valor_medio == null)
                valor_medio = 0;

            return Convert.ToDecimal(valor_medio);
        }

        public int BuscaQtdeCotasMes(int COLECAO, int CATEGORIA, string GRIFFE, string GRUPO, int ANO_SEMANA)
        {
            int qtde = 0;

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            COTAS_SEMANA cotasSemana = (from cs in db.COTAS_SEMANAs
                                        where cs.COLECAO == COLECAO &
                                              cs.CATEGORIA == CATEGORIA &
                                              cs.GRIFFE.Equals(GRIFFE) &
                                              cs.GRUPO.Equals(GRUPO) &
                                              cs.ANO_SEMANA == ANO_SEMANA
                                        select cs).SingleOrDefault();
            if (cotasSemana != null)
            {
                int ano = Convert.ToInt32(cotasSemana.ANO_SEMANA.ToString().Substring(0, 4));
                int mes = Convert.ToInt32(cotasSemana.MES.ToString());

                int? qtdeCotasSemana = (from cs in db.COTAS_SEMANAs
                                        where cs.COLECAO == COLECAO &
                                              cs.CATEGORIA == CATEGORIA &
                                              cs.GRIFFE.Equals(GRIFFE) &
                                              cs.GRUPO.Equals(GRUPO) &
                                              cs.ANO == ano &
                                              cs.MES == mes
                                        select cs.QTDE).Sum();

                if (qtdeCotasSemana != null)
                    qtde = Convert.ToInt32(qtdeCotasSemana);
            }

            return qtde;
        }

        public decimal BuscaValorCotasMes(int COLECAO, int CATEGORIA, string GRIFFE, string GRUPO, int ANO_SEMANA)
        {
            decimal valor = 0;

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            COTAS_SEMANA cotasSemana = (from cs in db.COTAS_SEMANAs
                                        where cs.COLECAO == COLECAO &
                                              cs.CATEGORIA == CATEGORIA &
                                              cs.GRIFFE.Equals(GRIFFE) &
                                              cs.GRUPO.Equals(GRUPO) &
                                              cs.ANO_SEMANA == ANO_SEMANA
                                        select cs).SingleOrDefault();
            if (cotasSemana != null)
            {
                int ano = Convert.ToInt32(cotasSemana.ANO_SEMANA.ToString().Substring(0, 4));
                int mes = Convert.ToInt32(cotasSemana.MES.ToString());

                decimal? valorCotasSemana = (from cs in db.COTAS_SEMANAs
                                             where cs.COLECAO == COLECAO &
                                                   cs.CATEGORIA == CATEGORIA &
                                                   cs.GRIFFE.Equals(GRIFFE) &
                                                   cs.GRUPO.Equals(GRUPO) &
                                                   cs.ANO == ano &
                                                   cs.MES == mes
                                             select cs.VALOR).Sum();

                if (valorCotasSemana != null)
                    valor = Convert.ToDecimal(valorCotasSemana);
            }

            return valor;
        }

        public IMAGEM_PRODUTO BuscaImagemProduto(int codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ip in db.IMAGEM_PRODUTOs where ip.CODIGO_PRODUTO == codigoProduto & ip.ATIVO == true select ip).Take(1).SingleOrDefault();
        }

        public List<USUARIOLOJA> BuscaUsuarioLoja(USUARIO usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (usuario.CODIGO_PERFIL == 1 || usuario.CODIGO_PERFIL == 11)
                return (from ul in db.USUARIOLOJAs select ul).ToList();
            else
                return (from ul in db.USUARIOLOJAs where ul.CODIGO_USUARIO == usuario.CODIGO_USUARIO select ul).ToList();
        }

        public List<FECHAMENTO_CAIXA> BuscaFechamentosCaixa(DateTime dataInicio, DateTime dataFim, int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (filial == 0)
                return (from c in db.FECHAMENTO_CAIXAs where c.DATA_FECHAMENTO.Date >= dataInicio.Date & c.DATA_FECHAMENTO.Date <= dataFim.Date orderby c.DATA_FECHAMENTO ascending select c).ToList();
            else
                return (from c in db.FECHAMENTO_CAIXAs where c.DATA_FECHAMENTO.Date >= dataInicio.Date & c.DATA_FECHAMENTO.Date <= dataFim.Date & c.CODIGO_FILIAL == filial orderby c.DATA_FECHAMENTO ascending select c).ToList();
        }

        public List<ACOMPANHAMENTO_ALUGUEL> BuscaAlugueis(int codigoFilial, DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from a in db.ACOMPANHAMENTO_ALUGUELs where a.CODIGO_FILIAL == codigoFilial & a.DATA_VENCIMENTO.Date == data.Date & a.STATUS == 0 orderby a.CODIGO_MESANO ascending select a).ToList();
        }
        public List<ACOMPANHAMENTO_ALUGUEL> BuscaAlugueis(int codigoFilial, int codigoReferencia, int status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from a in db.ACOMPANHAMENTO_ALUGUELs where a.CODIGO_FILIAL == codigoFilial & a.CODIGO_MESANO == codigoReferencia & a.STATUS == status orderby a.DATA_VENCIMENTO ascending select a).ToList();
        }
        public List<ACOMPANHAMENTO_ALUGUEL> BuscaAlugueisPorCompetencia(int codigoFilial, DateTime competencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from a in db.ACOMPANHAMENTO_ALUGUELs where a.CODIGO_FILIAL == codigoFilial & a.COMPETENCIA == competencia select a).ToList();
        }


        public FECHAMENTO_CAIXA BuscaFechamentoCaixa(DateTime data, int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CAIXAs where c.DATA_FECHAMENTO.Date == data.Date & c.CODIGO_FILIAL == filial orderby c.DATA_FECHAMENTO ascending select c).SingleOrDefault();
        }

        public List<DivergenciaFechamento> BuscaDivergenciasFechamento(DateTime dataInicio, DateTime dataFim)
        {
            List<DivergenciaFechamento> divergencias = new List<DivergenciaFechamento>();

            List<FILIAI> filiais = BuscaFiliais();

            if (filiais != null)
            {
                foreach (FILIAI filial in filiais)
                {
                    for (DateTime data = dataInicio; data <= dataFim; data = data.AddDays(1))
                    {
                        DivergenciaFechamento divergencia = new DivergenciaFechamento();

                        divergencia.Filial = filial.FILIAL;

                        FECHAMENTO_CAIXA fechamento = BuscaFechamentoCaixa(data, Convert.ToInt32(filial.COD_FILIAL));

                        SP_BUSCA_PENDENCIAS_FECHAMENTOResult pendencia = BuscaPendenciaFechamento(filial.FILIAL, AjustaData(data.Date.ToString()));

                        if (fechamento != null && pendencia != null)
                        {
                            divergencia.Data = data;
                            divergencia.ValorComandaIntranet = Convert.ToDecimal(fechamento.VALOR_COMANDA);

                            if (fechamento.VALOR_DINHEIRO == null)
                                fechamento.VALOR_DINHEIRO = 0;

                            if (fechamento.VALOR_CARTAO_CREDITO == null)
                                fechamento.VALOR_CARTAO_CREDITO = 0;

                            if (fechamento.VALOR_CHEQUE == null)
                                fechamento.VALOR_CHEQUE = 0;

                            if (fechamento.VALOR_CHEQUE_PRE == null)
                                fechamento.VALOR_CHEQUE_PRE = 0;

                            divergencia.ValorDinheiroIntranet = Convert.ToDecimal(fechamento.VALOR_DINHEIRO);
                            divergencia.ValorCartaoIntranet = Convert.ToDecimal(fechamento.VALOR_CARTAO_CREDITO);
                            divergencia.ValorOutrosIntranet = Convert.ToDecimal(fechamento.VALOR_CHEQUE) + Convert.ToDecimal(fechamento.VALOR_CHEQUE_PRE);

                            if (pendencia.DINHEIRO == null)
                                pendencia.DINHEIRO = 0;

                            divergencia.ValorDinheiroLinx = Convert.ToDecimal(pendencia.DINHEIRO);

                            if (pendencia.CARTAO == null)
                                pendencia.CARTAO = 0;

                            divergencia.ValorCartaoLinx = Convert.ToDecimal(pendencia.CARTAO);

                            if (pendencia.OUTROS_MEIOS == null)
                                pendencia.OUTROS_MEIOS = 0;

                            divergencia.ValorOutrosLinx = Convert.ToDecimal(pendencia.OUTROS_MEIOS);

                            decimal variacaoDinheiro = (divergencia.ValorDinheiroLinx * 2) / 100;
                            decimal variacaoDinheiroMais = divergencia.ValorDinheiroLinx + variacaoDinheiro;
                            decimal variacaoDinheiroMenos = divergencia.ValorDinheiroLinx - variacaoDinheiro;

                            decimal variacaoCartao = (divergencia.ValorCartaoLinx * 2) / 100;
                            decimal variacaoCartaoMais = divergencia.ValorCartaoLinx + variacaoCartao;
                            decimal variacaoCartaoMenos = divergencia.ValorCartaoLinx - variacaoCartao;

                            if (fechamento.CONFERIDO == null)
                            {
                                decimal varDinheiro = divergencia.ValorDinheiroIntranet - divergencia.ValorDinheiroLinx;
                                decimal varCartao = divergencia.ValorCartaoIntranet - divergencia.ValorCartaoLinx;
                                decimal varOutros = divergencia.ValorOutrosIntranet - divergencia.ValorOutrosLinx;

                                if (divergencia.ValorDinheiroIntranet < variacaoDinheiroMenos ||
                                    divergencia.ValorDinheiroIntranet > variacaoDinheiroMais ||
                                    divergencia.ValorCartaoIntranet < variacaoCartaoMenos ||
                                    divergencia.ValorCartaoIntranet > variacaoCartaoMais ||
                                    varDinheiro > 5 ||
                                    varDinheiro < -5 ||
                                    varCartao > 5 ||
                                    varCartao < -5 ||
                                    varOutros > 5 ||
                                    varOutros < -5)
                                    divergencias.Add(divergencia);
                            }
                        }
                        else
                        {
                            if (fechamento == null && pendencia != null)
                            {
                                divergencia.Data = data;
                                divergencia.ValorComandaIntranet = 0;

                                if (pendencia.DINHEIRO == null)
                                    pendencia.DINHEIRO = 0;

                                divergencia.ValorDinheiroLinx = Convert.ToDecimal(pendencia.DINHEIRO);

                                if (pendencia.CARTAO == null)
                                    pendencia.CARTAO = 0;

                                divergencia.ValorCartaoLinx = Convert.ToDecimal(pendencia.CARTAO);

                                if (pendencia.OUTROS_MEIOS == null)
                                    pendencia.OUTROS_MEIOS = 0;

                                divergencia.ValorOutrosLinx = Convert.ToDecimal(pendencia.OUTROS_MEIOS);

                                divergencia.ValorDinheiroIntranet = 0;
                                divergencia.ValorCartaoIntranet = 0;
                                divergencia.ValorOutrosIntranet = 0;

                                divergencias.Add(divergencia);
                            }
                            else
                                if (fechamento != null && pendencia == null)
                            {
                                divergencia.Data = data;
                                divergencia.ValorComandaIntranet = Convert.ToDecimal(fechamento.VALOR_COMANDA);

                                if (fechamento.VALOR_DINHEIRO == null)
                                    fechamento.VALOR_DINHEIRO = 0;

                                if (fechamento.VALOR_CARTAO_CREDITO == null)
                                    fechamento.VALOR_CARTAO_CREDITO = 0;

                                if (fechamento.VALOR_CHEQUE == null)
                                    fechamento.VALOR_CHEQUE = 0;

                                if (fechamento.VALOR_CHEQUE_PRE == null)
                                    fechamento.VALOR_CHEQUE_PRE = 0;

                                divergencia.ValorDinheiroIntranet = Convert.ToDecimal(fechamento.VALOR_DINHEIRO);
                                divergencia.ValorCartaoIntranet = Convert.ToDecimal(fechamento.VALOR_CARTAO_CREDITO);
                                divergencia.ValorOutrosIntranet = Convert.ToDecimal(fechamento.VALOR_CHEQUE) + Convert.ToDecimal(fechamento.VALOR_CHEQUE_PRE);

                                divergencia.ValorDinheiroLinx = 0;
                                divergencia.ValorCartaoLinx = 0;
                                divergencia.ValorOutrosLinx = 0;

                                if (fechamento.CONFERIDO == null)
                                    divergencias.Add(divergencia);
                                else
                                {
                                    if (!Convert.ToBoolean(fechamento.CONFERIDO))
                                        divergencias.Add(divergencia);
                                }
                            }
                            else
                                    if (fechamento == null && pendencia == null)
                            {
                                divergencia.Data = data;
                                divergencia.ValorComandaIntranet = 0;
                                divergencia.ValorDinheiroIntranet = 0;
                                divergencia.ValorCartaoIntranet = 0;
                                divergencia.ValorOutrosIntranet = 0;
                                divergencia.ValorDinheiroLinx = 0;
                                divergencia.ValorCartaoLinx = 0;
                                divergencia.ValorOutrosLinx = 0;

                                divergencias.Add(divergencia);
                            }
                        }
                    }
                }
            }

            return divergencias;
        }

        public List<NOTA_RETIRADA> BuscaNotasRetiradaFechada(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nr in db.NOTA_RETIRADAs
                    where nr.CODIGO_FILIAL == filial & (nr.NUMERO_NOTA_CMAX != null ||
                                                        nr.NUMERO_NOTA_HBF != null ||
                                                        nr.NUMERO_NOTA_CALCADOS != null ||
                                                        nr.NUMERO_NOTA_OUTROS != null ||
                                                        nr.NUMERO_NOTA_LUGZI != null)
                    orderby nr.CODIGO_NOTA_RETIRADA descending
                    select nr).ToList();
        }

        public List<NOTA_RETIRADA> BuscaNotasRetiradaEmAberto(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nr in db.NOTA_RETIRADAs
                    where nr.CODIGO_FILIAL == filial &
                         (nr.NUMERO_NOTA_CMAX == null ||
                          nr.NUMERO_NOTA_CMAX.Equals("") ||
                          nr.NUMERO_NOTA_HBF == null ||
                          nr.NUMERO_NOTA_HBF.Equals("") ||
                          nr.NUMERO_NOTA_CALCADOS == null ||
                          nr.NUMERO_NOTA_CALCADOS.Equals("") == null ||
                          nr.NUMERO_NOTA_OUTROS == null ||
                          nr.NUMERO_NOTA_OUTROS.Equals("") ||
                          nr.NUMERO_NOTA_LUGZI == null ||
                          nr.NUMERO_NOTA_LUGZI.Equals(""))
                    select nr).ToList();
        }

        public List<Sp_Busca_Nf_DefeitoResult> BuscaNotaRetiradaItemEmAberto(string defeito, string destino, string data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from bnfd in db.Sp_Busca_Nf_Defeito(defeito, destino, data) orderby bnfd.codigo_produto, bnfd.data_destino select bnfd).ToList();
        }
        /*
        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaItemEmAberto(int notaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nr in db.NOTA_RETIRADA_ITEMs where nr.CODIGO_NOTA_RETIRADA == notaRetirada select nr).ToList();
        }
        */
        public SP_BUSCA_PENDENCIAS_FECHAMENTOResult BuscaPendenciaFechamento(string filial, string data)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pf in db.SP_BUSCA_PENDENCIAS_FECHAMENTO(filial, data) select pf).SingleOrDefault();
        }

        public List<Sp_Busca_Nf_Datas_RetornoResult> BuscaDatasRetorno()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dr in db.Sp_Busca_Nf_Datas_Retorno() select dr).ToList();
        }

        public List<Sp_Depesas_CaixaResult> BuscaDespesasCaixa(string dataInicio, string dataFim, string filial, string despesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from dc in db.Sp_Depesas_Caixa(dataInicio, dataFim, filial, despesa) select dc).ToList();
        }

        public List<FECHAMENTO_CAIXA> BuscaReceitasCaixa(DateTime dataInicio, DateTime dataFim, int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CAIXAs where (c.CODIGO_FILIAL == filial || filial == 0) & c.DATA_FECHAMENTO > dataInicio & c.DATA_FECHAMENTO < dataFim orderby c.CODIGO_FILIAL select c).ToList();
        }

        public List<FECHAMENTO_CAIXA> ExisteMovimentoCaixa(DateTime data, int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CAIXAs where c.DATA_FECHAMENTO.Date == data.Date & c.CODIGO_FILIAL == filial select c).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_FECHAMENTO> ExisteFechamentoCaixa(DateTime data, int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffhf in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where ffhf.DATA == data.Date & ffhf.CODIGO_FILIAL == filial select ffhf).ToList();
        }

        public List<USUARIO> BuscaUsuarioPerfil(int perfil)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from u in db.USUARIOs where u.CODIGO_PERFIL == perfil select u).ToList();
        }

        public USUARIOLOJA BuscaUsuarioLoja(int codigoUsuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ul in db.USUARIOLOJAs where ul.CODIGO_USUARIO == codigoUsuario select ul).Take(1).SingleOrDefault();
        }

        public LOJA_VENDEDORE BuscaGerenteLoja(string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where lv.CODIGO_FILIAL.Equals(filial) & lv.GERENTE.Equals("1") & lv.DATA_DESATIVACAO == null & lv.DESC_CARGO.Equals("GERENTE") orderby lv.NOME_VENDEDOR ascending select lv).Take(1).SingleOrDefault();
        }

        public Sp_Busca_Supervisor_LojaResult BuscaSupervisorLoja(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from sl in db.Sp_Busca_Supervisor_Loja(filial) select sl).Take(1).SingleOrDefault();
        }

        public List<USUARIO> BuscaSupervisores()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from s in db.USUARIOs where s.CODIGO_PERFIL == 3 select s).ToList();
        }

        public USUARIO BuscaUsuario(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from u in db.USUARIOs where u.CODIGO_USUARIO == codigo select u).SingleOrDefault();
        }

        public List<NOTA_RETIRADA_DEFEITO> BuscaDefeitos()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.NOTA_RETIRADA_DEFEITOs select d).ToList();
        }

        public NOTA_RETIRADA_DEFEITO BuscaDefeito(string codigoDefeito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.NOTA_RETIRADA_DEFEITOs where d.CODIGO == Convert.ToInt32(codigoDefeito) select d).SingleOrDefault();
        }

        public List<NOTA_RETIRADA_DESTINO> BuscaDestinos()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.NOTA_RETIRADA_DESTINOs select d).ToList();
        }

        public NOTA_RETIRADA_DESTINO BuscaDestino(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.NOTA_RETIRADA_DESTINOs where d.DESCRICAO_DESTINO.Equals(descricao) select d).SingleOrDefault();
        }

        public List<NOTA_RETIRADA_ORIGEM_DEFEITO> BuscaOrigensDefeito()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from od in db.NOTA_RETIRADA_ORIGEM_DEFEITOs select od).ToList();
        }

        public NOTA_RETIRADA_ORIGEM_DEFEITO BuscaOrigemDefeito(string codigoOrigemDefeito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from od in db.NOTA_RETIRADA_ORIGEM_DEFEITOs where od.CODIGO == Convert.ToInt32(codigoOrigemDefeito) select od).SingleOrDefault();
        }

        public SEMANACOLECAO BuscaColecao(string semanaAno)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from sc in db.SEMANACOLECAOs where sc.ANO_SEMANA.Equals(semanaAno) & sc.DESCRICAO_COLECAO.Equals("LANÇAMENTO") select sc).SingleOrDefault();
        }

        public List<LOJA_VENDA> BuscaVendasPeriodo(string filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDAs where lv.CODIGO_FILIAL.Equals(filial) & lv.DATA_VENDA.Date >= dataInicio.Date & lv.DATA_VENDA.Date <= dataFim.Date orderby lv.VENDEDOR select lv).ToList();
        }

        public LOJA_VENDA BuscaVendaTicket(string ticket)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDAs where lv.TICKET.Equals(ticket) select lv).SingleOrDefault();
        }

        public List<Sp_Busca_Produto_Venda_TicketResult> BuscaItensVenda(string filial, string ticket)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lvp in db.Sp_Busca_Produto_Venda_Ticket(filial, ticket) select lvp).ToList();
        }

        public List<Sp_Busca_Produto_Troca_TicketResult> BuscaItensTroca(string filial, string ticket)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lvt in db.Sp_Busca_Produto_Troca_Ticket(filial, ticket) select lvt).ToList();
        }

        public List<Sp_Partes_ProdutoResult> BuscaPartesProduto(string filial, string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.Sp_Partes_Produto(filial, dataInicio, dataFim) select pp).ToList();
        }

        public List<Sp_Busca_Draft_FinalResult> BuscaDraftFinal(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from df in db.Sp_Busca_Draft_Final(colecao) select df).ToList();
        }

        public List<Sp_Busca_Produtos_Pecas_ColecaoResult> BuscaProdutoPecaColecao(string colecao, string griffe, string grupo)
        {
            string subCategoria = "Original " + colecao;

            db = new DCDataContext(Constante.ConnectionString);

            return (from ppc in db.Sp_Busca_Produtos_Pecas_Colecao(colecao, griffe, subCategoria, grupo) select ppc).ToList();
        }

        public List<Importacao> BuscaDivergenciaFob(List<Sp_Busca_Produtos_Pecas_ColecaoResult> listaProdutosLinx, List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto)
        {
            List<Importacao> listaDivergenciaFob = new List<Importacao>();

            if (listaProformaProduto.Count > 0)
            {
                foreach (Sp_Busca_Produtos_Pecas_ColecaoResult itemLinx in listaProdutosLinx)
                {
                    Boolean ok = false;
                    decimal preco = 0;

                    foreach (IMPORTACAO_PROFORMA_PRODUTO itemProforma in listaProformaProduto)
                    {
                        if (Convert.ToInt32(itemLinx.CODIGO_PRODUTO) == itemProforma.CODIGO_PRODUTO &
                            Convert.ToInt32(itemLinx.CODIGO_PRODUTO_COR) == itemProforma.CODIGO_PRODUTO_COR &
                            Convert.ToDecimal(itemLinx.PRECO) != itemProforma.FOB)
                        {
                            ok = true;
                            preco = itemProforma.FOB;

                            break;
                        }
                    }

                    if (ok)
                    {
                        Importacao importacao = new Importacao();

                        importacao.CodigoProduto = Convert.ToInt32(itemLinx.CODIGO_PRODUTO);

                        PRODUTO produto = BuscaProduto(itemLinx.CODIGO_PRODUTO);

                        if (produto != null)
                            importacao.DescricaoProduto = produto.DESC_PRODUTO;

                        PRODUTO_CORE produtoCor = BuscaProdutoCorCodigo(itemLinx.CODIGO_PRODUTO, itemLinx.CODIGO_PRODUTO_COR);

                        if (produtoCor != null)
                            importacao.DescricaoProdutoCor = produtoCor.DESC_COR_PRODUTO;

                        importacao.FobLinx = itemLinx.PRECO.ToString();
                        importacao.FobIntranet = preco.ToString();

                        listaDivergenciaFob.Add(importacao);
                    }
                }

                return listaDivergenciaFob;
            }
            else
                return listaDivergenciaFob;
        }

        public List<Importacao> BuscaDivergenciaLinx(List<Sp_Busca_Produtos_Pecas_ColecaoResult> listaProdutosLinx, List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto)
        {
            List<Importacao> listaDivergenciaLinx = new List<Importacao>();

            if (listaProformaProduto.Count > 0)
            {
                foreach (Sp_Busca_Produtos_Pecas_ColecaoResult itemLinx in listaProdutosLinx)
                {
                    Boolean ok = true;

                    foreach (IMPORTACAO_PROFORMA_PRODUTO itemProforma in listaProformaProduto)
                    {
                        if (Convert.ToInt32(itemLinx.CODIGO_PRODUTO) == itemProforma.CODIGO_PRODUTO &
                            Convert.ToInt32(itemLinx.CODIGO_PRODUTO_COR) == itemProforma.CODIGO_PRODUTO_COR)
                        {
                            ok = false;

                            break;
                        }
                    }

                    if (ok)
                    {
                        Importacao importacao = new Importacao();

                        importacao.CodigoProduto = Convert.ToInt32(itemLinx.CODIGO_PRODUTO);

                        PRODUTO produto = BuscaProduto(itemLinx.CODIGO_PRODUTO);

                        if (produto != null)
                            importacao.DescricaoProduto = produto.DESC_PRODUTO;

                        PRODUTO_CORE produtoCor = BuscaProdutoCorCodigo(itemLinx.CODIGO_PRODUTO, itemLinx.CODIGO_PRODUTO_COR);

                        if (produtoCor != null)
                            importacao.DescricaoProdutoCor = produtoCor.DESC_COR_PRODUTO;

                        importacao.FobLinx = itemLinx.PRECO.ToString();

                        listaDivergenciaLinx.Add(importacao);
                    }
                }

                return listaDivergenciaLinx;
            }
            else
                return listaDivergenciaLinx;
        }

        public List<Importacao> BuscaDivergenciaIntranet(List<Sp_Busca_Produtos_Pecas_ColecaoResult> listaProdutosLinx, List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto)
        {
            List<Importacao> listaDivergenciaIntranet = new List<Importacao>();

            if (listaProdutosLinx.Count > 0)
            {
                foreach (IMPORTACAO_PROFORMA_PRODUTO itemIntranet in listaProformaProduto)
                {
                    Boolean ok = true;

                    foreach (Sp_Busca_Produtos_Pecas_ColecaoResult itemLinx in listaProdutosLinx)
                    {
                        if (itemIntranet.CODIGO_PRODUTO == Convert.ToInt32(itemLinx.CODIGO_PRODUTO) &
                            itemIntranet.CODIGO_PRODUTO_COR == Convert.ToInt32(itemLinx.CODIGO_PRODUTO_COR))
                        {
                            ok = false;

                            break;
                        }
                    }

                    if (ok)
                    {
                        Importacao importacao = new Importacao();

                        importacao.CodigoProduto = Convert.ToInt32(itemIntranet.CODIGO_PRODUTO);

                        PRODUTO produto = BuscaProduto(itemIntranet.CODIGO_PRODUTO.ToString());

                        if (produto != null)
                            importacao.DescricaoProduto = produto.DESC_PRODUTO;

                        PRODUTO_CORE produtoCor = BuscaProdutoCorCodigo(itemIntranet.CODIGO_PRODUTO.ToString(), itemIntranet.CODIGO_PRODUTO_COR.ToString());

                        if (produtoCor != null)
                            importacao.DescricaoProdutoCor = produtoCor.DESC_COR_PRODUTO;

                        importacao.FobIntranet = itemIntranet.FOB.ToString();

                        listaDivergenciaIntranet.Add(importacao);
                    }
                }

                return listaDivergenciaIntranet;
            }
            else
                return listaDivergenciaIntranet;
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProdutoColecao(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_COLECAO == colecao orderby pp.CODIGO_JANELA, pp.CODIGO_PRODUTO select pp).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProduto(int colecao, int janela, int proforma)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_COLECAO == colecao & pp.CODIGO_JANELA == janela & pp.CODIGO_PROFORMA == proforma orderby pp.CODIGO_PRODUTO select pp).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProdutoProduto(int produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_PRODUTO == produto orderby pp.CODIGO_JANELA, pp.CODIGO_PRODUTO select pp).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProdutoFornecedor(int colecao, string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_COLECAO == colecao & pp.DESCRICAO_FORNECEDOR.Equals(descricao) orderby pp.CODIGO_JANELA, pp.CODIGO_PRODUTO select pp).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaPorCodigoProduto(int colecao, int codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_COLECAO == colecao & pp.CODIGO_PRODUTO == codigoProduto orderby pp.CODIGO_JANELA, pp.CODIGO_PRODUTO select pp).ToList();
        }

        public List<PRODUTO> BuscaProdutoPelaDescricao(string colecao, string descricao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs where p.COLECAO.Equals(colecao) & p.DESC_PRODUTO.Equals(descricao) select p).ToList();
        }

        public List<p_valida_pos_tef_relatorio_intranetResult> BuscaPosTef()
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);

            return (from pt in db.p_valida_pos_tef_relatorio_intranet() select pt).ToList();
        }

        public List<IMPORTACAO_NEL_PRODUTO> BuscaNelProdutoProduto(int produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from np in db.IMPORTACAO_NEL_PRODUTOs where np.CODIGO_PRODUTO == produto orderby np.CODIGO_JANELA, np.CODIGO_PRODUTO select np).ToList();
        }

        public List<Sp_Busca_Draft_OriginalResult> BuscaDraftOriginal(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from df in db.Sp_Busca_Draft_Original(colecao) select df).ToList();
        }

        public List<Sp_Busca_Container_OriginalResult> BuscaContainerOriginal(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cf in db.Sp_Busca_Container_Original(colecao) select cf).ToList();
        }

        public List<Sp_Busca_Por_GriffeResult> BuscaPorGriffe(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cf in db.Sp_Busca_Por_Griffe(colecao) select cf).ToList();
        }

        public List<Sp_Busca_Por_GrupoResult> BuscaPorGrupo(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cf in db.Sp_Busca_Por_Grupo(colecao) select cf).ToList();
        }

        public List<Sp_Busca_Por_ProformaResult> BuscaPorProforma(int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cf in db.Sp_Busca_Por_Proforma(colecao) select cf).ToList();
        }

        public List<Sp_Busca_TicketsResult> BuscaTicketsLoja(string data, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from bt in db.Sp_Busca_Tickets(data, filial) select bt).ToList();
        }

        public List<LOJA_VENDA_PRODUTO> BuscaKitNatalTicket(string filial, string ticket)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from bknt in db.LOJA_VENDA_PRODUTOs where bknt.CODIGO_FILIAL.Equals(filial) & bknt.TICKET == ticket & bknt.PRODUTO.Equals("N001") select bknt).ToList();
        }

        public DEPOSITO_SALDO BuscaUltimoDepositoSaldo(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            List<DEPOSITO_SALDO> listasaldos = (from bds in db.DEPOSITO_SALDOs where bds.CODIGO_FILIAL == filial select bds).ToList();

            if (listasaldos.Count > 0)
            {
                int ultimo = (from bds in db.DEPOSITO_SALDOs where bds.CODIGO_FILIAL == filial select bds.CODIGO_SALDO).Max();

                return (from bds in db.DEPOSITO_SALDOs where bds.CODIGO_FILIAL == filial & bds.CODIGO_SALDO == ultimo select bds).SingleOrDefault();
            }
            else
                return null;
        }

        public List<DEPOSITO_SALDO> BuscaUltimosSaldos(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from bds in db.DEPOSITO_SALDOs where bds.CODIGO_FILIAL == filial orderby bds.CODIGO_SALDO descending select bds).ToList();
        }

        public COTASLOJA BuscaCotaFilial(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cl in db.COTASLOJAs where cl.LOJA.Equals(filial) select cl).SingleOrDefault();
        }

        public COLECOE BuscaColecaoAtual(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from c in db.COLECOEs where c.COLECAO.Equals(colecao) select c).SingleOrDefault();
        }

        public FUNDO_FIXO_CONTA_RECEITA BuscaContaReceita(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from cr in db.FUNDO_FIXO_CONTA_RECEITAs where cr.CODIGO_FILIAL == filial select cr).SingleOrDefault();
        }

        public FUNDO_FIXO_HISTORICO_FECHAMENTO BuscaUltimoFechamento(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from hf in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where hf.CODIGO_FILIAL == filial orderby hf.DATA descending select hf).Take(1).SingleOrDefault();
        }

        public FUNDO_FIXO_CONTA_RECEITA BuscaLimiteFundoFilial(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from lfl in db.FUNDO_FIXO_CONTA_RECEITAs where lfl.CODIGO_FILIAL == filial select lfl).SingleOrDefault();
        }

        public FILIAI BuscaFilial(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs where f.FILIAL.Equals(descricao) select f).SingleOrDefault();
        }

        public CLIENTES_VAREJO BuscaCliente(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from c in db.CLIENTES_VAREJOs where c.CODIGO_CLIENTE == codigo select c).SingleOrDefault();
        }

        public CLIENTES_VAREJO BuscaClienteCPF(string cpf)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from c in db.CLIENTES_VAREJOs where c.CPF_CGC.Trim().Equals(cpf.Trim()) select c).Take(1).SingleOrDefault();
        }

        public FILIAI BuscaFilialCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs where Convert.ToInt32(f.COD_FILIAL) == codigo select f).SingleOrDefault();
        }

        public ACOMPANHAMENTO_ALUGUEL_MESANO BuscaReferencia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from r in db.ACOMPANHAMENTO_ALUGUEL_MESANOs where r.CODIGO_ACOMPANHAMENTO_MESANO == codigo select r).SingleOrDefault();
        }

        public FILIAI BuscaFilialCodigoInt(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs where Convert.ToInt32(f.COD_FILIAL) == codigo select f).SingleOrDefault();
        }

        public FUNDO_FIXO_CONTA_DESPESA BuscaContaPorCodigoContaLoja(string codigoContaLoja)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FUNDO_FIXO_CONTA_DESPESAs where c.CODIGO.Equals(codigoContaLoja) select c).SingleOrDefault();
        }

        public ACOMPANHAMENTO_ALUGUEL_DESPESA BuscaAluguelDespesa(int codigoAluguelDespesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ad in db.ACOMPANHAMENTO_ALUGUEL_DESPESAs where ad.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA == codigoAluguelDespesa select ad).SingleOrDefault();
        }

        public ACOMPANHAMENTO_ALUGUEL_DESPESA BuscaContaAluguelPorCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.ACOMPANHAMENTO_ALUGUEL_DESPESAs where c.CODIGO_ACOMPANHAMENTO_ALUGUEL_DESPESA == codigo select c).SingleOrDefault();
        }

        public List<ACOMPANHAMENTO_ALUGUEL> BuscarAluguelEmAberto(string filial, string competencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.ACOMPANHAMENTO_ALUGUELs
                    where
                        c.CODIGO_FILIAL == Convert.ToInt32(filial)
                        && c.CODIGO_MESANO == Convert.ToInt32(competencia)
                        && c.STATUS == 1
                    select c).ToList();
        }


        public SP_ATUALIZAR_FUNDO_FIXOResult AtualizarFundoFixo(string codigoFilial, int codigoFechamento)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from f in dbIntra.SP_ATUALIZAR_FUNDO_FIXO(codigoFilial, codigoFechamento) select f).SingleOrDefault();
        }

        public FUNDO_FIXO_HISTORICO_DESPESA BuscaDespesaPorCodigo(int codigoDespesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from hd in db.FUNDO_FIXO_HISTORICO_DESPESAs where hd.CODIGO == codigoDespesa select hd).SingleOrDefault();
        }
        public void AtualizarDespesaFundoFixo(FUNDO_FIXO_HISTORICO_DESPESA despesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FUNDO_FIXO_HISTORICO_DESPESA _despesa = BuscaDespesaPorCodigo(despesa.CODIGO);

            if (_despesa != null)
            {
                _despesa.CODIGO = despesa.CODIGO;
                _despesa.VALOR = despesa.VALOR;

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

        public void ExcluirDespesaFundoFixo(int codigoDespesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {

                var d = BuscaDespesaPorCodigo(codigoDespesa);

                if (d != null)
                {
                    db.FUNDO_FIXO_HISTORICO_DESPESAs.DeleteOnSubmit(d);
                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public ACOMPANHAMENTO_ALUGUEL BuscaAluguelPorCodigo(int codigoAluguel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ai in db.ACOMPANHAMENTO_ALUGUELs where ai.CODIGO_ACOMPANHAMENTO_ALUGUEL == codigoAluguel select ai).SingleOrDefault();
        }

        public void AtualizaAcompanhamentoAluguel(ACOMPANHAMENTO_ALUGUEL _alu)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            ACOMPANHAMENTO_ALUGUEL alu = BuscaAluguelPorCodigo(_alu.CODIGO_ACOMPANHAMENTO_ALUGUEL);

            if (alu != null)
                alu.STATUS = 0;

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FILIAI BuscaDescricaoFilial(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs where f.COD_FILIAL.Equals(codigo) select f).SingleOrDefault();
        }

        public List<FUNDO_FIXO_CONTA_DESPESA> BuscaContas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FUNDO_FIXO_CONTA_DESPESAs where c.INATIVA == 0 select c).ToList();
        }

        public List<ACOMPANHAMENTO_ALUGUEL_DESPESA> BuscaAluguelDespesas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.ACOMPANHAMENTO_ALUGUEL_DESPESAs select c).ToList();
        }

        public ACOMPANHAMENTO_ALUGUEL_DESPESA ObterAluguelDespesaPorConta(string contaContabil)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.ACOMPANHAMENTO_ALUGUEL_DESPESAs where c.CONTA_CONTABIL == contaContabil.Trim() select c).SingleOrDefault();
        }

        public List<FECHAMENTO_CONTA> BuscaContasAntigas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CONTAs select c).ToList();
        }

        public List<IMPORTACAO_COLECAO> BuscaColecao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.IMPORTACAO_COLECAOs orderby c.CODIGO_COLECAO select c).ToList();
        }

        public IMPORTACAO_CONTAINER BuscaContainerPelaDescricao(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.IMPORTACAO_CONTAINERs where c.DESCRICAO.Equals(descricao) select c).SingleOrDefault();
        }

        public List<IMPORTACAO_NEL> BuscaNel()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from n in db.IMPORTACAO_NELs orderby n.CODIGO_NEL select n).ToList();
        }

        public List<IMPORTACAO_NEL> BuscaNel(int colecao, int janela)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from n in db.IMPORTACAO_NELs where n.CODIGO_COLECAO == colecao & n.CODIGO_JANELA == janela orderby n.CODIGO_NEL select n).ToList();
        }

        public IMPORTACAO_NEL BuscaNelPeloCodigo(int codigoNel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from n in db.IMPORTACAO_NELs where n.CODIGO_NEL == codigoNel select n).SingleOrDefault();
        }

        public List<IMPORTACAO_NEL> BuscaNelPorContainer(int container)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from n in db.IMPORTACAO_NELs where n.CODIGO_CONTAINER == container orderby n.CODIGO_NEL select n).ToList();
        }

        public List<IMPORTACAO_NEL_PRODUTO> BuscaNelProdutos(int codigoNel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from np in db.IMPORTACAO_NEL_PRODUTOs where np.CODIGO_NEL == codigoNel orderby np.CODIGO_NEL_PRODUTO select np).ToList();
        }

        public List<ESTOQUE_SOBRA> BuscaEstoqueSobra(string categoria, string colecao, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from es in db.ESTOQUE_SOBRAs where es.CATEGORIA.Equals(categoria) & es.COLECAO.Equals(colecao) & es.GRIFFE.Equals(griffe) orderby es.GRUPO select es).ToList();
        }

        public List<IMPORTACAO_JANELA> BuscaJanela()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from j in db.IMPORTACAO_JANELAs select j).ToList();
        }

        public IMPORTACAO_JANELA BuscaJanela(int codigoJanela)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from j in db.IMPORTACAO_JANELAs where j.CODIGO_JANELA == codigoJanela select j).SingleOrDefault();
        }

        public List<IMPORTACAO_GRIFFE> BuscaGriffe()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.IMPORTACAO_GRIFFEs select g).ToList();
        }

        public IMPORTACAO_GRIFFE BuscaGriffe(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.IMPORTACAO_GRIFFEs where g.CODIGO_GRIFFE == codigo select g).SingleOrDefault();
        }

        public List<IMPORTACAO_PROFORMA> BuscaProformasColecao(int codigoColecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from p in db.IMPORTACAO_PROFORMAs where p.CODIGO_COLECAO == codigoColecao select p).ToList();
        }

        public List<IMPORTACAO_PROFORMA> BuscaProformasProvisoria()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from p in db.IMPORTACAO_PROFORMAs where p.CODIGO_COLECAO == 0 select p).ToList();
        }

        public List<IMPORTACAO_PROFORMA> BuscaProformas(int codigoColecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from p in db.IMPORTACAO_PROFORMAs where p.CODIGO_COLECAO == codigoColecao || p.CODIGO_COLECAO == 0 select p).ToList();
        }

        public IMPORTACAO_PROFORMA BuscaProformaPeloCodigo(int codigoProforma)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from p in db.IMPORTACAO_PROFORMAs where p.CODIGO_PROFORMA == codigoProforma select p).SingleOrDefault();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProdutoPorProforma(int proforma)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_PROFORMA == proforma select pp).ToList();
        }

        public List<IMPORTACAO_FORNECEDOR> BuscaFornecedor()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.IMPORTACAO_FORNECEDORs select f).ToList();
        }

        public IMPORTACAO_FORNECEDOR BuscaFornecedor(int codigoFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.IMPORTACAO_FORNECEDORs where f.CODIGO_FORNECEDOR == codigoFornecedor select f).SingleOrDefault();
        }

        public IMPORTACAO_PACK_GRADE BuscaPackGrade(int codigoPackGrade)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pg in db.IMPORTACAO_PACK_GRADEs where pg.CODIGO_PACK_GRADE == codigoPackGrade select pg).SingleOrDefault();
        }

        public IMPORTACAO_PACK_GROUP BuscaPackGroup(int codigoPackGroup)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pg in db.IMPORTACAO_PACK_GROUPs where pg.CODIGO_PACK_GROUP == codigoPackGroup select pg).SingleOrDefault();
        }

        public List<IMPORTACAO_ARMARIO> BuscaArmario()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from a in db.IMPORTACAO_ARMARIOs select a).ToList();
        }

        public List<IMPORTACAO_PACK_GRADE> BuscaPackGrade()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pg in db.IMPORTACAO_PACK_GRADEs select pg).ToList();
        }

        public List<IMPORTACAO_PACK_GROUP> BuscaPackGroup()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pg in db.IMPORTACAO_PACK_GROUPs select pg).ToList();
        }

        public List<IMPORTACAO_PACK_GRADE> BuscaPackGrade(string grupoProduto, int codigoGriffe)
        {
            string codigoGrupo = BuscaGrupoProduto(grupoProduto).CODIGO_GRUPO;

            if (codigoGrupo != null)
            {
                db = new DCDataContext(Constante.ConnectionStringIntranet);

                return (from pg in db.IMPORTACAO_PACK_GRADEs where pg.CODIGO_GRUPO.Equals(codigoGrupo) & pg.CODIGO_GRIFFE == codigoGriffe select pg).ToList();
            }
            else
                return null;
        }

        public List<IMPORTACAO_PACK_GROUP> BuscaPackGroup(string grupoProduto, int codigoGriffe)
        {
            string codigoGrupo = BuscaGrupoProduto(grupoProduto).CODIGO_GRUPO;

            if (codigoGrupo != null)
            {
                db = new DCDataContext(Constante.ConnectionStringIntranet);

                return (from pg in db.IMPORTACAO_PACK_GROUPs where pg.CODIGO_GRUPO.Equals(codigoGrupo) & pg.CODIGO_GRIFFE == codigoGriffe select pg).ToList();
            }
            else
                return null;
        }

        public List<IMPORTACAO_PACK_GROUP> BuscaPackGroupTipo(string grupoProduto, int codigoGriffe, string tipoProduto)
        {
            string codigoGrupo = BuscaGrupoProduto(grupoProduto).CODIGO_GRUPO;

            if (codigoGrupo != null)
            {
                db = new DCDataContext(Constante.ConnectionStringIntranet);

                return (from pg in db.IMPORTACAO_PACK_GROUPs where pg.CODIGO_GRUPO.Equals(codigoGrupo) & pg.CODIGO_GRIFFE == codigoGriffe & pg.DESCRICAO_TIPO.Equals(tipoProduto) select pg).ToList();
            }
            else
                return null;
        }

        public List<FILIAI1> ListarFiliais()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.FILIAI1s select f).OrderBy(p => p.FILIAL).ToList();
        }

        public List<SP_OBTER_FILIAIS_MARCADASResult> ListarFiliaisMarcadas(Char c_AGORA_REDE,
                                                                           Char c_AGORA_SUPERVISORES,
                                                                           Char c_MOD_ADMINISTRACAO,
                                                                           Char c_MOD_GESTAO,
                                                                           Char c_MOD_ATACADO,
                                                                           Char c_MOD_FINANCEIRO,
                                                                           Char c_MOD_CONTABILIDADE,
                                                                           Char c_MOD_FISCAL,
                                                                           Char c_MOD_ADM_NOTA_FISCAL,
                                                                           Char c_MOD_ADMINISTRACAO_LOJA,
                                                                           Char c_MOD_GERENCIAMENTO_LOJA,
                                                                           Char c_MOD_ACOMPANHAMENTO_MENSAL,
                                                                           Char c_MOD_CONTROLE_ESTOQUE,
                                                                           Char c_MOD_CONTAGEM,
                                                                           Char c_MOD_RH,
                                                                           Char c_MOD_REPRESENTANTE)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 600;
            return (from m in db.SP_OBTER_FILIAIS_MARCADAS(c_AGORA_REDE, c_AGORA_SUPERVISORES, c_MOD_ADMINISTRACAO, c_MOD_GESTAO, c_MOD_ATACADO, c_MOD_FINANCEIRO, c_MOD_CONTABILIDADE, c_MOD_FISCAL, c_MOD_ADM_NOTA_FISCAL, c_MOD_ADMINISTRACAO_LOJA, c_MOD_GERENCIAMENTO_LOJA, c_MOD_ACOMPANHAMENTO_MENSAL, c_MOD_CONTROLE_ESTOQUE, c_MOD_CONTAGEM, c_MOD_RH, c_MOD_REPRESENTANTE) select m).ToList();

        }

        public List<FILIAI> BuscaFiliais()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs
                    where f.TIPO_FILIAL.ToUpper().Trim().Equals("LOJA")
                    select f).ToList();
        }

        public List<DRE_FILIAL> ObterFiliaisDRE()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.DRE_FILIALs
                    where f.ATIVO == true
                    orderby f.FILIAL
                    select f).ToList();
        }
        public List<DRE_FILIAL> ObterFiliaisDRE(string unidadeNegocio)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.DRE_FILIALs
                    where f.UNIDADE_NEGOCIO == unidadeNegocio
                    && f.ATIVO == true
                    orderby f.FILIAL
                    select f).ToList();
        }

        public List<FILIAI> BuscaFiliaisAtivaInativa()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs
                    where
                        f.TIPO_FILIAL.ToUpper().Trim().Equals("LOJA") ||
                        (
                        (f.TIPO_FILIAL.ToUpper().Trim().Equals("BAIXADO") ||
                        f.TIPO_FILIAL.ToUpper().Trim().Equals("INATIVA")) &&
                        !f.COD_FILIAL.Trim().Contains("0000"))


                    select f).ToList();
        }

        public List<FILIAI> BuscaFiliais_Intermediario()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs
                    where f.TIPO_FILIAL.ToUpper().Trim().Equals("LOJA")
                    select f).ToList();
        }

        public List<FILIAI> BuscaFiliaisAtivaInativa_Intermediario()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs
                    where
                        (f.TIPO_FILIAL.ToUpper().Trim().Equals("LOJA") || f.TIPO_FILIAL.ToUpper().Trim().Equals("INATIVA"))
                    select f).ToList();
        }

        public List<FILIAI> BuscaFiliais_Agora()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs
                    where f.TIPO_FILIAL.ToUpper().Trim().Equals("LOJA")
                    select f).ToList();
        }

        public List<FILIAI> BuscaFiliaisAtivaInativa_Agora()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs
                    where
                        (f.TIPO_FILIAL.ToUpper().Trim().Equals("LOJA") || f.TIPO_FILIAL.ToUpper().Trim().Equals("INATIVA"))
                    select f).ToList();
        }

        public List<ACOMPANHAMENTO_ALUGUEL_MESANO> BuscaReferencias()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from r in db.ACOMPANHAMENTO_ALUGUEL_MESANOs orderby r.ANO descending, r.MES descending select r).ToList();
        }

        public List<APOIO_FAROL> BuscaApoioFarol()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from af in db.APOIO_FAROLs select af).ToList();
        }

        public FILIAIS_DE_PARA BuscaFilialDePara(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.FILIAIS_DE_PARAs where f.PARA.Equals(filial) select f).SingleOrDefault();
        }

        public List<FILIAIS_DE_PARA> BuscaFilialDePara()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.FILIAIS_DE_PARAs select f).ToList();
        }

        public DateTime? BuscaDataHoraUltimaVenda(DateTime data, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDAs where lv.DATA_VENDA.Date == data.Date & lv.CODIGO_FILIAL.Equals(filial) select lv.DATA_DIGITACAO).Max();
        }

        public DateTime BuscaDataFiscal(string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lcf in db.LOJA_CONTROLE_FISCALs where lcf.CODIGO_FILIAL.Equals(filial) select lcf.DATA_FISCAL).Max();
        }

        public List<FILIAI> BuscaFiliais(USUARIO usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            List<USUARIOLOJA> usuarioLoja = BuscaUsuarioLoja(usuario);

            List<string> idLojas = new List<string>();

            foreach (USUARIOLOJA i in usuarioLoja)
                idLojas.Add(i.CODIGO_LOJA);

            db = new DCDataContext(Constante.ConnectionString);

            if (usuario.CODIGO_PERFIL == 5 ||
                usuario.CODIGO_PERFIL == 24 || usuario.CODIGO_PERFIL == 34 ||
                usuario.CODIGO_PERFIL == 11 ||
                usuario.CODIGO_PERFIL == 25 ||
                usuario.CODIGO_PERFIL == 37 || usuario.CODIGO_PERFIL == 38 ||
                usuario.CODIGO_PERFIL == 17 ||
                usuario.CODIGO_PERFIL == 36 ||
                usuario.CODIGO_PERFIL == 40 ||
                usuario.CODIGO_PERFIL == 49 ||
                usuario.CODIGO_PERFIL == 27)
                return (from f in db.FILIAIs
                        where f.TIPO_FILIAL.Equals("Loja")
                        select f).OrderBy(p => p.FILIAL).ToList();
            if (usuario.CODIGO_PERFIL == 1 ||
                usuario.CODIGO_PERFIL == 43 ||
                usuario.CODIGO_PERFIL == 10 ||
                usuario.CODIGO_PERFIL == 51 ||
                usuario.CODIGO_PERFIL == 31 ||
                usuario.CODIGO_PERFIL == 6)
                return (from f in db.FILIAIs
                        where (f.TIPO_FILIAL.Equals("Loja"))
                        select f).OrderBy(p => p.FILIAL).ToList();
            else
                return (from f in db.FILIAIs
                        where idLojas.Contains(f.COD_FILIAL) &&
                              f.TIPO_FILIAL.Equals("Loja")
                        select f).OrderBy(p => p.FILIAL).ToList();
        }

        public List<FILIAI> BuscaFiliais_Intermediario(USUARIO usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            List<USUARIOLOJA> usuarioLoja = BuscaUsuarioLoja(usuario);

            List<string> idLojas = new List<string>();

            foreach (USUARIOLOJA i in usuarioLoja)
                idLojas.Add(i.CODIGO_LOJA);

            db = new DCDataContext(Constante.ConnectionString);

            if (usuario.CODIGO_PERFIL == 5 ||
                usuario.CODIGO_PERFIL == 24 || usuario.CODIGO_PERFIL == 34 ||
                usuario.CODIGO_PERFIL == 11 ||
                usuario.CODIGO_PERFIL == 25 || usuario.CODIGO_PERFIL == 47 ||
                usuario.CODIGO_PERFIL == 37 || usuario.CODIGO_PERFIL == 38 ||
                usuario.CODIGO_PERFIL == 17 ||
                usuario.CODIGO_PERFIL == 36 ||
                usuario.CODIGO_PERFIL == 40 ||
                usuario.CODIGO_PERFIL == 27)
                return (from f in db.FILIAIs
                        where f.TIPO_FILIAL.Equals("Loja")
                        select f).ToList();
            if (usuario.CODIGO_PERFIL == 1 ||
                usuario.CODIGO_PERFIL == 10 ||
                usuario.CODIGO_PERFIL == 31 ||
                usuario.CODIGO_PERFIL == 6)
                return (from f in db.FILIAIs
                        where (f.TIPO_FILIAL.Equals("Loja") ||
                              f.TIPO_FILIAL.Equals("Atacado") ||
                              f.TIPO_FILIAL.Equals("Inativa"))
                        select f).ToList();
            else
                return (from f in db.FILIAIs
                        where idLojas.Contains(f.COD_FILIAL) &&
                              f.TIPO_FILIAL.Equals("Loja")
                        select f).ToList();
        }

        public List<FILIAI> BuscaFiliais_Agora(USUARIO usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            List<USUARIOLOJA> usuarioLoja = BuscaUsuarioLoja(usuario);

            List<string> idLojas = new List<string>();

            foreach (USUARIOLOJA i in usuarioLoja)
                idLojas.Add(i.CODIGO_LOJA);

            db = new DCDataContext(Constante.ConnectionString);

            if (usuario.CODIGO_PERFIL == 5 ||
                usuario.CODIGO_PERFIL == 24 || usuario.CODIGO_PERFIL == 34 ||
                usuario.CODIGO_PERFIL == 11 ||
                usuario.CODIGO_PERFIL == 25 ||
                usuario.CODIGO_PERFIL == 37 || usuario.CODIGO_PERFIL == 38 ||
                usuario.CODIGO_PERFIL == 17 ||
                usuario.CODIGO_PERFIL == 36 ||
                usuario.CODIGO_PERFIL == 40 ||
                usuario.CODIGO_PERFIL == 27)
                return (from f in db.FILIAIs
                        where f.TIPO_FILIAL.Equals("Loja")
                        select f).ToList();
            if (usuario.CODIGO_PERFIL == 1 ||
                usuario.CODIGO_PERFIL == 10 ||
                usuario.CODIGO_PERFIL == 31 ||
                usuario.CODIGO_PERFIL == 6)
                return (from f in db.FILIAIs
                        where (f.TIPO_FILIAL.Equals("Loja") ||
                              f.TIPO_FILIAL.Equals("Atacado"))
                        select f).ToList();
            else
                return (from f in db.FILIAIs
                        where idLojas.Contains(f.COD_FILIAL) &&
                              f.TIPO_FILIAL.Equals("Loja")
                        select f).ToList();
        }

        public List<FILIAI> BuscaTodasFiliais()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from f in db.FILIAIs where f.TIPO_FILIAL != "" && f.TIPO_FILIAL != "" orderby f.FILIAL ascending select f).ToList();
        }

        public List<DEPOSITO_BAIXA> BuscaDepositoBaixas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from b in db.DEPOSITO_BAIXAs select b).ToList();
        }

        public List<Farol> BuscaMovimentosLoja(List<FILIAI> filiais, string data)
        {
            List<Farol> farois = new List<Farol>();
            Farol farol = null;

            string ano = "";
            string mes = "";
            string dia = "";
            string dataAnoAnt = "";
            string dataInicioSemana = "";
            string dataInicioSemanaAnoAnt = "";
            string dataInicioMes = "";
            string dataInicioMesAnoAnt = "";
            string mesInicioTri = "";
            string mesInicioTriAnoAnt = "";
            string dataInicioTri = "";
            string dataInicioTriAnoAnt = "";

            int anoAnt = 0;

            Sp_Busca_Semana_454Result anoSemana = null;
            Sp_Busca_Semana_454Result anoSemanaAnoAnterior = null;

            foreach (FILIAI filial in filiais)
            {
                ano = AjustaData(data).Substring(0, 4);
                mes = AjustaData(data).Substring(4, 2);
                dia = AjustaData(data).Substring(6, 2);

                anoAnt = Convert.ToInt32(ano) - 1;

                dataAnoAnt = anoAnt.ToString() + mes + dia;

                anoSemana = BuscaSemana454(AjustaData(data));
                anoSemanaAnoAnterior = BuscaSemana454(AjustaData(dataAnoAnt));

                dataInicioSemana = BuscaDataInicio(anoSemana.ano_semana.ToString());
                dataInicioSemanaAnoAnt = BuscaDataInicio(anoSemanaAnoAnterior.ano_semana.ToString());

                dataInicioMes = BuscaDataInicioMes(Convert.ToInt32(anoSemana.mes), anoSemana.ano);
                dataInicioMesAnoAnt = BuscaDataInicioMes(Convert.ToInt32(anoSemanaAnoAnterior.mes), anoSemanaAnoAnterior.ano);

                mesInicioTri = "";
                mesInicioTriAnoAnt = "";

                if (anoSemana.mes.ToString() == "1" ||
                    anoSemana.mes.ToString() == "2" ||
                    anoSemana.mes.ToString() == "3")
                    mesInicioTri = "1";
                if (anoSemana.mes.ToString() == "4" ||
                    anoSemana.mes.ToString() == "5" ||
                    anoSemana.mes.ToString() == "6")
                    mesInicioTri = "4";
                if (anoSemana.mes.ToString() == "7" ||
                    anoSemana.mes.ToString() == "8" ||
                    anoSemana.mes.ToString() == "9")
                    mesInicioTri = "7";
                if (anoSemana.mes.ToString() == "10" ||
                    anoSemana.mes.ToString() == "11" ||
                    anoSemana.mes.ToString() == "12")
                    mesInicioTri = "10";

                if (anoSemanaAnoAnterior.mes.ToString() == "1" ||
                    anoSemanaAnoAnterior.mes.ToString() == "2" ||
                    anoSemanaAnoAnterior.mes.ToString() == "3")
                    mesInicioTriAnoAnt = "1";
                if (anoSemanaAnoAnterior.mes.ToString() == "4" ||
                    anoSemanaAnoAnterior.mes.ToString() == "5" ||
                    anoSemanaAnoAnterior.mes.ToString() == "6")
                    mesInicioTriAnoAnt = "4";
                if (anoSemanaAnoAnterior.mes.ToString() == "7" ||
                    anoSemanaAnoAnterior.mes.ToString() == "8" ||
                    anoSemanaAnoAnterior.mes.ToString() == "9")
                    mesInicioTriAnoAnt = "7";
                if (anoSemanaAnoAnterior.mes.ToString() == "10" ||
                    anoSemanaAnoAnterior.mes.ToString() == "11" ||
                    anoSemanaAnoAnterior.mes.ToString() == "12")
                    mesInicioTriAnoAnt = "10";

                dataInicioTri = BuscaDataInicioMes(Convert.ToInt32(mesInicioTri), anoSemana.ano);
                dataInicioTriAnoAnt = BuscaDataInicioMes(Convert.ToInt32(mesInicioTriAnoAnt), anoSemanaAnoAnterior.ano);

                //Vendas

                db = new DCDataContext(Constante.ConnectionString);
                db.CommandTimeout = 0;

                Sp_Farol_LojaResult wFarol = db.Sp_Farol_Loja(filial.COD_FILIAL,
                                                              AjustaData(data),
                                                              AjustaData(dataAnoAnt),
                                                              AjustaData(dataInicioSemana),
                                                              AjustaData(dataInicioSemanaAnoAnt),
                                                              AjustaData(dataInicioMes),
                                                              AjustaData(dataInicioMesAnoAnt),
                                                              AjustaData(dataInicioTri),
                                                              AjustaData(dataInicioTriAnoAnt)).SingleOrDefault();
                farol = new Farol();
                farol.Loja = filial.FILIAL;

                farol.Vl_venda_dia = (Convert.ToDouble(wFarol.Vl_Venda_Dia)).ToString("##,###.##");
                farol.Vl_venda_dia_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Dia_Ano_Ant)).ToString("##,###.##");
                farol.Qt_venda_dia = Convert.ToInt32(wFarol.Qt_Venda_Dia);
                farol.Qt_venda_dia_ano_ant = Convert.ToInt32(wFarol.Qt_Venda_Dia_Ano_Ant);

                if (Convert.ToDouble(wFarol.Qt_Venda_Dia) > 0)
                    farol.Vl_medio_dia = (Convert.ToDouble(wFarol.Vl_Venda_Dia) / Convert.ToDouble(wFarol.Qt_Venda_Dia)).ToString("##,###.##");
                if (Convert.ToDouble(wFarol.Qt_Venda_Dia_Ano_Ant) > 0)
                    farol.Vl_medio_dia_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Dia_Ano_Ant) / Convert.ToDouble(wFarol.Qt_Venda_Dia_Ano_Ant)).ToString("##,###.##");

                farol.Vl_venda_sem = (Convert.ToDouble(wFarol.Vl_Venda_Sem)).ToString("##,###.##");
                farol.Vl_venda_sem_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Sem_Ano_Ant)).ToString("##,###.##");
                farol.Qt_venda_sem = Convert.ToInt32(wFarol.Qt_Venda_Sem);
                farol.Qt_venda_sem_ano_ant = Convert.ToInt32(wFarol.Qt_Venda_Sem_Ano_Ant);

                if (Convert.ToDouble(wFarol.Qt_Venda_Sem) > 0)
                    farol.Vl_medio_sem = (Convert.ToDouble(wFarol.Vl_Venda_Sem) / Convert.ToDouble(wFarol.Qt_Venda_Sem)).ToString("##,###.##");
                if (Convert.ToDouble(wFarol.Qt_Venda_Sem_Ano_Ant) > 0)
                    farol.Vl_medio_sem_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Sem_Ano_Ant) / Convert.ToDouble(wFarol.Qt_Venda_Sem_Ano_Ant)).ToString("##,###.##");

                farol.Vl_venda_mes = (Convert.ToDouble(wFarol.Vl_Venda_Mes)).ToString("##,###.##");
                farol.Vl_venda_mes_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Mes_Ano_Ant)).ToString("##,###.##");
                farol.Qt_venda_mes = Convert.ToInt32(wFarol.Qt_Venda_Mes);
                farol.Qt_venda_mes_ano_ant = Convert.ToInt32(wFarol.Qt_Venda_Mes_Ano_Ant);

                if (Convert.ToDouble(wFarol.Qt_Venda_Mes) > 0)
                    farol.Vl_medio_mes = (Convert.ToDouble(wFarol.Vl_Venda_Mes) / Convert.ToDouble(wFarol.Qt_Venda_Mes)).ToString("##,###.##");
                if (Convert.ToDouble(wFarol.Qt_Venda_Mes_Ano_Ant) > 0)
                    farol.Vl_medio_mes_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Mes_Ano_Ant) / Convert.ToDouble(wFarol.Qt_Venda_Mes_Ano_Ant)).ToString("##,###.##");

                farol.Vl_venda_tri = (Convert.ToDouble(wFarol.Vl_Venda_Mes)).ToString("##,###.##");
                farol.Vl_venda_tri_ano_ant = (Convert.ToDouble(wFarol.Vl_Venda_Mes_Ano_Ant)).ToString("##,###.##");

                // Cotas

                db = new DCDataContext(Constante.ConnectionStringIntranet);
                db.CommandTimeout = 0;

                Sp_Farol_Cota_LojaResult wFarolCotas = db.Sp_Farol_Cota_Loja(filial.COD_FILIAL,
                                                                             AjustaData(data),
                                                                             AjustaData(dataAnoAnt),
                                                                             AjustaData(dataInicioSemana),
                                                                             AjustaData(dataInicioSemanaAnoAnt),
                                                                             AjustaData(dataInicioMes),
                                                                             AjustaData(dataInicioMesAnoAnt),
                                                                             AjustaData(dataInicioTri),
                                                                             AjustaData(dataInicioTriAnoAnt)).SingleOrDefault();

                farol.Vl_cota_dia = (Convert.ToDouble(wFarolCotas.Vl_Cota_Dia)).ToString("##,###.##");

                if (Convert.ToDouble(farol.Vl_medio_dia) > 0)
                    farol.Qt_cota_dia = Convert.ToInt32(Convert.ToDouble(wFarolCotas.Vl_Cota_Dia) / Convert.ToDouble(farol.Vl_medio_dia));

                farol.Vl_cota_dia_ano_ant = (Convert.ToDouble(wFarolCotas.Vl_Cota_Dia_Ano_Ant)).ToString("##,###.##");

                if (Convert.ToDouble(farol.Vl_medio_dia_ano_ant) > 0)
                    farol.Qt_cota_dia_ano_ant = Convert.ToInt32(Convert.ToDouble(wFarolCotas.Vl_Cota_Dia_Ano_Ant) / Convert.ToDouble(farol.Vl_medio_dia_ano_ant));

                farol.Vl_cota_sem = (Convert.ToDouble(wFarolCotas.Vl_Cota_Sem)).ToString("##,###.##");

                if (Convert.ToDouble(farol.Vl_medio_sem) > 0)
                    farol.Qt_cota_sem = Convert.ToInt32(Convert.ToDouble(wFarolCotas.Vl_Cota_Sem) / Convert.ToDouble(farol.Vl_medio_sem));

                farol.Vl_cota_sem_ano_ant = (Convert.ToDouble(wFarolCotas.Vl_Cota_Sem_Ano_Ant)).ToString("##,###.##");

                if (Convert.ToDouble(farol.Vl_medio_sem_ano_ant) > 0)
                    farol.Qt_cota_sem_ano_ant = Convert.ToInt32(Convert.ToDouble(wFarolCotas.Vl_Cota_Sem_Ano_Ant) / Convert.ToDouble(farol.Vl_medio_sem_ano_ant));

                farol.Vl_cota_mes = (Convert.ToDouble(wFarolCotas.Vl_Cota_Mes)).ToString("##,###.##");

                if (Convert.ToDouble(farol.Vl_medio_mes) > 0)
                    farol.Qt_cota_mes = Convert.ToInt32(Convert.ToDouble(wFarolCotas.Vl_Cota_Mes) / Convert.ToDouble(farol.Vl_medio_mes));

                farol.Vl_cota_mes_ano_ant = (Convert.ToDouble(wFarolCotas.Vl_Cota_Mes_Ano_Ant)).ToString("##,###.##");

                if (Convert.ToDouble(farol.Vl_medio_mes_ano_ant) > 0)
                    farol.Qt_cota_mes_ano_ant = Convert.ToInt32(Convert.ToDouble(wFarolCotas.Vl_Cota_Mes_Ano_Ant) / Convert.ToDouble(farol.Vl_medio_mes_ano_ant));

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Dia) > 0)
                    farol.Perc_at_dia = (((Convert.ToDouble(wFarol.Vl_Venda_Dia) / Convert.ToDouble(wFarolCotas.Vl_Cota_Dia)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Sem) > 0)
                    farol.Perc_at_sem = (((Convert.ToDouble(wFarol.Vl_Venda_Sem) / Convert.ToDouble(wFarolCotas.Vl_Cota_Sem)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Mes) > 0)
                    farol.Perc_at_mes = (((Convert.ToDouble(wFarol.Vl_Venda_Mes) / Convert.ToDouble(wFarolCotas.Vl_Cota_Mes)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Dia_Ano_Ant) > 0)
                    farol.Perc_at_dia_ano_ant = (((Convert.ToDouble(wFarol.Vl_Venda_Dia_Ano_Ant) / Convert.ToDouble(wFarolCotas.Vl_Cota_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Sem_Ano_Ant) > 0)
                    farol.Perc_at_sem_ano_ant = (((Convert.ToDouble(wFarol.Vl_Venda_Sem_Ano_Ant) / Convert.ToDouble(wFarolCotas.Vl_Cota_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Mes_Ano_Ant) > 0)
                    farol.Perc_at_mes_ano_ant = (((Convert.ToDouble(wFarol.Vl_Venda_Mes_Ano_Ant) / Convert.ToDouble(wFarolCotas.Vl_Cota_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Dia_Ano_Ant) > 0)
                    farol.Perc_at_dia_cota = (((Convert.ToDouble(wFarolCotas.Vl_Cota_Dia) / Convert.ToDouble(wFarolCotas.Vl_Cota_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarol.Vl_Venda_Dia_Ano_Ant) > 0)
                    farol.Perc_at_dia_venda = (((Convert.ToDouble(wFarol.Vl_Venda_Dia) / Convert.ToDouble(wFarol.Vl_Venda_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Sem_Ano_Ant) > 0)
                    farol.Perc_at_sem_cota = (((Convert.ToDouble(wFarolCotas.Vl_Cota_Sem) / Convert.ToDouble(wFarolCotas.Vl_Cota_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarol.Vl_Venda_Sem_Ano_Ant) > 0)
                    farol.Perc_at_sem_venda = (((Convert.ToDouble(wFarol.Vl_Venda_Sem) / Convert.ToDouble(wFarol.Vl_Venda_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Mes_Ano_Ant) > 0)
                    farol.Perc_at_mes_cota = (((Convert.ToDouble(wFarolCotas.Vl_Cota_Mes) / Convert.ToDouble(wFarolCotas.Vl_Cota_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarol.Vl_Venda_Mes_Ano_Ant) > 0)
                    farol.Perc_at_mes_venda = (((Convert.ToDouble(wFarol.Vl_Venda_Mes) / Convert.ToDouble(wFarol.Vl_Venda_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarol.Vl_Venda_Tri_Ano_Ant) > 0)
                    farol.Perc_venda_tri_at_ano = (((Convert.ToDouble(wFarol.Vl_Venda_Tri) / Convert.ToDouble(wFarol.Vl_Venda_Tri_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Tri_Ano_Ant) > 0)
                    farol.Perc_cota_tri_at_ano = (((Convert.ToDouble(wFarolCotas.Vl_Cota_Tri) / Convert.ToDouble(wFarolCotas.Vl_Cota_Tri_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Tri) > 0)
                    farol.Perc_venda_cota_tri = (((Convert.ToDouble(wFarol.Vl_Venda_Tri) / Convert.ToDouble(wFarolCotas.Vl_Cota_Tri)) - 1) * 100).ToString("N0") + "%";

                if (Convert.ToDouble(wFarolCotas.Vl_Cota_Tri_Ano_Ant) > 0)
                    farol.Perc_venda_cota_tri_ano_ant = (((Convert.ToDouble(wFarol.Vl_Venda_Tri_Ano_Ant) / Convert.ToDouble(wFarolCotas.Vl_Cota_Tri_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

                farol.Vl_cota_tri = (Convert.ToDouble(wFarolCotas.Vl_Cota_Tri)).ToString("##,###.##");
                farol.Vl_cota_tri_ano_ant = (Convert.ToDouble(wFarolCotas.Vl_Cota_Tri_Ano_Ant)).ToString("##,###.##");

                farois.Add(farol);
            }

            //Vendas

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            Sp_FarolResult tFarol = db.Sp_Farol(AjustaData(data),
                                                AjustaData(dataAnoAnt),
                                                AjustaData(dataInicioSemana),
                                                AjustaData(dataInicioSemanaAnoAnt),
                                                AjustaData(dataInicioMes),
                                                AjustaData(dataInicioMesAnoAnt),
                                                AjustaData(dataInicioTri),
                                                AjustaData(dataInicioTriAnoAnt)).SingleOrDefault();
            farol = new Farol();
            farol.Loja = "Geral";

            farol.Vl_venda_dia = (Convert.ToDouble(tFarol.Vl_Venda_Dia)).ToString("##,###.##");
            farol.Vl_venda_dia_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant)).ToString("##,###.##");
            farol.Qt_venda_dia = Convert.ToInt32(tFarol.Qt_Venda_Dia);
            farol.Qt_venda_dia_ano_ant = Convert.ToInt32(tFarol.Qt_Venda_Dia_Ano_Ant);

            if (Convert.ToDouble(tFarol.Qt_Venda_Dia) > 0)
                farol.Vl_medio_dia = (Convert.ToDouble(tFarol.Vl_Venda_Dia) / Convert.ToDouble(tFarol.Qt_Venda_Dia)).ToString("##,###.##");
            if (Convert.ToDouble(tFarol.Qt_Venda_Dia_Ano_Ant) > 0)
                farol.Vl_medio_dia_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant) / Convert.ToDouble(tFarol.Qt_Venda_Dia_Ano_Ant)).ToString("##,###.##");

            farol.Vl_venda_sem = (Convert.ToDouble(tFarol.Vl_Venda_Sem)).ToString("##,###.##");
            farol.Vl_venda_sem_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant)).ToString("##,###.##");
            farol.Qt_venda_sem = Convert.ToInt32(tFarol.Qt_Venda_Sem);
            farol.Qt_venda_sem_ano_ant = Convert.ToInt32(tFarol.Qt_Venda_Sem_Ano_Ant);

            if (Convert.ToDouble(tFarol.Qt_Venda_Sem) > 0)
                farol.Vl_medio_sem = (Convert.ToDouble(tFarol.Vl_Venda_Sem) / Convert.ToDouble(tFarol.Qt_Venda_Sem)).ToString("##,###.##");
            if (Convert.ToDouble(tFarol.Qt_Venda_Sem_Ano_Ant) > 0)
                farol.Vl_medio_sem_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant) / Convert.ToDouble(tFarol.Qt_Venda_Sem_Ano_Ant)).ToString("##,###.##");

            farol.Vl_venda_mes = (Convert.ToDouble(tFarol.Vl_Venda_Mes)).ToString("##,###.##");
            farol.Vl_venda_mes_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant)).ToString("##,###.##");
            farol.Qt_venda_mes = Convert.ToInt32(tFarol.Qt_Venda_Mes);
            farol.Qt_venda_mes_ano_ant = Convert.ToInt32(tFarol.Qt_Venda_Mes_Ano_Ant);

            if (Convert.ToDouble(tFarol.Qt_Venda_Mes) > 0)
                farol.Vl_medio_mes = (Convert.ToDouble(tFarol.Vl_Venda_Mes) / Convert.ToDouble(tFarol.Qt_Venda_Mes)).ToString("##,###.##");
            if (Convert.ToDouble(tFarol.Qt_Venda_Mes_Ano_Ant) > 0)
                farol.Vl_medio_mes_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant) / Convert.ToDouble(tFarol.Qt_Venda_Mes_Ano_Ant)).ToString("##,###.##");

            farol.Vl_venda_tri = (Convert.ToDouble(tFarol.Vl_Venda_Mes)).ToString("##,###.##");
            farol.Vl_venda_tri_ano_ant = (Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant)).ToString("##,###.##");

            // Cotas

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            Sp_Farol_CotaResult tFarolCotas = db.Sp_Farol_Cota(AjustaData(data),
                                                               AjustaData(dataAnoAnt),
                                                               AjustaData(dataInicioSemana),
                                                               AjustaData(dataInicioSemanaAnoAnt),
                                                               AjustaData(dataInicioMes),
                                                               AjustaData(dataInicioMesAnoAnt),
                                                               AjustaData(dataInicioTri),
                                                               AjustaData(dataInicioTriAnoAnt)).SingleOrDefault();

            farol.Vl_cota_dia = (Convert.ToDouble(tFarolCotas.Vl_Cota_Dia)).ToString("##,###.##");

            if (Convert.ToDouble(farol.Vl_medio_dia) > 0)
                farol.Qt_cota_dia = Convert.ToInt32(Convert.ToDouble(tFarolCotas.Vl_Cota_Dia) / Convert.ToDouble(farol.Vl_medio_dia));

            farol.Vl_cota_dia_ano_ant = (Convert.ToDouble(tFarolCotas.Vl_Cota_Dia_Ano_Ant)).ToString("##,###.##");

            if (Convert.ToDouble(farol.Vl_medio_dia_ano_ant) > 0)
                farol.Qt_cota_dia_ano_ant = Convert.ToInt32(Convert.ToDouble(tFarolCotas.Vl_Cota_Dia_Ano_Ant) / Convert.ToDouble(farol.Vl_medio_dia_ano_ant));

            farol.Vl_cota_sem = (Convert.ToDouble(tFarolCotas.Vl_Cota_Sem)).ToString("##,###.##");

            if (Convert.ToDouble(farol.Vl_medio_sem) > 0)
                farol.Qt_cota_sem = Convert.ToInt32(Convert.ToDouble(tFarolCotas.Vl_Cota_Sem) / Convert.ToDouble(farol.Vl_medio_sem));

            farol.Vl_cota_sem_ano_ant = (Convert.ToDouble(tFarolCotas.Vl_Cota_Sem_Ano_Ant)).ToString("##,###.##");

            if (Convert.ToDouble(farol.Vl_medio_sem_ano_ant) > 0)
                farol.Qt_cota_sem_ano_ant = Convert.ToInt32(Convert.ToDouble(tFarolCotas.Vl_Cota_Sem_Ano_Ant) / Convert.ToDouble(farol.Vl_medio_sem_ano_ant));

            farol.Vl_cota_mes = (Convert.ToDouble(tFarolCotas.Vl_Cota_Mes)).ToString("##,###.##");

            if (Convert.ToDouble(farol.Vl_medio_mes) > 0)
                farol.Qt_cota_mes = Convert.ToInt32(Convert.ToDouble(tFarolCotas.Vl_Cota_Mes) / Convert.ToDouble(farol.Vl_medio_mes));

            farol.Vl_cota_mes_ano_ant = (Convert.ToDouble(tFarolCotas.Vl_Cota_Mes_Ano_Ant)).ToString("##,###.##");

            if (Convert.ToDouble(farol.Vl_medio_mes_ano_ant) > 0)
                farol.Qt_cota_mes_ano_ant = Convert.ToInt32(Convert.ToDouble(tFarolCotas.Vl_Cota_Mes_Ano_Ant) / Convert.ToDouble(farol.Vl_medio_mes_ano_ant));

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Dia) > 0)
                farol.Perc_at_dia = (((Convert.ToDouble(tFarol.Vl_Venda_Dia) / Convert.ToDouble(tFarolCotas.Vl_Cota_Dia)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Sem) > 0)
                farol.Perc_at_sem = (((Convert.ToDouble(tFarol.Vl_Venda_Sem) / Convert.ToDouble(tFarolCotas.Vl_Cota_Sem)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Mes) > 0)
                farol.Perc_at_mes = (((Convert.ToDouble(tFarol.Vl_Venda_Mes) / Convert.ToDouble(tFarolCotas.Vl_Cota_Mes)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Dia_Ano_Ant) > 0)
                farol.Perc_at_dia_ano_ant = (((Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant) / Convert.ToDouble(tFarolCotas.Vl_Cota_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Sem_Ano_Ant) > 0)
                farol.Perc_at_sem_ano_ant = (((Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant) / Convert.ToDouble(tFarolCotas.Vl_Cota_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Mes_Ano_Ant) > 0)
                farol.Perc_at_mes_ano_ant = (((Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant) / Convert.ToDouble(tFarolCotas.Vl_Cota_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Dia_Ano_Ant) > 0)
                farol.Perc_at_dia_cota = (((Convert.ToDouble(tFarolCotas.Vl_Cota_Dia) / Convert.ToDouble(tFarolCotas.Vl_Cota_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant) > 0)
                farol.Perc_at_dia_venda = (((Convert.ToDouble(tFarol.Vl_Venda_Dia) / Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Sem_Ano_Ant) > 0)
                farol.Perc_at_sem_cota = (((Convert.ToDouble(tFarolCotas.Vl_Cota_Sem) / Convert.ToDouble(tFarolCotas.Vl_Cota_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant) > 0)
                farol.Perc_at_sem_venda = (((Convert.ToDouble(tFarol.Vl_Venda_Sem) / Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Mes_Ano_Ant) > 0)
                farol.Perc_at_mes_cota = (((Convert.ToDouble(tFarolCotas.Vl_Cota_Mes) / Convert.ToDouble(tFarolCotas.Vl_Cota_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant) > 0)
                farol.Perc_at_mes_venda = (((Convert.ToDouble(tFarol.Vl_Venda_Mes) / Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Tri_Ano_Ant) > 0)
                farol.Perc_venda_tri_at_ano = (((Convert.ToDouble(tFarol.Vl_Venda_Tri) / Convert.ToDouble(tFarol.Vl_Venda_Tri_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Tri_Ano_Ant) > 0)
                farol.Perc_cota_tri_at_ano = (((Convert.ToDouble(tFarolCotas.Vl_Cota_Tri) / Convert.ToDouble(tFarolCotas.Vl_Cota_Tri_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Tri) > 0)
                farol.Perc_venda_cota_tri = (((Convert.ToDouble(tFarol.Vl_Venda_Tri) / Convert.ToDouble(tFarolCotas.Vl_Cota_Tri)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarolCotas.Vl_Cota_Tri_Ano_Ant) > 0)
                farol.Perc_venda_cota_tri_ano_ant = (((Convert.ToDouble(tFarol.Vl_Venda_Tri_Ano_Ant) / Convert.ToDouble(tFarolCotas.Vl_Cota_Tri_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant) > 0)
                farol.Perc_qt_atendido_dia = (((Convert.ToDouble(tFarol.Vl_Venda_Dia) / Convert.ToDouble(tFarol.Vl_Venda_Dia_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant) > 0)
                farol.Perc_qt_atendido_sem = (((Convert.ToDouble(tFarol.Vl_Venda_Sem) / Convert.ToDouble(tFarol.Vl_Venda_Sem_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant) > 0)
                farol.Perc_qt_atendido_mes = (((Convert.ToDouble(tFarol.Vl_Venda_Mes) / Convert.ToDouble(tFarol.Vl_Venda_Mes_Ano_Ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(farol.Vl_medio_dia_ano_ant) > 0)
                farol.Perc_vl_medio_atendido_dia = (((Convert.ToDouble(farol.Vl_medio_dia) / Convert.ToDouble(farol.Vl_medio_dia_ano_ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(farol.Vl_medio_sem_ano_ant) > 0)
                farol.Perc_vl_medio_atendido_sem = (((Convert.ToDouble(farol.Vl_medio_sem) / Convert.ToDouble(farol.Vl_medio_sem_ano_ant)) - 1) * 100).ToString("N0") + "%";

            if (Convert.ToDouble(farol.Vl_medio_mes_ano_ant) > 0)
                farol.Perc_vl_medio_atendido_mes = (((Convert.ToDouble(farol.Vl_medio_mes) / Convert.ToDouble(farol.Vl_medio_mes_ano_ant)) - 1) * 100).ToString("N0") + "%";

            farol.Vl_cota_tri = (Convert.ToDouble(tFarolCotas.Vl_Cota_Tri)).ToString("##,###.##");
            farol.Vl_cota_tri_ano_ant = (Convert.ToDouble(tFarolCotas.Vl_Cota_Tri_Ano_Ant)).ToString("##,###.##");

            farois.Add(farol);

            return farois;
        }

        public List<PRODUTOS_GRIFFE> BuscaGriffes()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from pg in db.PRODUTOS_GRIFFEs select pg).ToList();
        }

        public PRODUTOS_GRIFFE BuscaGriffe(string COD_GRIFFE)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from pg in db.PRODUTOS_GRIFFEs where pg.COD_GRIFFE == COD_GRIFFE select pg).SingleOrDefault();
        }

        public Sp_Pega_Data_BancoResult BuscaDataBanco()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pdb in db.Sp_Pega_Data_Banco() select pdb).SingleOrDefault();
        }

        public Sp_Busca_Semana_454Result BuscaSemana454(string data)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from bs in db.Sp_Busca_Semana_454(AjustaData(data)) select bs).SingleOrDefault();
        }

        public List<PRODUTO> BuscaProdutos(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs where p.COLECAO.Equals(colecao) && p.COD_CATEGORIA.Equals("01") orderby p.PRODUTO1 select p).ToList();
        }

        public PRODUTOS_BARRA BuscaProdutoBarra(string codigoBarra)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pb in db.PRODUTOS_BARRAs where pb.CODIGO_BARRA.Equals(codigoBarra) select pb).SingleOrDefault();
        }

        public PRODUTOS_BARRA BuscaProdutoBarraGrade(string codigoProduto, string corProduto, string gradeProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pb in db.PRODUTOS_BARRAs where pb.PRODUTO.Equals(codigoProduto) && pb.COR_PRODUTO.Equals(corProduto) && pb.GRADE.Equals(gradeProduto) select pb).SingleOrDefault();
        }

        public PRODUTOS_BARRA BuscaProdutoBarraGradeTamanho(string codigoProduto, string corProduto, string gradeProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pb in db.PRODUTOS_BARRAs where pb.PRODUTO.Equals(codigoProduto) && pb.COR_PRODUTO.Equals(corProduto) && pb.TAMANHO == Convert.ToInt32(gradeProduto) select pb).SingleOrDefault();
        }

        public List<PRODUTO> BuscaProdutos(string colecao, string grupo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs where p.COLECAO.Equals(colecao) && p.COD_CATEGORIA.Equals("01") && p.GRUPO_PRODUTO.Equals(grupo) select p).ToList();
        }

        public List<PRODUTO> BuscaProdutos()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs orderby p.DESC_PRODUTO select p).ToList();
        }

        public List<PRODUTO> BuscaProdutosDescricao(string desc_produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from p in db.PRODUTOs where p.DESC_PRODUTO.Trim().Contains(desc_produto.ToUpper().Trim()) select p).ToList();
        }

        public PRODUTO BuscaProduto(string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from p in db.PRODUTOs where p.PRODUTO1.Equals(codigoProduto) select p).Take(1).SingleOrDefault();
        }

        public FUNDO_FIXO_CONTA_DESPESA BuscaDespesa(string codigoDespesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffcd in db.FUNDO_FIXO_CONTA_DESPESAs where ffcd.CODIGO.Equals(codigoDespesa) select ffcd).SingleOrDefault();
        }

        public FUNDO_FIXO_CONTA_RECEITA BuscaReceita(string codigoReceita)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ffcr in db.FUNDO_FIXO_CONTA_RECEITAs where ffcr.CODIGO.Equals(codigoReceita) select ffcr).SingleOrDefault();
        }

        public IMPORTACAO_PROFORMA_PRODUTO BuscaProformaProduto(int codigoProformaProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_PROFORMA_PRODUTO == codigoProformaProduto select pp).SingleOrDefault();
        }

        public IMPORTACAO_NEL_PRODUTO BuscaNelProduto(int codigoNelProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from np in db.IMPORTACAO_NEL_PRODUTOs where np.CODIGO_NEL_PRODUTO == codigoNelProduto select np).SingleOrDefault();
        }

        public ESTOQUE_SOBRA BuscaEstoqueSobra(int codigoEstoqueSobra)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from es in db.ESTOQUE_SOBRAs where es.CODIGO == codigoEstoqueSobra select es).SingleOrDefault();
        }

        public PRODUTO BuscaProdutoBancoTeste(string codigoProduto)
        {
            db = new DCDataContext(Constante.TesteConnectionString);

            return (from p in db.PRODUTOs where p.PRODUTO1.Equals(codigoProduto) select p).SingleOrDefault();
        }

        public PRODUTOS_FOTO BuscaFotoProduto(string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pf in db.PRODUTOS_FOTOs where pf.PRODUTO1.Equals(codigoProduto) select pf).SingleOrDefault();
        }

        public List<PRODUTO_CORE> BuscaProdutoCores(string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTO_COREs where pc.PRODUTO.Equals(codigoProduto) orderby pc.DESC_COR_PRODUTO select pc).ToList();
        }

        public Sp_Busca_Produto_CorResult BuscaProdutoCor(string codigoProduto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.Sp_Busca_Produto_Cor(codigoProduto, cor) select pc).SingleOrDefault();
        }

        public PRODUTO_CORE BuscaProdutoCores(string codigoProduto, string descCorProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTO_COREs where pc.PRODUTO.Trim().Equals(codigoProduto) & pc.DESC_COR_PRODUTO.Trim().Equals(descCorProduto) select pc).SingleOrDefault();
        }

        public PRODUTO_CORE BuscaProdutoCorCodigo(string codigoProduto, string Cor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTO_COREs where pc.PRODUTO.Equals(codigoProduto) & pc.COR_PRODUTO.Equals(Cor) select pc).SingleOrDefault();
        }

        public PRODUTO_CORE BuscaProdutoCor(string codigoProdutoCor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTO_COREs where pc.COR_PRODUTO.Equals(codigoProdutoCor) select pc).SingleOrDefault();
        }

        public PRODUTO_CORE BuscaProdutoCorDescricao(string codigoProduto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTO_COREs where pc.PRODUTO.Equals(codigoProduto) & pc.DESC_COR_PRODUTO.Trim().Equals(cor) select pc).SingleOrDefault();
        }

        public Sp_Preco_GrupoResult BuscaPrecoGrupo(string tipoPreco, string grupo, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pg in db.Sp_Preco_Grupo(tipoPreco, grupo, colecao) select pg).SingleOrDefault();
        }

        public PRODUTOS_PRECO BuscaPrecoOriginalProduto(string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.PRODUTOS_PRECOs where pp.PRODUTO.Trim().Equals(produto.Trim()) & pp.CODIGO_TAB_PRECO.Equals("TO") select pp).Take(1).SingleOrDefault();
        }

        public PRODUTOS_PRECO BuscaPrecoLojaProduto(string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.PRODUTOS_PRECOs where pp.PRODUTO.Trim().Equals(produto.Trim()) & pp.CODIGO_TAB_PRECO.Equals("TL") select pp).Take(1).SingleOrDefault();
        }

        public List<PRODUTOS_PRECO> BuscaPrecoProduto(string produto, string codigoTabela)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.PRODUTOS_PRECOs where pp.PRODUTO.Trim() == produto.Trim() && pp.CODIGO_TAB_PRECO == codigoTabela select pp).ToList();
        }

        public void InserirPrecoProduto(PRODUTOS_PRECO _produtoPreco)
        {
            db = new DCDataContext(Constante.ConnectionString);

            try
            {
                db.PRODUTOS_PRECOs.InsertOnSubmit(_produtoPreco);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPrecoProduto(string codigoProduto, string codigoTabela, decimal preco)
        {
            db = new DCDataContext(Constante.ConnectionString);
            List<PRODUTOS_PRECO> _produtoPreco = BuscaPrecoProduto(codigoProduto, codigoTabela);

            if (_produtoPreco != null && _produtoPreco.Count > 0)
            {
                foreach (PRODUTOS_PRECO pp in _produtoPreco)
                {
                    pp.PRECO1 = preco;
                    pp.PRECO_LIQUIDO1 = preco;
                    pp.ULT_ATUALIZACAO = DateTime.Now.Date;
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
        }

        public Sp_Preco_GriffeResult BuscaPrecoGriffe(string tipoPreco, string griffe, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from bpg in db.Sp_Preco_Griffe(tipoPreco, griffe, colecao) select bpg).SingleOrDefault();
        }

        public Sp_Preco_SubGrupoResult BuscaPrecoSubGrupo(string tipoPreco, string subGrupo, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from psg in db.Sp_Preco_SubGrupo(tipoPreco, subGrupo, colecao) select psg).SingleOrDefault();
        }

        public PRODUTOS_PRECO BuscaPrecoTLProduto(string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.PRODUTOS_PRECOs where pp.PRODUTO.Equals(codigoProduto) && pp.CODIGO_TAB_PRECO.Equals("TL") select pp).SingleOrDefault();
        }

        public PRODUTOS_PRECO BuscaPrecoTAProduto(string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.PRODUTOS_PRECOs where pp.PRODUTO.Equals(codigoProduto) && pp.CODIGO_TAB_PRECO.Equals("TA") select pp).SingleOrDefault();
        }

        public PRODUTOS_PRECO BuscaPrecoTFProduto(string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pp in db.PRODUTOS_PRECOs where pp.PRODUTO.Equals(codigoProduto) && pp.CODIGO_TAB_PRECO.Equals("TF") select pp).SingleOrDefault();
        }

        public List<PRODUTOS_CATEGORIA> BuscaCategorias()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTOS_CATEGORIAs orderby pc.CATEGORIA_PRODUTO select pc).ToList();
        }

        public List<LX_CARREGA_DATAResult> BuscaDatas(int ano)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lcd in db.LX_CARREGA_DATA(ano) select lcd).ToList();
        }

        public List<PRODUTO_CORE> BuscaCores()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pc in db.PRODUTO_COREs select pc).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProduto(int colecao, int griffe, string grupo, int proforma)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs
                    where pp.CODIGO_COLECAO == colecao &&
                          pp.CODIGO_GRIFFE == griffe &&
                          pp.GRUPO_PRODUTO.Equals(grupo) &&
                          pp.CODIGO_PROFORMA == proforma
                    orderby pp.CODIGO_PRODUTO
                    select pp).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProformaProduto(int colecao, int griffe, string grupo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs
                    where pp.CODIGO_COLECAO == colecao &&
                          pp.CODIGO_GRIFFE == griffe &&
                          pp.GRUPO_PRODUTO.Equals(grupo)
                    orderby pp.CODIGO_PRODUTO
                    select pp).ToList();
        }

        public List<PRODUTOS_LINHA> BuscaLinhas()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pl in db.PRODUTOS_LINHAs select pl).ToList();
        }
        public PRODUTOS_LINHA BuscaLinhas(string linha)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pl in db.PRODUTOS_LINHAs where pl.LINHA.Trim() == linha.Trim() select pl).SingleOrDefault();
        }

        public List<PRODUTOS_TAMANHO> BuscaProdutosTamanho()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pl in db.PRODUTOS_TAMANHOs select pl).ToList();
        }

        public List<COLECOE> BuscaColecoesMateriais()
        {
            List<COLECOE> lista = new List<COLECOE>();

            db = new DCDataContext(Constante.ConnectionString);

            List<COLECOE> listaTodos = (from c in db.COLECOEs select c).ToList();

            if (listaTodos != null)
            {
                foreach (COLECOE item in listaTodos)
                {
                    if (item.COLECAO.Trim().Length >= 2 &&
                        !item.COLECAO.Trim().Contains("P") &&
                        !item.COLECAO.Trim().Contains("A") &&
                        !item.COLECAO.Trim().Contains("I") &&
                        !item.COLECAO.Trim().Contains("O") &&
                        !item.COLECAO.Trim().Contains("V") &&
                        !item.COLECAO.Trim().Contains("U") &&
                        !item.COLECAO.Trim().Contains("W") &&
                        !item.COLECAO.Trim().Contains("MK") &&
                        !item.COLECAO.Trim().Contains("ETC")
                        )
                    {
                        if ((Convert.ToInt32(item.COLECAO) >= 316) || ((Convert.ToInt32(item.COLECAO) >= 11) && (Convert.ToInt32(item.COLECAO) <= 70)))
                            lista.Add(item);
                    }
                }
            }
            return lista;

        }

        public List<SP_OBTER_COLECAO_ATACADOResult> ObterColecaoAtacado()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return dbLinx.SP_OBTER_COLECAO_ATACADO().ToList();
        }

        public List<COLECOE> BuscaColecoes()
        {
            List<COLECOE> lista = new List<COLECOE>();

            db = new DCDataContext(Constante.ConnectionString);

            List<COLECOE> listaTodos = (from c in db.COLECOEs select c).ToList();

            if (listaTodos != null)
            {
                foreach (COLECOE item in listaTodos)
                {
                    if (!item.COLECAO.Trim().Contains("ETC") &&
                        !item.COLECAO.Trim().Contains("MK") &&
                        !item.COLECAO.Trim().Contains("18A") &&
                        !item.COLECAO.Trim().Contains("20A") &&
                        !item.COLECAO.Trim().Contains("20P") &&
                        !item.COLECAO.Trim().Contains("P") &&
                        !item.COLECAO.Trim().Contains("O") &&
                        !item.COLECAO.Trim().Contains("I") &&
                        !item.COLECAO.Trim().Contains("V") &&
                        !item.COLECAO.Trim().Contains("U") &&
                        !item.COLECAO.Trim().Contains("W") &&
                        !item.COLECAO.Trim().Contains("A")
                        )
                    {
                        if ((Convert.ToInt32(item.COLECAO) > 18 && Convert.ToInt32(item.COLECAO) < 90) || Convert.ToInt32(item.COLECAO) == 100 || Convert.ToInt32(item.COLECAO) == 101 || Convert.ToInt32(item.COLECAO) == 94 || Convert.ToInt32(item.COLECAO) == 95 || Convert.ToInt32(item.COLECAO) == 93)
                            lista.Add(item);
                    }
                }
            }
            return lista;
        }

        public List<COLECOE> BuscaColecoesLinx()
        {
            List<COLECOE> lista = new List<COLECOE>();

            db = new DCDataContext(Constante.ConnectionString);

            List<COLECOE> listaTodos = (from c in db.COLECOEs select c).ToList();

            if (listaTodos != null)
            {
                foreach (COLECOE item in listaTodos)
                {
                    if (!item.COLECAO.Trim().Contains("ETC") &&
                        !item.COLECAO.Trim().Contains("MK") &&
                        !item.COLECAO.Trim().Contains("18A") &&
                        !item.COLECAO.Trim().Contains("20A") &&
                        !item.COLECAO.Trim().Contains("20P") &&
                        !item.COLECAO.Trim().Contains("P") &&
                        !item.COLECAO.Trim().Contains("O") &&
                        !item.COLECAO.Trim().Contains("I") &&
                        !item.COLECAO.Trim().Contains("V") &&
                        !item.COLECAO.Trim().Contains("U") &&
                        !item.COLECAO.Trim().Contains("W") &&
                        !item.COLECAO.Trim().Contains("A")
                        )
                    {
                        if ((Convert.ToInt32(item.COLECAO) > 18 && Convert.ToInt32(item.COLECAO) < 90) || Convert.ToInt32(item.COLECAO) == 100 || Convert.ToInt32(item.COLECAO) == 101 || Convert.ToInt32(item.COLECAO) == 94 || Convert.ToInt32(item.COLECAO) == 95 || Convert.ToInt32(item.COLECAO) == 93)
                            lista.Add(item);
                    }
                }
            }
            return lista;
        }

        public List<COLECOE> BuscaSubColecoes(string colecaoRaiz)
        {
            List<COLECOE> lista = new List<COLECOE>();

            db = new DCDataContext(Constante.ConnectionString);

            List<COLECOE> listaTodos = (from c in db.COLECOEs select c).ToList();

            if (listaTodos != null)
            {
                lista = listaTodos.Where(item => item.COLECAO.Trim().StartsWith(colecaoRaiz.Trim())
                    & !item.COLECAO.Trim().EndsWith(colecaoRaiz.Trim())).ToList();

            }

            return lista;
        }

        public List<FUNDO_FIXO_HISTORICO_DESPESA> BuscaDespesasEmAberto(int codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from hd in db.FUNDO_FIXO_HISTORICO_DESPESAs where hd.CODIGO_FILIAL == codigoFilial & hd.CODIGO_FECHAMENTO == 0 orderby hd.DATA select hd).ToList();
        }

        public PRODUTOS_GRUPO BuscaGrupoProduto(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pg in db.PRODUTOS_GRUPOs where pg.GRUPO_PRODUTO.Equals(descricao) select pg).SingleOrDefault();
        }

        public PRODUTOS_GRUPO BuscaGrupoProdutoPeloCodigo(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pg in db.PRODUTOS_GRUPOs where pg.CODIGO_GRUPO.Equals(codigo) select pg).SingleOrDefault();
        }

        public PRODUTOS_GRIFFE BuscaGriffeProduto(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pgp in db.PRODUTOS_GRIFFEs where pgp.GRIFFE.Equals(descricao) select pgp).SingleOrDefault();
        }

        public PRODUTOS_SUBGRUPO BuscaSubGrupoProduto(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from psg in db.PRODUTOS_SUBGRUPOs where psg.SUBGRUPO_PRODUTO.Equals(descricao) select psg).Take(1).SingleOrDefault();
        }

        public string ObterCodigoBarra(string produto, string corProduto, PROD_HB_GRADE hbGrade)
        {

            var produtoBarra = new PRODUTOS_BARRA();

            // encontra primeira grade com valor e assume q o código será dela
            string grade = "";
            if (hbGrade.GRADE_EXP > 0)
                grade = "EXP";
            else if (hbGrade.GRADE_XP > 0)
                grade = "XP";
            else if (hbGrade.GRADE_PP > 0)
                grade = "PP";
            else if (hbGrade.GRADE_P > 0)
                grade = "PQ";
            else if (hbGrade.GRADE_M > 0)
                grade = "MD";
            else if (hbGrade.GRADE_G > 0)
                grade = "GD";
            else if (hbGrade.GRADE_GG > 0)
                grade = "GG";

            // buscar baseano nas grades acima
            produtoBarra = BuscaProdutoBarra(produto, corProduto, grade);
            //nao encontrou, busca grade UNICA
            if (produtoBarra == null)
            {
                produtoBarra = BuscaProdutoBarra(produto, corProduto, "UN");
                //nao encontrou, buscar grade por numero
                if (produtoBarra == null)
                {
                    if (hbGrade.GRADE_EXP > 0)
                        grade = "32";
                    else if (hbGrade.GRADE_XP > 0)
                        grade = "34";
                    else if (hbGrade.GRADE_PP > 0)
                        grade = "36";
                    else if (hbGrade.GRADE_P > 0)
                        grade = "38";
                    else if (hbGrade.GRADE_M > 0)
                        grade = "40";
                    else if (hbGrade.GRADE_G > 0)
                        grade = "42";
                    else if (hbGrade.GRADE_GG > 0)
                        grade = "44";

                    produtoBarra = BuscaProdutoBarra(produto, corProduto, grade);
                }
            }

            if (produtoBarra != null)
            {
                return produtoBarra.CODIGO_BARRA.Trim();
            }

            return "";
        }

        public PRODUTOS_BARRA BuscaProdutoBarra(string produto, string corProduto, string grade)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pb in db.PRODUTOS_BARRAs where pb.PRODUTO.Trim().Equals(produto) & pb.COR_PRODUTO.Trim().Equals(corProduto) & pb.GRADE.Trim().Equals(grade) select pb).Take(1).SingleOrDefault();
        }

        public List<PRODUTOS_GRUPO> BuscaGrupos()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pg in db.PRODUTOS_GRUPOs select pg).ToList();
        }

        public PRODUTOS_GRUPO BuscaGrupo(string CODIGO_GRUPO)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pg in db.PRODUTOS_GRUPOs where pg.CODIGO_GRUPO == CODIGO_GRUPO select pg).SingleOrDefault();
        }

        public List<Sp_Busca_Grupos_ProdutoResult> BuscaGruposProduto(string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return db.Sp_Busca_Grupos_Produto(categoria, colecao).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProdutosProforma(int colecao, int janela, int proforma, int fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_COLECAO == colecao & pp.CODIGO_JANELA == janela & pp.CODIGO_PROFORMA == proforma & pp.CODIGO_FORNECEDOR == fornecedor select pp).ToList();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> BuscaProdutosProforma(int proforma)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from pp in db.IMPORTACAO_PROFORMA_PRODUTOs where pp.CODIGO_PROFORMA_DEFINIDO == proforma select pp).ToList();
        }

        public List<Sp_Busca_Produtos_Pecas_ColecaoResult> BuscaProdutosPecasColecao(string colecao, string griffe, string grupo, List<IMPORTACAO_PROFORMA_PRODUTO> listaProformaProduto)
        {
            List<Sp_Busca_Produtos_Pecas_ColecaoResult> listaProdutosLinxAtualizada = new List<Sp_Busca_Produtos_Pecas_ColecaoResult>();

            string subCategoria = "Original " + colecao;

            db = new DCDataContext(Constante.ConnectionString);

            List<Sp_Busca_Produtos_Pecas_ColecaoResult> listaProdutosLinx = db.Sp_Busca_Produtos_Pecas_Colecao(colecao, griffe, subCategoria, grupo).ToList();

            if (listaProformaProduto.Count > 0)
            {
                foreach (Sp_Busca_Produtos_Pecas_ColecaoResult itemLinx in listaProdutosLinx)
                {
                    Boolean ok = false;

                    foreach (IMPORTACAO_PROFORMA_PRODUTO itemProforma in listaProformaProduto)
                    {
                        if (Convert.ToInt32(itemLinx.CODIGO_PRODUTO) == itemProforma.CODIGO_PRODUTO & Convert.ToInt32(itemLinx.CODIGO_PRODUTO_COR) == itemProforma.CODIGO_PRODUTO_COR)
                        {
                            ok = true;
                            break;
                        }
                    }

                    if (!ok)
                        listaProdutosLinxAtualizada.Add(itemLinx);
                }

                return listaProdutosLinxAtualizada;
            }
            else
                return listaProdutosLinx;
        }

        public List<Sp_Busca_Produtos_Acessorios_ColecaoResult> BuscaProdutosAcessoriosColecao(string colecao, string griffe, string grupo)
        {
            string subCategoria = "Original " + colecao;

            db = new DCDataContext(Constante.ConnectionString);

            return db.Sp_Busca_Produtos_Acessorios_Colecao(colecao, griffe, subCategoria, grupo).ToList();
        }

        public Sp_Busca_Colecao_AtacadoResult BuscaColecaoAtacado(string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);

            return db.Sp_Busca_Colecao_Atacado(produto).Take(1).SingleOrDefault();
        }

        public List<Sp_Busca_SubGrupos_ProdutoResult> BuscaSubGruposProduto(string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from bsp in db.Sp_Busca_SubGrupos_Produto(categoria, colecao) select bsp).ToList();
        }

        public string BuscaDataInicio(string anoSemana)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.ANO_SEMANA_454.Equals(anoSemana) select d.DATA1).Min().ToString();
        }

        public string BuscaDataFim(string semanaAno)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.ANO_SEMANA_454.Equals(semanaAno) select d.DATA1).Max().ToString();
        }

        public DateTime BuscaDateTimeInicio(string anoSemana)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.ANO_SEMANA_454.Equals(anoSemana) select d.DATA1).Min();
        }

        public DateTime BuscaDateTimeFim(string semanaAno)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.ANO_SEMANA_454.Equals(semanaAno) select d.DATA1).Max();
        }

        public List<IMPORTACAO_PROFORMA_PRODUTO> ExisteProdutoJanela(int produto, int cor, int janela, int proforma, int colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from p in db.IMPORTACAO_PROFORMA_PRODUTOs
                    where p.CODIGO_PRODUTO == produto &
                          p.CODIGO_PRODUTO_COR == cor &
                          p.CODIGO_JANELA == janela &
                          p.CODIGO_PROFORMA == proforma &
                          p.CODIGO_COLECAO == colecao
                    select p).ToList();
        }

        public List<IMPORTACAO_NEL_PRODUTO> ExisteProdutoJanelaNel(int produto, int cor, int janela, int proforma, int colecao, int nel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from n in db.IMPORTACAO_NEL_PRODUTOs
                    where n.CODIGO_PRODUTO == produto &
                          n.CODIGO_PRODUTO_COR == cor &
                          n.CODIGO_JANELA == janela &
                          n.CODIGO_PROFORMA == proforma &
                          n.CODIGO_COLECAO == colecao &
                          n.CODIGO_NEL == nel
                    select n).ToList();
        }

        public List<IMPORTACAO_NEL> ExisteNel(int codigoNel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from n in db.IMPORTACAO_NELs where n.CODIGO_NEL == codigoNel select n).ToList();
        }

        public int BuscaUltimoCaixa(int loja)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CAIXAs where c.CODIGO_FILIAL == loja select c.CODIGO_CAIXA).Max();
        }

        public List<FECHAMENTO_ITEM_CAIXA> BuscaItensCaixa(int codigoCaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ic in db.FECHAMENTO_ITEM_CAIXAs where ic.CODIGO_CAIXA == codigoCaixa select ic).ToList();
        }

        public FECHAMENTO_ITEM_CAIXA BuscaItemCaixa(int codigoItemCaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ic in db.FECHAMENTO_ITEM_CAIXAs where ic.CODIGO_ITEM_CAIXA == codigoItemCaixa select ic).SingleOrDefault();
        }

        public FECHAMENTO_CAIXA BuscaCaixa(int codigoCaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CAIXAs where c.CODIGO_CAIXA == codigoCaixa select c).SingleOrDefault();
        }

        public FUNDO_FIXO_HISTORICO_RECEITA BuscaHistoricoReceita(FECHAMENTO_CAIXA caixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from r in db.FUNDO_FIXO_HISTORICO_RECEITAs where r.CODIGO_FILIAL == caixa.CODIGO_FILIAL & r.DATA.Date == caixa.DATA_FECHAMENTO.Date select r).SingleOrDefault();
        }

        public List<DEPOSITO_HISTORICO> BuscaHistoricoDeposito(FECHAMENTO_CAIXA caixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.DEPOSITO_HISTORICOs where d.CODIGO_FILIAL == caixa.CODIGO_FILIAL & d.DATA.Date == caixa.DATA_FECHAMENTO.Date select d).ToList();
        }

        public DEPOSITO_HISTORICO BuscaHistoricoDeposito(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.DEPOSITO_HISTORICOs where d.CODIGO == codigo select d).SingleOrDefault();
        }

        public DEPOSITO_COMPROVANTE BuscaDepositoComprovante(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.DEPOSITO_COMPROVANTEs where d.CODIGO == codigo select d).SingleOrDefault();
        }

        public DEPOSITO_SEMANA BuscaDepositoSemanaCodigoDeposito(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ds in db.DEPOSITO_SEMANAs where ds.CODIGO_DEPOSITO == codigo select ds).SingleOrDefault();
        }

        public int? BuscaUltimoNotaRetiradaItem(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada select nri.ITEM_PRODUTO).Max();
        }

        public NOTA_RETIRADA BuscaNotaRetirada(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nr in db.NOTA_RETIRADAs where nr.CODIGO_NOTA_RETIRADA == codigoNotaRetirada select nr).SingleOrDefault();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaListaNotaRetiradaItem(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada select nri).ToList();
        }

        public int? BuscaEstoqueFinalSemanaProduto(int anoSemana, int produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            return (from e in db.ESTOQUE_DATA_LOJAs where e.ANO_SEMANA == anoSemana select e.ESTOQUE_FINAL).Sum();
        }

        public decimal? BuscaVendasAgora(DateTime data, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from lv in db.LOJA_VENDAs
                    where lv.DATA_VENDA.Date == data.Date
                    && lv.CODIGO_FILIAL.Equals(filial)
                    && lv.TOTAL_QTDE_CANCELADA == 0
                    select lv.VALOR_PAGO).Sum();
        }

        public int? BuscaTicketsAgora(DateTime data, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from lv in db.LOJA_VENDAs
                    where lv.DATA_VENDA.Date == data.Date
                    && lv.CODIGO_FILIAL.Equals(filial)
                    && lv.TOTAL_QTDE_CANCELADA == 0
                    select lv).Count();
        }

        public int? BuscaTotalTicketsPeriodo(string filial, DateTime dataini, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from lv in db.LOJA_VENDAs
                    where lv.DATA_VENDA.Date >= dataini.Date
                    && lv.DATA_VENDA.Date <= dataFim.Date
                    && lv.CODIGO_FILIAL.Equals(filial)
                    && lv.TOTAL_QTDE_CANCELADA == 0
                    select lv).Count();
        }

        /*
        public int? BuscaQtAgora(DateTime data, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from lv in db.LOJA_VENDAs where lv.DATA_VENDA.Date == data.Date & lv.CODIGO_FILIAL.Equals(filial) select lv.QTDE_TOTAL-lv.QTDE_TROCA_TOTAL).Sum();
        }
        */
        public decimal? BuscaCotaAgora(DateTime data, string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            return (from cd in db.COTAS_DIAs where Convert.ToDateTime(cd.DATA).Date == data.Date & cd.CODIGO_LOJA.Equals(filial) select cd.COTA).SingleOrDefault();
        }

        public int? BuscaEstoqueFinalSemanaGriffe(int anoSemana, int categoria, int colecao, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            return (from e in db.ESTOQUE_DATA_LOJAs
                    where e.ANO_SEMANA == anoSemana &
                          e.CODIGO_CATEGORIA == categoria &
                          e.CODIGO_COLECAO == colecao &
                          e.GRIFFE.Equals(griffe)
                    select e.ESTOQUE_FINAL).Sum();
        }

        public int? BuscaEstoqueFinalSemanaGrupo(int anoSemana, int categoria, int colecao, string grupo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            return (from e in db.ESTOQUE_DATA_LOJAs
                    where e.ANO_SEMANA == anoSemana &
                          e.CODIGO_CATEGORIA == categoria &
                          e.CODIGO_COLECAO == colecao &
                          e.GRUPO.Equals(grupo)
                    select e.ESTOQUE_FINAL).Sum();
        }

        public int? BuscaEstoqueFinalSemanaSubGrupo(int anoSemana, int categoria, int colecao, string subgrupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from e in db.ESTOQUE_DATA_LOJAs
                    where e.ANO_SEMANA == anoSemana &
                          e.CODIGO_CATEGORIA == categoria &
                          e.CODIGO_COLECAO == colecao &
                          e.SUBGRUPO.Equals(subgrupo)
                    select e.ESTOQUE_FINAL).Sum();
        }

        public string BuscaDataInicioMes(int mes, string ano)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.MES454 == mes & d.ANO454.Equals(ano) select d.DATA1).Min().ToString();
        }

        public string BuscaDataFimMes(int mes, string ano)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.MES454 == mes & d.ANO454.Equals(ano) select d.DATA1).Max().ToString();
        }

        public DATAATACADO BuscaDataCorteAtacado()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from da in db.DATAATACADOs orderby da.DATA descending select da).Take(1).SingleOrDefault();
        }

        public ESTOQUE_SOBRA BuscaEstoqueSobra(string categoria, string colecao, string grupo, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from es in db.ESTOQUE_SOBRAs where es.CATEGORIA.Equals(categoria) & es.COLECAO.Equals(colecao) & es.GRUPO.Equals(grupo) & es.GRIFFE.Equals(griffe) select es).SingleOrDefault();
        }

        public List<PRODUTOS_SUBGRUPO> BuscaSubGrupos()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from psg in db.PRODUTOS_SUBGRUPOs select psg).ToList();
        }

        public List<PRODUTOS_TIPO> BuscaTipos()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from pt in db.PRODUTOS_TIPOs select pt).ToList();
        }

        public DATA BuscaDatas(string anoSemana)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.ANO_SEMANA_454.Equals(anoSemana) orderby d.DIA_DO_ANO454 select d).Take(1).SingleOrDefault();
        }

        public List<PRODUTO_GRUPO_PRODUTO> BuscaQtdeGrupoProduto(string griffe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from gp in db.PRODUTO_GRUPO_PRODUTOs where gp.GRIFFE.Equals(griffe) select gp).ToList();
        }

        public List<COTAS_SEMANA_GRUPO> BuscaCotasSemanaGrupo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from csg in db.COTAS_SEMANA_GRUPOs select csg).Distinct().ToList();
        }

        public List<string> RetornaDatasSemana(int mes, string ano, string diaSemana)
        {
            if (diaSemana.Equals("Segunda"))
                diaSemana = "Monday";
            if (diaSemana.Equals("Terca"))
                diaSemana = "Tuesday";
            if (diaSemana.Equals("Quarta"))
                diaSemana = "Wednesday";
            if (diaSemana.Equals("Quinta"))
                diaSemana = "Thursday";
            if (diaSemana.Equals("Sexta"))
                diaSemana = "Friday";
            if (diaSemana.Equals("Sabado"))
                diaSemana = "Saturday";
            if (diaSemana.Equals("Domingo"))
                diaSemana = "Sunday";

            List<string> datasSemana = new List<string>();
            string w_dia_semana;

            DateTime dataInicio = Convert.ToDateTime(BuscaDataInicioMes(mes, ano));
            DateTime dataFim = Convert.ToDateTime(BuscaDataFimMes(mes, ano));

            for (int i = 0; dataInicio <= dataFim; i++)
            {
                w_dia_semana = dataInicio.DayOfWeek.ToString();

                if (w_dia_semana.Equals(diaSemana))
                    datasSemana.Add(AjustaData(dataInicio.ToString()));

                dataInicio = dataInicio.AddDays(1);
            }

            return datasSemana;
        }

        public List<int?> BuscaSemanas(string anoSemana)
        {
            string ano = anoSemana.Substring(0, 4);
            int semana = Convert.ToInt32(anoSemana.Substring(5, 2));

            db = new DCDataContext(Constante.ConnectionString);

            return (from d in db.DATAs where d.SEMANA454 == semana && d.ANO454.Equals(ano) select d.SEMANA454).ToList();
        }

        public Sp_Vendas_PeriodoResult BuscaVendasPeriodo(string categoria, string colecao, string griffe, string grupo, string filial, string dataInicio1, string dataFim1, string dataInicio2, string dataFim2, string dataInicio3, string dataFim3, string dataInicio4, string dataFim4)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from vp in db.Sp_Vendas_Periodo(categoria, colecao, griffe, grupo, filial, dataInicio1, dataFim1, dataInicio2, dataFim2, dataInicio3, dataFim3, dataInicio4, dataFim4) select vp).SingleOrDefault();
        }

        public List<Sp_Line_Report_Venda_Grupo_Colecao_ProdutoResult> BuscaVendasGrupoColecao(string dataInicioSemana, string dataFimSemana, string categoria, string anoSemana, string griffe, string dataInicioMes, string dataFimMes, string dataInicioMesFechado, string dataFimMesFechado)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from vgcp in db.Sp_Line_Report_Venda_Grupo_Colecao_Produto(dataInicioSemana, dataFimSemana, categoria, anoSemana, griffe, dataInicioMes, dataFimMes, dataInicioMesFechado, dataFimMesFechado) select vgcp).ToList();
        }

        public List<Sp_Line_Report_ColecaoResult> BuscaVendasColecao(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string dataInicioMes, string dataFimMes, string dataInicioSemAnoAnt, string dataFimSemAnoAnt, string dataInicioMesAnoAnt, string dataFimMesAnoAnt, string colecaoAnoAnterior)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (db.Sp_Line_Report_Colecao(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimMes, dataInicioSemAnoAnt, dataFimSemAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior)).ToList();
        }

        public List<Sp_Line_Report_Colecao_SubResult> BuscaVendasColecaoSub(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string dataInicioMes, string dataFimMes, string dataInicioSemAnoAnt, string dataFimSemAnoAnt, string dataInicioMesAnoAnt, string dataFimMesAnoAnt, string colecaoAnoAnterior)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (db.Sp_Line_Report_Colecao_Sub(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimMes, dataInicioSemAnoAnt, dataFimSemAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior)).ToList();
        }

        public List<Sp_Line_Report_Colecao_AtualResult> BuscaVendasColecaoLiq(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string dataInicioMes, string dataFimMes, string dataInicioSemAnoAnt, string dataFimSemAnoAnt, string dataInicioMesAnoAnt, string dataFimMesAnoAnt, string colecaoAnoAnterior)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (db.Sp_Line_Report_Colecao_Atual(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimMes, dataInicioSemAnoAnt, dataFimSemAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior)).ToList();
        }

        public List<Sp_Line_Report_Colecao_Sub_AtualResult> BuscaVendasColecaoSubLiq(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string dataInicioMes, string dataFimMes, string dataInicioSemAnoAnt, string dataFimSemAnoAnt, string dataInicioMesAnoAnt, string dataFimMesAnoAnt, string colecaoAnoAnterior)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (db.Sp_Line_Report_Colecao_Sub_Atual(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimMes, dataInicioSemAnoAnt, dataFimSemAnoAnt, dataInicioMesAnoAnt, dataFimMesAnoAnt, colecaoAnoAnterior)).ToList();
        }

        public List<Sp_Line_Report_Colecao_LojaResult> BuscaVendasColecaoLoja(string dataInicioSemana,
                                                                              string dataFimSemana,
                                                                              string categoria,
                                                                              string colecao,
                                                                              string griffe,
                                                                              string dataInicioMes,
                                                                              string dataFimMes,
                                                                              string filial,
                                                                              string dataInicioSemanaAnoAnterior,
                                                                              string dataFimSemanaAnoAnterior,
                                                                              string dataInicioMesAnoAnterior,
                                                                              string dataFimMesAnoAnterior)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from lrcl in db.Sp_Line_Report_Colecao_Loja(dataInicioSemana,
                                                                dataFimSemana,
                                                                categoria,
                                                                colecao,
                                                                griffe,
                                                                dataInicioMes,
                                                                dataFimMes,
                                                                filial,
                                                                dataInicioSemanaAnoAnterior,
                                                                dataFimSemanaAnoAnterior,
                                                                dataInicioMesAnoAnterior,
                                                                dataFimMesAnoAnterior)
                    select lrcl).ToList();
        }

        public List<Sp_Line_Report_Colecao_Sub_LojaResult> BuscaVendasColecaoSubLoja(string dataInicioSemana,
                                                                                     string dataFimSemana,
                                                                                     string categoria,
                                                                                     string colecao,
                                                                                     string griffe,
                                                                                     string dataInicioMes,
                                                                                     string dataFimMes,
                                                                                     string filial,
                                                                                     string dataInicioSemanaAnoAnterior,
                                                                                     string dataFimSemanaAnoAnterior,
                                                                                     string dataInicioMesAnoAnterior,
                                                                                     string dataFimMesAnoAnterior)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from lrcl in db.Sp_Line_Report_Colecao_Sub_Loja(dataInicioSemana,
                                                                    dataFimSemana,
                                                                    categoria,
                                                                    colecao,
                                                                    griffe,
                                                                    dataInicioMes,
                                                                    dataFimMes,
                                                                    filial,
                                                                    dataInicioSemanaAnoAnterior,
                                                                    dataFimSemanaAnoAnterior,
                                                                    dataInicioMesAnoAnterior,
                                                                    dataFimMesAnoAnterior)
                    select lrcl).ToList();
        }

        public List<Sp_Line_Report_Colecao_FranquiaResult> BuscaVendasColecaoFranquia(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string dataInicioMes, string dataFimMes)
        {
            db = new DCDataContext(Constante.ConnectionStringFranquia);
            db.CommandTimeout = 500;

            return (from lrc in db.Sp_Line_Report_Colecao_Franquia(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimMes) select lrc).ToList();
        }

        public List<Sp_Line_Report_Colecao_Sub_FranquiaResult> BuscaVendasColecaoSubFranquia(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string dataInicioMes, string dataFimMes)
        {
            db = new DCDataContext(Constante.ConnectionStringFranquia);
            db.CommandTimeout = 500;

            return (from lrc in db.Sp_Line_Report_Colecao_Sub_Franquia(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, dataInicioMes, dataFimMes) select lrc).ToList();
        }

        public Sp_Busca_Vendas_Franquia_GrupoResult BuscaVendasFranquiaGrupo(string dataInicioSemana, string dataFimSemana, string grupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringFranquia);
            db.CommandTimeout = 500;

            return (from bvfg in db.Sp_Busca_Vendas_Franquia_Grupo(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), grupo, categoria, colecao) select bvfg).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Franquia_GriffeResult BuscaVendasFranquiaGriffe(string dataInicioSemana, string dataFimSemana, string griffe, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringFranquia);
            db.CommandTimeout = 500;

            return (from bvfg in db.Sp_Busca_Vendas_Franquia_Griffe(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), griffe, categoria, colecao) select bvfg).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Franquia_SubGrupoResult BuscaVendasFranquiaSubGrupo(string dataInicioSemana, string dataFimSemana, string subGrupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringFranquia);
            db.CommandTimeout = 500;

            return (from bvfg in db.Sp_Busca_Vendas_Franquia_SubGrupo(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), subGrupo, categoria, colecao) select bvfg).SingleOrDefault();
        }

        public Sp_Busca_Vendas_FranquiaResult BuscaVendasFranquia(string dataInicioSemana, string dataFimSemana, string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringFranquia);

            return (from bvf in db.Sp_Busca_Vendas_Franquia(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), codigoProduto) select bvf).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Varejo_GrupoResult BuscaVendasVarejoGrupo(string dataInicioSemana, string dataFimSemana, string grupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bvvg in db.Sp_Busca_Vendas_Varejo_Grupo(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), grupo, categoria, colecao) select bvvg).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Varejo_GriffeResult BuscaVendasVarejoGriffe(string dataInicioSemana, string dataFimSemana, string griffe, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from bvvg in db.Sp_Busca_Vendas_Varejo_Griffe(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), griffe, categoria, colecao) select bvvg).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Varejo_SubGrupoResult BuscaVendasVarejoSubGrupo(string dataInicioSemana, string dataFimSemana, string subGrupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bvvg in db.Sp_Busca_Vendas_Varejo_SubGrupo(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), subGrupo, categoria, colecao) select bvvg).SingleOrDefault();
        }

        public Sp_Busca_Vendas_VarejoResult BuscaVendasVarejo(string dataInicioSemana, string dataFimSemana, string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from bvv in db.Sp_Busca_Vendas_Varejo(AjustaData(dataInicioSemana), AjustaData(dataFimSemana), codigoProduto) select bvv).SingleOrDefault();
        }

        public List<Sp_Line_Report_Franquia_ColecaoResult> BuscaFranquiaVendasColecao(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string griffe, string grupo, string dataInicioMes, string dataFimMes, string dataInicioMesFechado, string dataFimMesFechado)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from lrc in db.Sp_Line_Report_Franquia_Colecao(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, grupo, dataInicioMes, dataFimMes, dataInicioMesFechado, dataFimMesFechado) select lrc).ToList();
        }

        public List<Sp_Line_Report_Franquia_Colecao_SubResult> BuscaFranquiaVendasColecaoSub(string dataInicioSemana, string dataFimSemana, string categoria, string colecao, string subgrupo, string griffe, string dataInicioMes, string dataFimMes, string dataInicioMesFechado, string dataFimMesFechado)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from lrc in db.Sp_Line_Report_Franquia_Colecao_Sub(dataInicioSemana, dataFimSemana, categoria, colecao, griffe, subgrupo, dataInicioMes, dataFimMes, dataInicioMesFechado, dataFimMesFechado) select lrc).ToList();
        }
        /*
        public Sp_Line_Report_Estoque_Semana_Fechada_Grupo_ProdutoResult BuscaEstoqueSemanaFechada(string dataInicioSemana, string dataFimSemana, string grupo, string griffe, string colecao, string categoria)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return db.Sp_Line_Report_Estoque_Semana_Fechada_Grupo_Produto(dataInicioSemana, dataFimSemana, grupo, griffe, colecao, categoria).SingleOrDefault();
        }
        */
        public Sp_Estoque_Semana_Fechada_Grupo_ProdutoResult BuscaEstoqueSemanaFechada(string grupo, string griffe, string colecao, string categoria)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return db.Sp_Estoque_Semana_Fechada_Grupo_Produto(grupo, griffe, colecao, categoria).SingleOrDefault();
        }

        public void BuscaEstoqueSemanaBase(string dataInicioSemana, string dataFimSemana)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            db.Sp_Estoque_Semana_Fechada_Base(dataInicioSemana, dataFimSemana);
        }

        public ESTOQUE_DATA BuscaEstoqueSemanaFechadaNovo(string anoSemana, string grupo, string griffe, string colecao, string categoria)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 500;

            return (from ed in db.ESTOQUE_DATAs
                    where ed.ANO_SEMANA.Equals(anoSemana) &
                          ed.GRUPO.Equals(grupo) &
                          ed.GRIFFE.Equals(griffe) &
                          ed.COLECAO.Equals(colecao) &
                          ed.CATEGORIA.Equals(categoria)
                    select ed).SingleOrDefault();
        }

        public Sp_Busca_Estoque_Final_Data_GrupoResult BuscaEstoqueFinalGrupo(string data, string grupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from befdg in db.Sp_Busca_Estoque_Final_Data_Grupo(AjustaData(data), grupo, categoria, colecao) select befdg).SingleOrDefault();
        }

        public Sp_Busca_Estoque_Final_Data_GriffeResult BuscaEstoqueFinalGriffe(string data, string griffe, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from befdg in db.Sp_Busca_Estoque_Final_Data_Griffe(AjustaData(data), griffe, categoria, colecao) select befdg).SingleOrDefault();
        }

        public Sp_Busca_Estoque_Final_Data_SubGrupoResult BuscaEstoqueFinalSubGrupo(string data, string subGrupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from befdg in db.Sp_Busca_Estoque_Final_Data_SubGrupo(AjustaData(data), subGrupo, categoria, colecao) select befdg).SingleOrDefault();
        }

        public Sp_Busca_Estoque_Final_DataResult BuscaEstoqueFinalSemana(string data, string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from befd in db.Sp_Busca_Estoque_Final_Data(AjustaData(data), produto) select befd).SingleOrDefault();
        }
        /*
        public Sp_Line_Report_Estoque_Semana_Fechada_Grupo_Produto_SubResult BuscaEstoqueSemanaFechadaSub(string dataInicioSemana, string dataFimSemana, string grupo, string griffe, string colecao, string categoria)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return db.Sp_Line_Report_Estoque_Semana_Fechada_Grupo_Produto_Sub(dataInicioSemana, dataFimSemana, grupo, griffe, colecao, categoria).SingleOrDefault();
        }
        */
        public Sp_Estoque_Semana_Fechada_Grupo_Produto_SubResult BuscaEstoqueSemanaFechadaSub(string grupo, string griffe, string colecao, string categoria)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return db.Sp_Estoque_Semana_Fechada_Grupo_Produto_Sub(grupo, griffe, colecao, categoria).SingleOrDefault();
        }

        public Sp_Line_Report_Estoque_Semana_Fechada_LojaResult BuscaEstoqueSemanaFechadaLoja(string dataInicioSemana, string dataFimSemana, string grupo, string griffe, string colecao, string categoria, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from esfl in db.Sp_Line_Report_Estoque_Semana_Fechada_Loja(dataInicioSemana, dataFimSemana, grupo, griffe, colecao, categoria, filial) select esfl).SingleOrDefault();
        }

        public Sp_Line_Report_Estoque_Semana_Fechada_Loja_SubResult BuscaEstoqueSemanaFechadaLojaSub(string dataInicioSemana, string dataFimSemana, string grupo, string griffe, string colecao, string categoria, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from esfl in db.Sp_Line_Report_Estoque_Semana_Fechada_Loja_Sub(dataInicioSemana, dataFimSemana, grupo, griffe, colecao, categoria, filial) select esfl).SingleOrDefault();
        }

        public Sp_Line_Report_Estoque_Valor_Grupo_ProdutoResult BuscaEstoqueValor(string grupo, string griffe, string colecao, string tipoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from ev in db.Sp_Line_Report_Estoque_Valor_Grupo_Produto(grupo, griffe, colecao, tipoFilial) select ev).SingleOrDefault();
        }

        public Sp_Line_Report_Estoque_Valor_Grupo_Produto_SubResult BuscaEstoqueValorSub(string grupo, string griffe, string colecao, string tipoFilial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from ev in db.Sp_Line_Report_Estoque_Valor_Grupo_Produto_Sub(grupo, griffe, colecao, tipoFilial) select ev).SingleOrDefault();
        }

        public Sp_Branch_Report_Cota_LojaResult BuscaVendasLoja(string filial, string dataInicioSemana, string dataFimSemana, string dataInicioMes, string dataFimMes)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from brc in db.Sp_Branch_Report_Cota_Loja(filial, dataInicioSemana, dataFimSemana, dataInicioMes, dataFimMes) select brc).SingleOrDefault();
        }

        public List<VendasCotasLoja> BuscaVendasCotasLoja(DateTime dataInicio, DateTime dataFim)
        {
            List<VendasCotasLoja> listaVendasCotasLoja = new List<VendasCotasLoja>();
            FILIAIS_DE_PARA filial_de_para = new FILIAIS_DE_PARA();
            string v_filial_antiga = string.Empty;

            List<FILIAI> filiais = BuscaFiliais();

            List<FILIAIS_DE_PARA> dePara = new List<FILIAIS_DE_PARA>();
            dePara = BuscaFilialDePara();

            if (filiais != null)
            {
                VendasCotasLoja vendasCotasLoja = null;

                foreach (FILIAI filial in filiais)
                {
                    if (dePara.Where(p => p.DE.Trim() == filial.COD_FILIAL.Trim()).Count() <= 0)
                    {
                        vendasCotasLoja = new VendasCotasLoja();

                        vendasCotasLoja.Filial = filial.FILIAL;

                        LOJA_VENDEDORE gerente = BuscaGerenteLoja(filial.COD_FILIAL);

                        if (gerente != null)
                            vendasCotasLoja.Gerente = gerente.VENDEDOR_APELIDO;

                        Sp_Busca_Supervisor_LojaResult supervisor = BuscaSupervisorLoja(filial.COD_FILIAL);

                        if (supervisor != null)
                        {
                            USUARIO usuario = BuscaUsuario(supervisor.codigo_usuario);

                            if (usuario != null)
                                vendasCotasLoja.Supervisor = usuario.NOME_USUARIO;
                        }

                        decimal? venda = BuscaTotalVendasPeriodo(filial.COD_FILIAL, dataInicio, dataFim);

                        //obter filial antiga
                        //15/08/2014 - Leandro Bevilaqua
                        filial_de_para = BuscaFilialDePara(filial.COD_FILIAL);
                        if (filial_de_para != null)
                        {
                            v_filial_antiga = filial_de_para.DE;

                            decimal? venda_filial_antiga = BuscaTotalVendasPeriodo(v_filial_antiga, dataInicio, dataFim);
                            if (venda_filial_antiga != null)
                                venda += venda_filial_antiga;
                        }

                        if (venda == null)
                            venda = 0;

                        decimal? cota = BuscaTotalCotasPeriodo(filial.COD_FILIAL, dataInicio, dataFim);

                        if (cota == null)
                            cota = 0;

                        decimal? vendaAnoAnterior = BuscaTotalVendasPeriodo(filial.COD_FILIAL, dataInicio.AddYears(-1), dataFim.AddYears(-1));

                        if (vendaAnoAnterior == null)
                            vendaAnoAnterior = 0;

                        decimal? cotaAnoAnterior = BuscaTotalCotasPeriodo(filial.COD_FILIAL, dataInicio.AddYears(-1), dataFim.AddYears(-1));

                        if (cotaAnoAnterior == null)
                            cotaAnoAnterior = 0;

                        if (cota > 0)
                        {
                            vendasCotasLoja.PercAtingido = Convert.ToDecimal((venda / cota) * 100);
                            vendasCotasLoja.ValAtingido = Convert.ToDecimal(venda);
                            vendasCotasLoja.ValCota = Convert.ToDecimal(cota);
                        }
                        else
                        {
                            vendasCotasLoja.PercAtingido = 0;
                            vendasCotasLoja.ValAtingido = 0;
                        }

                        if (vendasCotasLoja.PercAtingido > 90)
                            vendasCotasLoja.CodigoFarol = 1;

                        if (vendasCotasLoja.PercAtingido > 80 & vendasCotasLoja.PercAtingido < 91)
                            vendasCotasLoja.CodigoFarol = 3;

                        if (vendasCotasLoja.PercAtingido > 70 & vendasCotasLoja.PercAtingido < 81)
                            vendasCotasLoja.CodigoFarol = 4;

                        if (vendasCotasLoja.PercAtingido < 71)
                            vendasCotasLoja.CodigoFarol = 5;

                        if (cotaAnoAnterior > 0)
                            vendasCotasLoja.PercAtingidoAnoAnterior = Convert.ToDecimal((vendaAnoAnterior / cotaAnoAnterior) * 100);
                        else
                            vendasCotasLoja.PercAtingidoAnoAnterior = 0;

                        if (vendasCotasLoja.PercAtingido > vendasCotasLoja.PercAtingidoAnoAnterior)
                            vendasCotasLoja.Sobe = true;
                        else
                            vendasCotasLoja.Sobe = false;

                        listaVendasCotasLoja.Add(vendasCotasLoja);
                    }
                }
            }

            return listaVendasCotasLoja;
        }


        public SP_OBTER_NF_SAIDAResult ObterNFSaida(string nota, string serie, string emissao)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);

            return (from s in db.SP_OBTER_NF_SAIDA(nota, serie, AjustaData(emissao)) select s).SingleOrDefault();
        }

        public SP_OBTER_PRODUTO_DATAFOILOJAResult ObterProdutoDataFoiLoja(string produto, string cor)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return (from s in dbLinx.SP_OBTER_PRODUTO_DATAFOILOJA(produto, cor) select s).SingleOrDefault();
        }

        public List<SP_BUSCAR_NF_RETAGUARDAResult> BuscaNotasRetaguarda(string filial, string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from s in db.SP_BUSCAR_NF_RETAGUARDA(filial, AjustaData(dataInicio), AjustaData(dataFim)) select s).ToList();
        }

        public List<SP_BUSCAR_NF_PORTALResult> BuscaNotasPortal(string filial, string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from s in db.SP_BUSCAR_NF_PORTAL(filial, AjustaData(dataInicio), AjustaData(dataFim)) select s).ToList();
        }

        public List<VendasCotas> BuscaVendasLoja(string filial, DateTime dataInicio, DateTime dataFim)
        {
            List<VendasCotas> vendasCotas = new List<VendasCotas>();
            FILIAIS_DE_PARA filial_de_para = new FILIAIS_DE_PARA();
            string v_filial_antiga = string.Empty;

            //obter filial antiga
            //15/08/2014 - Leandro Bevilaqua
            filial_de_para = BuscaFilialDePara(filial);
            if (filial_de_para != null)
            {
                v_filial_antiga = filial_de_para.DE;
            }

            List<COTAS_DIA> cotas = BuscaCotas(filial, dataInicio, dataFim);

            if (cotas != null)
            {
                foreach (COTAS_DIA cota in cotas)
                {
                    VendasCotas vendaCotas = new VendasCotas();

                    vendaCotas.Data = Convert.ToDateTime(cota.DATA).ToString("dd/MM/yyyy");

                    decimal? valorVenda = BuscaVenda(filial, Convert.ToDateTime(cota.DATA));

                    if (valorVenda == null)
                    {
                        //Se não encontrou, verifica se tem filial antiga
                        valorVenda = BuscaVenda(v_filial_antiga, Convert.ToDateTime(cota.DATA));

                        if (valorVenda == null)
                        {
                            vendaCotas.ValorVenda = 0;
                        }
                        else
                        {
                            vendaCotas.ValorVenda = Convert.ToDecimal(valorVenda);
                        }
                    }
                    else
                    {
                        vendaCotas.ValorVenda = Convert.ToDecimal(valorVenda);
                    }

                    vendaCotas.ValorCota = Convert.ToDecimal(cota.COTA);

                    if (vendaCotas.ValorCota > 0)
                        vendaCotas.PercAtingido = (vendaCotas.ValorVenda / vendaCotas.ValorCota) * 100;
                    else
                        vendaCotas.PercAtingido = 0;

                    vendasCotas.Add(vendaCotas);
                }

                return vendasCotas;
            }

            return null;
        }

        public List<VendasCotas> BuscaVendasGrupo2(List<FILIAI> filiais, DateTime dataInicio, DateTime dataFim)
        {
            DateTime dataTrab = dataInicio;
            String v_filial_antiga = string.Empty;

            List<VendasCotas> vendasCotas = new List<VendasCotas>();
            FILIAIS_DE_PARA filial_de_para = new FILIAIS_DE_PARA();

            for (int i = 0; dataTrab <= dataFim; i++)
            {
                VendasCotas vendaCotas = new VendasCotas();

                vendaCotas.Data = dataTrab.ToString("dd/MM/yyyy");

                if (filiais != null)
                {
                    foreach (FILIAI filial in filiais)
                    {
                        decimal? valorVenda = BuscaVenda(filial.COD_FILIAL, dataTrab);

                        if (valorVenda == null)
                        {
                            //obter filial antiga
                            //15/08/2014 - Leandro Bevilaqua
                            filial_de_para = BuscaFilialDePara(filial.COD_FILIAL);
                            if (filial_de_para != null)
                            {
                                v_filial_antiga = filial_de_para.DE;
                                valorVenda = BuscaVenda(v_filial_antiga, dataTrab);
                            }
                        }

                        if (valorVenda == null)
                            vendaCotas.ValorVenda += 0;
                        else
                            vendaCotas.ValorVenda += Convert.ToDecimal(valorVenda);

                        decimal? valorCota = BuscaCota(filial.COD_FILIAL, dataTrab);

                        if (valorCota == null)
                            vendaCotas.ValorCota += 0;
                        else
                            vendaCotas.ValorCota += Convert.ToDecimal(valorCota);
                    }
                }

                if (vendaCotas.ValorCota > 0)
                    vendaCotas.PercAtingido += (vendaCotas.ValorVenda / vendaCotas.ValorCota) * 100;
                else
                    vendaCotas.PercAtingido += 0;

                vendasCotas.Add(vendaCotas);

                dataTrab = dataTrab.AddDays(1);
            }

            return vendasCotas;
        }

        public List<SP_OBTER_VENDASCOTASResult> ObterVendasCotas(string filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from aap in db.SP_OBTER_VENDASCOTAS(filial, dataInicio, dataFim) select aap).ToList();
        }

        public List<SP_OBTER_VENDASCOTAS_SEMVALEResult> ObterVendasCotasSemVale(string filial, DateTime dataInicio, DateTime dataFim)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from aap in dbIntra.SP_OBTER_VENDASCOTAS_SEMVALE(filial, dataInicio, dataFim) select aap).ToList();
        }

        public decimal? BuscaVenda(string filial, DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from v in db.LOJA_VENDAs where v.CODIGO_FILIAL.Equals(filial) & v.DATA_VENDA.Date == data.Date select v.VALOR_PAGO).Sum();
        }

        public decimal? BuscaTotalVendasPeriodo(string filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from v in db.LOJA_VENDAs where v.CODIGO_FILIAL.Equals(filial) & Convert.ToDateTime(v.DATA_VENDA).Date >= dataInicio.Date & Convert.ToDateTime(v.DATA_VENDA).Date <= dataFim.Date select v.VALOR_PAGO).Sum();
        }

        public decimal? BuscaVendaGeral(DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from v in db.LOJA_VENDAs where v.DATA_VENDA.Date == data.Date select v.VALOR_PAGO).Sum();
        }

        public decimal? BuscaCota(string filial, DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.COTAS_DIAs where c.CODIGO_LOJA.Equals(filial) & Convert.ToDateTime(c.DATA).Date == data.Date select c.COTA).Sum();
        }

        public decimal? BuscaCotaGeral(DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.COTAS_DIAs where Convert.ToDateTime(c.DATA).Date == data.Date select c.COTA).Sum();
        }

        public LOJA_VENDEDORE BuscaVendedor(string filial, string vendedor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where lv.CODIGO_FILIAL.Equals(filial) & lv.VENDEDOR.Equals(vendedor) select lv).SingleOrDefault();
        }

        public List<LOJA_VENDEDORE> BuscaVendedores(string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where lv.CODIGO_FILIAL.Equals(filial) & lv.DATA_DESATIVACAO == null select lv).ToList();
        }

        public LOJA_VENDEDORE BuscaVendedorPorCodigoEAtivo(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where lv.VENDEDOR.Equals(codigo) & lv.DATA_DESATIVACAO == null select lv).SingleOrDefault();
        }

        public LOJA_VENDEDORE BuscaVendedorPorCodigo(string codigo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDEDOREs where lv.VENDEDOR.Equals(codigo) select lv).SingleOrDefault();
        }

        public List<COTAS_DIA> BuscaCotas(string filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.COTAS_DIAs where c.CODIGO_LOJA.Equals(filial) & Convert.ToDateTime(c.DATA).Date >= dataInicio.Date & Convert.ToDateTime(c.DATA).Date <= dataFim.Date orderby c.DATA select c).ToList();
        }

        public decimal? BuscaTotalCotasPeriodo(string filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.COTAS_DIAs where c.CODIGO_LOJA.Equals(filial) & Convert.ToDateTime(c.DATA).Date >= dataInicio.Date & Convert.ToDateTime(c.DATA).Date <= dataFim.Date select c.COTA).Sum();
        }

        public Sp_Analise_Venda_PeriodoResult BuscaVendasPeriodo(string filial, string dataInicioAnoAnterior, string dataFimAnoAnterior, string dataInicioAno, string dataFimAno)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from aap in db.Sp_Analise_Venda_Periodo(filial, dataInicioAnoAnterior, dataFimAnoAnterior, dataInicioAno, dataFimAno) select aap).SingleOrDefault();
        }

        public List<Sp_Branch_Report_Cota_Loja_GrupoResult> BuscaEstoqueLoja(string filial, string dataInicioSemana, string dataFimSemana, string dataInicioMes, string dataFimMes)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from brcg in db.Sp_Branch_Report_Cota_Loja_Grupo(filial, dataInicioSemana, dataFimSemana, dataInicioMes, dataFimMes) select brcg).ToList();
        }

        public ESTOQUE_PRODUTO BuscaEstoqueLoja(string codigoProduto, string cor, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from el in db.ESTOQUE_PRODUTOs where el.PRODUTO.Trim().Equals(codigoProduto.Trim()) & el.COR_PRODUTO.Trim().Equals(cor.Trim()) & el.FILIAL.Trim().Equals(filial.Trim()) select el).SingleOrDefault();
        }

        public List<Sp_Venda_Estoque_LojaResult> BuscaVendaEstoque(string dataInicioSemana, string dataFimSemana, string colecao, string categoria, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            //return (from vel in db.Sp_Venda_Estoque_Loja(dataInicioSemana, dataFimSemana, colecao, categoria, griffe) select vel).ToList();
            return db.Sp_Venda_Estoque_Loja(dataInicioSemana, dataFimSemana, colecao, categoria, griffe).ToList();
        }

        public Sp_Vendas_AtacadoResult BuscaVendasAtacadoAnterior(string dataInicioSemana, string tipoProduto, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from vac in db.Sp_Vendas_Atacado(dataInicioSemana, tipoProduto, colecao) select vac).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Atacado_GrupoResult BuscaVendasAtacadoGrupo(string dataInicio, string dataFim, string grupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bvag in db.Sp_Busca_Vendas_Atacado_Grupo(AjustaData(dataInicio), AjustaData(dataFim), grupo, categoria, colecao) select bvag).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Atacado_GriffeResult BuscaVendasAtacadoGriffeRaiox(string dataInicio, string dataFim, string griffe, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bvag in db.Sp_Busca_Vendas_Atacado_Griffe(AjustaData(dataInicio), AjustaData(dataFim), griffe, categoria, colecao) select bvag).SingleOrDefault();
        }

        public Sp_Busca_Vendas_Atacado_SubGrupoResult BuscaVendasAtacadoSubGrupo(string dataInicio, string dataFim, string subGrupo, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bvag in db.Sp_Busca_Vendas_Atacado_SubGrupo(AjustaData(dataInicio), AjustaData(dataFim), subGrupo, categoria, colecao) select bvag).SingleOrDefault();
        }

        public Sp_Busca_Vendas_AtacadoResult BuscaVendasAtacado(string dataInicio, string dataFim, string codigoProduto)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bva in db.Sp_Busca_Vendas_Atacado(AjustaData(dataInicio), AjustaData(dataFim), codigoProduto) select bva).SingleOrDefault();
        }

        public List<Sp_Vendas_AtacadoResult> BuscaVendasAtacado(string dataInicioSemana, string colecao)
        {
            List<Sp_Vendas_AtacadoResult> lista = new List<Sp_Vendas_AtacadoResult>();

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            Sp_Vendas_AtacadoResult linha = (from vac in db.Sp_Vendas_Atacado(dataInicioSemana, "C-MAX%", colecao) select vac).SingleOrDefault();

            lista.Add(linha);

            db.CommandTimeout = 500;

            linha = (from vac in db.Sp_Vendas_Atacado(dataInicioSemana, "IMPORTADO_HBF%", colecao) select vac).SingleOrDefault();

            lista.Add(linha);

            return lista;
        }

        public List<Sp_Gestao_AtacadoResult> BuscaGestaoAtacado(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from ga in db.Sp_Gestao_Atacado(colecao) select ga).ToList();
        }

        public List<Sp_Codigo_Gestao_AtacadoResult> BuscaCodigoGestaoAtacado(string colecao, string codigoCliente)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from cga in db.Sp_Codigo_Gestao_Atacado(colecao, codigoCliente) select cga).ToList();
        }

        public List<Sp_Nome_Gestao_AtacadoResult> BuscaNomeGestaoAtacado(string colecao, string nomeCliente)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from cga in db.Sp_Nome_Gestao_Atacado(colecao, nomeCliente) select cga).ToList();
        }

        public List<Sp_Nome_Representante_AtacadoResult> BuscaNomeRepresentanteAtacado(string colecao, string nomeRepresentante)
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from cga in db.Sp_Nome_Representante_Atacado(colecao.Trim(), nomeRepresentante.Trim()) select cga).ToList();
        }

        public List<Sp_Busca_Representante_AtacadoResult> BuscaRepresentanteAtacado()
        {
            db = new DCDataContext(Constante.ConnectionStringJonas);
            db.CommandTimeout = 500;

            return (from bra in db.Sp_Busca_Representante_Atacado() select bra).ToList();
        }

        public List<Sp_Vendas_Atacado_2Result> BuscaVendasAtacado_2(string dataInicioSemana, string colecao)
        {
            List<Sp_Vendas_Atacado_2Result> lista = new List<Sp_Vendas_Atacado_2Result>();

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            Sp_Vendas_Atacado_2Result linha = (from vac in db.Sp_Vendas_Atacado_2(dataInicioSemana, "C-MAX%", colecao) select vac).SingleOrDefault();

            lista.Add(linha);

            db.CommandTimeout = 500;

            linha = (from vac in db.Sp_Vendas_Atacado_2(dataInicioSemana, "IMPORTADO_HBF%", colecao) select vac).SingleOrDefault();

            lista.Add(linha);

            return lista;
        }

        public List<Sp_Vendas_Atacado_GriffeResult> BuscaVendasAtacadoGriffe(string dataInicioSemana, string tipoProduto, string colecao, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from vacg in db.Sp_Vendas_Atacado_Griffe(dataInicioSemana, tipoProduto, colecao, griffe) select vacg).ToList();
        }

        public List<Sp_Vendas_Atacado_Griffe_2Result> BuscaVendasAtacadoGriffe_2(string dataInicioSemana, string tipoProduto, string colecao, string griffe)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from vacg in db.Sp_Vendas_Atacado_Griffe_2(dataInicioSemana, tipoProduto, colecao, griffe) select vacg).ToList();
        }

        public Sp_Vendas_Atacado_GrupoResult BuscaVendaAtacadoGrupoAnterior(string dataInicioSemana, string tipoProduto, string colecao, string griffe, string grupo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from vag in db.Sp_Vendas_Atacado_Grupo(dataInicioSemana, tipoProduto, colecao, griffe, grupo) select vag).SingleOrDefault();
        }

        public List<MovimentoProduto> BuscaMovimentoProduto(string dataInicioSemana, string dataFimSemana, string produto)
        {
            dataInicioSemana = AjustaData(dataInicioSemana);
            dataFimSemana = AjustaData(dataFimSemana);

            PRODUTO objProduto = BuscaProduto(produto);

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            List<MovimentoProduto> movimentoProdutoList = new List<MovimentoProduto>();

            List<Sp_Movimento_FilialResult> movimentoFilial = (from mv in db.Sp_Movimento_Filial(dataInicioSemana, dataFimSemana, produto) select mv).ToList();

            foreach (Sp_Movimento_FilialResult item in movimentoFilial)
            {
                MovimentoProduto movimentoProduto = new MovimentoProduto();

                movimentoProduto.Filial = BuscaFilialCodigo(Convert.ToInt32(item.FILIAL)).FILIAL;
                movimentoProduto.Estoque = Convert.ToInt32(item.QTDE_ESTOQUE);
                movimentoProduto.Recebido_semana = Convert.ToInt32(item.QTDE_RECEBIDO_SEMANA);
                movimentoProduto.Venda_semana = Convert.ToInt32(item.QTDE_VENDA_SEMANA);
                movimentoProduto.Recebido_geral = Convert.ToInt32(item.QTDE_RECEBIDO_GERAL);
                movimentoProduto.Venda_geral = Convert.ToInt32(item.QTDE_VENDA_GERAL);
                movimentoProduto.Data = dataFimSemana;
                movimentoProduto.Produto = Convert.ToInt32(produto);

                if (BuscaPrecoTLProduto(produto) == null)
                    movimentoProduto.Preco = "";
                else
                    movimentoProduto.Preco = Convert.ToDecimal(BuscaPrecoTLProduto(produto).PRECO1).ToString("###,###,##0.00");

                movimentoProduto.Modelo = objProduto.DESC_PRODUTO.Substring(objProduto.DESC_PRODUTO.Trim().Length - objProduto.GRUPO_PRODUTO.Trim().Length + 1, objProduto.GRUPO_PRODUTO.Trim().Length);
                movimentoProduto.Grupo = objProduto.GRUPO_PRODUTO;
                movimentoProduto.Colecao = Convert.ToInt32(objProduto.COLECAO);

                movimentoProdutoList.Add(movimentoProduto);
            }

            return movimentoProdutoList;
        }

        public List<Sp_Movimento_ProdutoResult> BuscaMovimentoProduto(string produto, string data_fim_1, string data_fim_2, string data_fim_3, string data_fim_4, string data_fim_5)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from mp in db.Sp_Movimento_Produto(produto, data_fim_1, data_fim_2, data_fim_3, data_fim_4, data_fim_5) select mp).ToList();
        }

        public List<Sp_Movimento_ProdutosResult> BuscaMovimentoProdutos(string data_fim_1, string data_fim_2, string data_fim_3, string data_fim_4, string data_fim_5)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from mp in db.Sp_Movimento_Produtos(data_fim_1, data_fim_2, data_fim_3, data_fim_4, data_fim_5) select mp).ToList();
        }

        public List<Sp_Cliente_FidelidadeResult> BuscaClienteFidelidade(string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from cf in db.Sp_Cliente_Fidelidade(AjustaData(dataInicio), AjustaData(dataFim)) select cf).ToList();
        }

        public Sp_Busca_Inicio_Fim_VendaResult BuscaDataInicioFimVenda(string colecao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return (from bifv in db.Sp_Busca_Inicio_Fim_Venda(colecao) select bifv).SingleOrDefault();
        }

        public List<Sp_Estoque_Data_Produto_LojaResult> BuscaEstoqueDataProdutoLoja(string dataInicio, string dataFim, string colecao, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from e in db.Sp_Estoque_Data_Produto_Loja(dataInicio, dataFim, colecao, filial) select e).ToList();
        }

        public List<Sp_Busca_Datas_SemanaResult> BuscaDatasSemana()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from bds in db.Sp_Busca_Datas_Semana() select bds).ToList();
        }

        public List<PRODUTO_CUSTO> BuscaCustoProduto()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from bcp in db.PRODUTO_CUSTOs select bcp).ToList();
        }

        public void AtualizaHistoricoDeposito(DEPOSITO_HISTORICO depositoNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DEPOSITO_HISTORICO deposito = BuscaHistoricoDeposito(depositoNew.CODIGO);

            if (deposito != null)
            {
                deposito.CODIGO_FILIAL = depositoNew.CODIGO_FILIAL;
                deposito.CODIGO_SALDO = depositoNew.CODIGO_SALDO;
                deposito.DATA = depositoNew.DATA;
                deposito.VALOR_DEPOSITO = depositoNew.VALOR_DEPOSITO;
                deposito.CONFERIDO = depositoNew.CONFERIDO;
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

        public List<DEPOSITO_HISTORICO> BuscaHistoricoDepositosPorCodigoSaldo(int? codigoSaldo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from d in db.DEPOSITO_HISTORICOs where d.CODIGO_SALDO == codigoSaldo select d).ToList();
        }

        public void AtualizaCodigoSaldoHistoricoDepositos(int? codigoSaldo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            List<DEPOSITO_HISTORICO> depositos = BuscaHistoricoDepositosPorCodigoSaldo(codigoSaldo);
            if (depositos != null)
            {
                foreach (DEPOSITO_HISTORICO deposito in depositos)
                {
                    if (deposito != null)
                    {
                        deposito.CODIGO_SALDO = 0;

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
            }
        }

        public void AtualizaDepositoComprovante(DEPOSITO_COMPROVANTE depositoComprovanteNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DEPOSITO_COMPROVANTE depositoComprovante = BuscaDepositoComprovante(depositoComprovanteNew.CODIGO);

            if (depositoComprovante != null)
            {
                depositoComprovante.CODIGO_FILIAL = depositoComprovanteNew.CODIGO_FILIAL;
                depositoComprovante.DATA_INICIO = depositoComprovanteNew.DATA_INICIO;
                depositoComprovante.DATA_FIM = depositoComprovanteNew.DATA_FIM;
                depositoComprovante.VALOR = depositoComprovanteNew.VALOR;
                depositoComprovante.OBS = depositoComprovanteNew.OBS;
                depositoComprovante.CONFERIDO = depositoComprovanteNew.CONFERIDO;
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

        public void BuscaContaBanco()
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);

        }


        public void AtualizaDepositoSemana(DEPOSITO_SEMANA depositoSemanaNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DEPOSITO_SEMANA depositoSemana = BuscaDepositoSemana(depositoSemanaNew.CODIGO_DEPOSITO);

            if (depositoSemana != null)
            {
                depositoSemana.CODIGO_FILIAL = depositoSemanaNew.CODIGO_FILIAL;
                depositoSemana.ASSINATURA = depositoSemanaNew.ASSINATURA;
                depositoSemana.OBS = depositoSemanaNew.OBS;
                depositoSemana.CODIGO_BAIXA = depositoSemanaNew.CODIGO_BAIXA;
                depositoSemana.DIFERENCA = depositoSemanaNew.DIFERENCA;
                depositoSemana.OBS = depositoSemanaNew.OBS;
                depositoSemana.SALDO_FINAL = depositoSemanaNew.SALDO_FINAL;
                depositoSemana.SALDO_INICIAL = depositoSemanaNew.SALDO_INICIAL;
                depositoSemana.VALOR_A_DEPOSITAR = depositoSemanaNew.VALOR_A_DEPOSITAR;
                depositoSemana.VALOR_DEPOSITADO = depositoSemanaNew.VALOR_DEPOSITADO;
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

        public void AtualizaCustoProduto(PRODUTO produtoNew)
        {
            db = new DCDataContext(Constante.TesteConnectionString);

            PRODUTO produto = BuscaProduto(produtoNew.PRODUTO1);

            if (produto != null)
                produto.CUSTO_REPOSICAO1 = produtoNew.CUSTO_REPOSICAO1;

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaProformaProduto(int codigoProformaProduto, int qtTotal, int qtXp, int qtPp, int qtPq, int qtMd, int qtGd, int qtGg, string fob, string codigoFornecedor, int codigoArmario, int codigoPackGrade, int codigoPackGroup, string tipoProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = BuscaProformaProduto(Convert.ToInt32(codigoProformaProduto));

            if (proformaProduto != null)
            {
                proformaProduto.QTDE_TOTAL = qtTotal;
                proformaProduto.QTDE_XP = qtXp;
                proformaProduto.QTDE_PP = qtPp;
                proformaProduto.QTDE_PQ = qtPq;
                proformaProduto.QTDE_MD = qtMd;
                proformaProduto.QTDE_GD = qtGd;
                proformaProduto.QTDE_GG = qtGg;
                proformaProduto.FOB = Convert.ToDecimal(fob);
                proformaProduto.DESCRICAO_FORNECEDOR = codigoFornecedor;
                proformaProduto.CODIGO_ARMARIO = codigoArmario;
                proformaProduto.CODIGO_PACK_GRADE = codigoPackGrade;
                proformaProduto.CODIGO_PACK_GROUP = codigoPackGroup;
                proformaProduto.TIPO_PRODUTO = tipoProduto;
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

        public void AtualizaProformaProduto(IMPORTACAO_PROFORMA_PRODUTO proformaProdutoOld, int proformaProdutoNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_PROFORMA_PRODUTO proformaProduto = BuscaProformaProduto(proformaProdutoOld.CODIGO_PROFORMA_PRODUTO);

            if (proformaProduto != null)
            {
                proformaProduto.CODIGO_PROFORMA_DEFINIDO = proformaProdutoNew;
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

        public void AtualizaNelProduto(string codigoNelProduto,
                                       string qtTotal,
                                       string qtXp,
                                       string qtPp,
                                       string qtPq,
                                       string qtMd,
                                       string qtGd,
                                       string qtGg,
                                       string fob)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_NEL_PRODUTO nelProduto = BuscaNelProduto(Convert.ToInt32(codigoNelProduto));

            if (nelProduto != null)
            {
                nelProduto.QTDE_TOTAL = Convert.ToInt32(qtTotal);
                nelProduto.QTDE_XP = Convert.ToInt32(qtXp);
                nelProduto.QTDE_PP = Convert.ToInt32(qtPp);
                nelProduto.QTDE_PQ = Convert.ToInt32(qtPq);
                nelProduto.QTDE_MD = Convert.ToInt32(qtMd);
                nelProduto.QTDE_GD = Convert.ToInt32(qtGd);
                nelProduto.QTDE_GG = Convert.ToInt32(qtGg);
                nelProduto.FOB = Convert.ToDecimal(fob);
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

        public void AtualizaDefeitoRetorno(NOTA_DEFEITO_RETORNO notaDefeitoRetornoNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_DEFEITO_RETORNO notaDefeitoRetorno = BuscaNotaDefeitoRetorno(notaDefeitoRetornoNew.CODIGO_NOTA_DEFEITO_RETORNO);

            if (notaDefeitoRetorno != null)
            {
                notaDefeitoRetorno.CODIGO_STATUS_RETORNO = 2;
                notaDefeitoRetorno.FLAG_BAIXADO = true;
                notaDefeitoRetorno.DATA_BAIXA = DateTime.Now;
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

        public void AtualizaNelOriginalProduto(string codigoNelProduto,
                                               string qtTotal,
                                               string qtXp,
                                               string qtPp,
                                               string qtPq,
                                               string qtMd,
                                               string qtGd,
                                               string qtGg)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_NEL_PRODUTO nelProduto = BuscaNelProduto(Convert.ToInt32(codigoNelProduto));

            if (nelProduto != null)
            {
                nelProduto.QTDE_TOTAL_ORIGINAL = Convert.ToInt32(qtTotal);
                nelProduto.QTDE_XP_ORIGINAL = Convert.ToInt32(qtXp);
                nelProduto.QTDE_PP_ORIGINAL = Convert.ToInt32(qtPp);
                nelProduto.QTDE_PQ_ORIGINAL = Convert.ToInt32(qtPq);
                nelProduto.QTDE_MD_ORIGINAL = Convert.ToInt32(qtMd);
                nelProduto.QTDE_GD_ORIGINAL = Convert.ToInt32(qtGd);
                nelProduto.QTDE_GG_ORIGINAL = Convert.ToInt32(qtGg);
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

        public void AtualizaEstoqueSobra(string codigo, string qtde, decimal valor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            ESTOQUE_SOBRA estoqueSobra = BuscaEstoqueSobra(Convert.ToInt32(codigo));

            if (estoqueSobra != null)
            {
                if (estoqueSobra.QTDE == null)
                    estoqueSobra.QTDE = 0;
                estoqueSobra.QTDE = estoqueSobra.QTDE + Convert.ToInt32(qtde);
                if (estoqueSobra.VALOR == null)
                    estoqueSobra.VALOR = 0;
                estoqueSobra.VALOR = estoqueSobra.VALOR + valor;
                estoqueSobra.QTDE_VIRADO = 0;
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

        public void IncluirMovimentoProduto(PRODUTO_MOVIMENTO_PRODUTO mp)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.PRODUTO_MOVIMENTO_PRODUTOs.InsertOnSubmit(mp);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GravarLogin(USUARIO_LOGIN ul)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.USUARIO_LOGINs.InsertOnSubmit(ul);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirMovimentoEstoque(ESTOQUE_DATA ed)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.ESTOQUE_DATAs.InsertOnSubmit(ed);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirMovimentoEstoqueLoja(ESTOQUE_DATA_LOJA edl)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.ESTOQUE_DATA_LOJAs.InsertOnSubmit(edl);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirDepositoBanco(DEPOSITO_SEMANA deposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.DEPOSITO_SEMANAs.InsertOnSubmit(deposito);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirDepositoBanco(DEPOSITO_SEMANA deposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.DEPOSITO_SEMANAs.Attach(deposito);
                db.DEPOSITO_SEMANAs.DeleteOnSubmit(deposito);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DEPOSITO_SALDO BuscaDepositoSaldo(int? CODIGO_SALDO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.DEPOSITO_SALDOs where (i.CODIGO_SALDO == CODIGO_SALDO) select i).Take(1).SingleOrDefault();
        }

        public void IncluirDepositoSaldo(DEPOSITO_SALDO saldo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.DEPOSITO_SALDOs.InsertOnSubmit(saldo);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirDepositoSaldo(int? CODIGO_SALDO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                DEPOSITO_SALDO saldo = new DEPOSITO_SALDO();
                saldo = BuscaDepositoSaldo(CODIGO_SALDO);

                if (saldo != null)
                {
                    db.DEPOSITO_SALDOs.DeleteOnSubmit(saldo);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public SP_OBTER_CODIGO_SALDO_DO_DEPOSITOResult ObterCodigoSaldoDoDeposito(string codigoFilial, string dataDeposito, int codigoDeposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from d in db.SP_OBTER_CODIGO_SALDO_DO_DEPOSITO(codigoFilial, dataDeposito, codigoDeposito) select d).SingleOrDefault();
        }

        public void IncluirDepositoPendenteLancamento(DEPOSITO_PENDENTE_LANCAMENTO deposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.DEPOSITO_PENDENTE_LANCAMENTOs.InsertOnSubmit(deposito);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluirDepositoPendenteLancamento(int CODIGO_DEPOSITO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                DEPOSITO_PENDENTE_LANCAMENTO imagem = new DEPOSITO_PENDENTE_LANCAMENTO();
                imagem = BuscaDepositoPendenteLancamento(CODIGO_DEPOSITO);

                if (imagem != null)
                {
                    db.DEPOSITO_PENDENTE_LANCAMENTOs.DeleteOnSubmit(imagem);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DEPOSITO_PENDENTE_LANCAMENTO BuscaDepositoPendenteLancamento(int CODIGO_DEPOSITO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.DEPOSITO_PENDENTE_LANCAMENTOs where (i.CODIGO_DEPOSITO == CODIGO_DEPOSITO) select i).Take(1).SingleOrDefault();
        }

        public void IncluirDepositoLOG(DEPOSITO_SEMANA_LOG depositoLOG)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.DEPOSITO_SEMANA_LOGs.InsertOnSubmit(depositoLOG);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirProformaProduto(IMPORTACAO_PROFORMA_PRODUTO proformaProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            db.CommandTimeout = 0;

            try
            {
                db.IMPORTACAO_PROFORMA_PRODUTOs.InsertOnSubmit(proformaProduto);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncluirNelProduto(IMPORTACAO_NEL_PRODUTO nelProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            db.CommandTimeout = 0;

            try
            {
                db.IMPORTACAO_NEL_PRODUTOs.InsertOnSubmit(nelProduto);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeletaMovimentoProduto(string semana)
        {
            string dataAtual = AjustaData(BuscaDataFim(semana));

            if (Convert.ToInt32(semana.Substring(4, 2)) > 4)
                semana = (Convert.ToInt32(semana) - 4).ToString();
            else
            {
                if (semana.Substring(4, 2).Equals("04"))
                    semana = (Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52";
                if (semana.Substring(4, 2).Equals("03"))
                    semana = (Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51";
                if (semana.Substring(4, 2).Equals("02"))
                    semana = (Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50";
                if (semana.Substring(4, 2).Equals("01"))
                    semana = (Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "49";
            }

            string dataPassada = AjustaData(BuscaDataInicio(semana));

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 500;

            db.Sp_Deleta_Movimento_Produto(dataAtual, dataPassada);
        }

        public void DeletaMovimentoEstoque(string semana, string categoria, string colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            db.Sp_Deleta_Movimento_Estoque(semana, categoria, colecao);
        }

        public void GravarDataCorte(DATAATACADO dataAtacado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.DATAATACADOs.InsertOnSubmit(dataAtacado);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string AjustaData(string data)
        {
            if (data.Length < 9)
            {
                return AjustaDataAnoBissexto(data);
            }

            string mes = "";
            string dia = "";
            string dataAjustada = "";

            if (data.Length > 12)
            {
                if (data.Substring(data.Length - 16, 1).Equals("/") || data.Substring(data.Length - 16, 1).Equals("-"))
                    mes = "0" + data.Substring(data.Length - 15, 1);
            }

            if (data.Substring(1, 1).Equals("/") || data.Substring(1, 1).Equals("-"))
                dia = "0" + data.Substring(0, 1);

            if (mes.Equals("") && dia.Equals(""))
                dataAjustada = data.Substring(6, 4) + data.Substring(3, 2) + data.Substring(0, 2);

            if (mes.Equals("") && !dia.Equals(""))
                dataAjustada = data.Substring(5, 4) + data.Substring(2, 2) + dia;

            if (!mes.Equals("") && dia.Equals(""))
                dataAjustada = data.Substring(5, 4) + mes + data.Substring(0, 2);

            if (!mes.Equals("") && !dia.Equals(""))
                dataAjustada = data.Substring(4, 4) + mes + dia;

            return dataAjustada;
        }

        private string AjustaDataAnoBissexto(string data)
        {
            string dataCorrigida = "";

            string ano = data.Substring(0, 4);
            string mes = data.Substring(4, 2);
            string dia = data.Substring(6, 2);

            if (!DateTime.IsLeapYear(int.Parse(ano)) && int.Parse(mes) == 2 && int.Parse(dia) == 29)
            {
                dataCorrigida = ano + mes + "28";
            }
            else
            {
                dataCorrigida = data;
            }

            return dataCorrigida;
        }

        public string AjustaDataInicioMes(string data)
        {
            string dataAjustada = "";

            dataAjustada = data.Substring(6, 4) + data.Substring(3, 2) + "01";

            return dataAjustada;
        }

        public string AjustaDataFimMes(string data)
        {
            string dataAjustada = "";

            dataAjustada = data.Substring(6, 4) + data.Substring(3, 2) + "30";

            return dataAjustada;
        }

        public string BuscaFimMes(string mes)
        {
            string dia = "";

            if (mes.Equals("01"))
                dia = "31";
            if (mes.Equals("02"))
                dia = "28";
            if (mes.Equals("03"))
                dia = "31";
            if (mes.Equals("04"))
                dia = "30";
            if (mes.Equals("05"))
                dia = "31";
            if (mes.Equals("06"))
                dia = "30";
            if (mes.Equals("07"))
                dia = "31";
            if (mes.Equals("08"))
                dia = "31";
            if (mes.Equals("09"))
                dia = "30";
            if (mes.Equals("10"))
                dia = "31";
            if (mes.Equals("11"))
                dia = "30";
            if (mes.Equals("12"))
                dia = "31";

            return dia;
        }

        public string BuscaDescricaoMes(int mes)
        {
            string descricao = "";

            if (mes == 1)
                descricao = "Mês-1 Cal.454";
            if (mes == 2)
                descricao = "Mês-2 Cal.454";
            if (mes == 3)
                descricao = "Mês-3 Cal.454";
            if (mes == 4)
                descricao = "Mês-4 Cal.454";
            if (mes == 5)
                descricao = "Mês-5 Cal.454";
            if (mes == 6)
                descricao = "Mês-6 Cal.454";
            if (mes == 7)
                descricao = "Mês-7 Cal.454";
            if (mes == 8)
                descricao = "Mês-8 Cal.454";
            if (mes == 9)
                descricao = "Mês-9 Cal.454";
            if (mes == 10)
                descricao = "Mês-10 Cal.454";
            if (mes == 11)
                descricao = "Mês-11 Cal.454";
            if (mes == 12)
                descricao = "Mês-12 Cal.454";

            return descricao;
        }

        public string[] RetornaDatas()
        {
            Sp_Pega_Data_BancoResult dt = BuscaDataBanco();

            string data = dt.data.Year.ToString() + dt.data.Month.ToString("00") + dt.data.Day.ToString("00");

            Sp_Busca_Semana_454Result bs = BuscaSemana454(data);

            string semana = bs.ano_semana.ToString();

            if (semana.Substring(4, 2).Equals("01"))
                semana = (dt.data.Year - 1).ToString() + "52";
            else
                semana = (bs.ano_semana - 1).ToString();

            List<string> anoSemanasPassada = new List<string>();

            if (Convert.ToInt32(semana.Substring(4, 2)) > 4)
            {
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 4).ToString());
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 3).ToString());
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
            }
            else
            {
                if (semana.Substring(4, 2).Equals("04"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 3).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                if (semana.Substring(4, 2).Equals("03"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 2).ToString());
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                if (semana.Substring(4, 2).Equals("02"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add((Convert.ToInt32(semana) - 1).ToString());
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
                if (semana.Substring(4, 2).Equals("01"))
                {
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "49");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "50");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "51");
                    anoSemanasPassada.Add((Convert.ToInt32(semana.Substring(0, 4)) - 1).ToString() + "52");
                    anoSemanasPassada.Add(Convert.ToInt32(semana).ToString());
                }
            }

            string[] datasInicioFim = new string[10];

            int i = 0;

            foreach (string item in anoSemanasPassada)
            {
                datasInicioFim[i] = AjustaData(BuscaDataInicio(item));
                i++;
                datasInicioFim[i] = AjustaData(BuscaDataFim(item));
                i++;
            }

            return datasInicioFim;
        }

        public List<LOJA_VENDA> BuscaTicketLojaVenda(string codFilial, DateTime dataVenda, string ticket)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDAs
                    where
                        lv.CODIGO_FILIAL.Trim() == codFilial.Trim() &&
                        lv.DATA_VENDA == dataVenda &&
                        lv.TICKET.Trim().Contains(ticket.Trim())
                    select lv).ToList();
        }
        public List<LOJA_VENDA_VENDEDORE> BuscaTicketLojaVendaVendedores(string codFilial, DateTime dataVenda, string ticket, string vendedor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.LOJA_VENDA_VENDEDOREs
                    where
                        lv.CODIGO_FILIAL.Trim() == codFilial.Trim() &&
                        lv.DATA_VENDA == dataVenda &&
                        lv.TICKET.Trim().Contains(ticket.Trim()) &&
                        lv.VENDEDOR.Trim() == vendedor.Trim()

                    select lv).Take(1).ToList();
        }

        public int AtualizarTicketLojaVenda(string codFilial, DateTime dataVenda, string ticket, string vendedorPara)
        {
            db = new DCDataContext(Constante.ConnectionString);

            LOJA_VENDA _lojaVenda = BuscaTicketLojaVenda(codFilial, dataVenda, ticket).SingleOrDefault();

            if (_lojaVenda != null)
            {
                _lojaVenda.VENDEDOR = vendedorPara;

                try
                {
                    db.SubmitChanges();

                    return 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return 0;
        }


        public void InserirTicketLojaVendaVendedores(LOJA_VENDA_VENDEDORE _novo)
        {
            db = new DCDataContext(Constante.ConnectionString);

            if (_novo != null)
            {

                db.LOJA_VENDA_VENDEDOREs.InsertOnSubmit(_novo);

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
        public void ExcluirTicketLojaVendaVendedores(string codFilial, DateTime dataVenda, string ticket, string vendedor)
        {
            db = new DCDataContext(Constante.ConnectionString);

            LOJA_VENDA_VENDEDORE _lojaVendaVendedores = BuscaTicketLojaVendaVendedores(codFilial, dataVenda, ticket, vendedor).SingleOrDefault();

            if (_lojaVendaVendedores != null)
            {

                db.LOJA_VENDA_VENDEDOREs.DeleteOnSubmit(_lojaVendaVendedores);
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

        public PRODUTO ObterProdutoLinx(string produto)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from lv in db.PRODUTOs
                    where
                        lv.PRODUTO1.Trim() == produto.Trim()
                    select lv).SingleOrDefault();
        }
        public void AtualizarProdutoLinx(PRODUTO p)
        {
            db = new DCDataContext(Constante.ConnectionString);

            PRODUTO _produto = ObterProdutoLinx(p.PRODUTO1);

            if (_produto != null)
            {
                _produto.CUSTO_REPOSICAO1 = p.CUSTO_REPOSICAO1;

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


        public List<PRODUTO_TRANSFERENCIA> ObterProdutoTransferencia()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIAs select g).ToList();
        }
        public List<PRODUTO_TRANSFERENCIA> ObterProdutoTransferenciaPorArquivo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIAs
                    where g.SOL_POR_ARQUIVO == true
                    select g).ToList();
        }
        public List<PRODUTO_TRANSFERENCIA> ObterProdutoTransferencia(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIAs
                    where g.CODIGO_FILIAL.Trim() == codigoFilial.Trim()
                    select g).ToList();
        }
        public PRODUTO_TRANSFERENCIA ObterProdutoTransferencia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIAs where g.CODIGO == codigo select g).SingleOrDefault();
        }
        public int InserirProdutoTransferencia(PRODUTO_TRANSFERENCIA _transf)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.PRODUTO_TRANSFERENCIAs.InsertOnSubmit(_transf);
                db.SubmitChanges();

                return _transf.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoTransferencia(PRODUTO_TRANSFERENCIA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PRODUTO_TRANSFERENCIA _transf = ObterProdutoTransferencia(_novo.CODIGO);

            if (_novo != null)
            {
                _transf.CODIGO = _novo.CODIGO;
                _transf.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _transf.VOLUME = _novo.VOLUME;
                _transf.QTDE_ITEM = _novo.QTDE_ITEM;
                _transf.DATA_ENVIO = _novo.DATA_ENVIO;
                _transf.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _transf.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _transf.OBSERVACAO = _novo.OBSERVACAO;
                _transf.PERMUTA = _novo.PERMUTA;
                _transf.MOTIVO = _novo.MOTIVO;
                _transf.CODIGO_FILIAL_TRANSF = _novo.CODIGO_FILIAL_TRANSF;
                _transf.SOL_POR_ARQUIVO = _novo.SOL_POR_ARQUIVO;

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
        public void ExcluirProdutoTransferencia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                PRODUTO_TRANSFERENCIA _transf = ObterProdutoTransferencia(codigo);
                if (_transf != null)
                {
                    db.PRODUTO_TRANSFERENCIAs.DeleteOnSubmit(_transf);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PRODUTO_TRANSFERENCIA_ITEM> ObterProdutoTransferenciaItem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs select g).ToList();
        }
        public List<PRODUTO_TRANSFERENCIA_ITEM> ObterProdutoTransferenciaItem(string codigoTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs
                    where g.PRODUTO_TRANSFERENCIA.ToString() == codigoTransferencia
                    select g).ToList();
        }
        public PRODUTO_TRANSFERENCIA_ITEM ObterProdutoTransferenciaItem(int codigoTransferencia, string produto, string cor, string tamanho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs
                    where g.PRODUTO_TRANSFERENCIA == codigoTransferencia
                    && g.PRODUTO.Trim() == produto
                    && g.COR.Trim() == cor
                    && g.TAMANHO.Trim() == tamanho
                    select g).OrderByDescending(p => p.SERIE_NF_ENTRADA).FirstOrDefault();
        }
        public List<PRODUTO_TRANSFERENCIA_ITEM> ObterProdutoTransferenciaItemPorFilial(string codigoTransferencia, string filialOrigem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs
                    where g.PRODUTO_TRANSFERENCIA.ToString() == codigoTransferencia
                    && g.FILIAL_ORIGEM == filialOrigem
                    select g).ToList();
        }
        public List<PRODUTO_TRANSFERENCIA_ITEM> ObterProdutoTransferenciaItem(string codigoTransferencia, bool produtoCmax)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (produtoCmax)
                return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs
                        where g.PRODUTO_TRANSFERENCIA.ToString() == codigoTransferencia
                        && g.PRODUTO.Substring(0, 1) == "6"
                        select g).ToList();
            else
                return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs
                        where g.PRODUTO_TRANSFERENCIA.ToString() == codigoTransferencia
                        && g.PRODUTO.Substring(0, 1) != "6"
                        select g).ToList();
        }
        public List<PRODUTO_TRANSFERENCIA_ITEM> ObterProdutoTransferenciaItemPorFilialOrigem(string codigoTransferencia, string filialOrigem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs
                    where g.PRODUTO_TRANSFERENCIA.ToString() == codigoTransferencia
                    && g.FILIAL_ORIGEM == filialOrigem
                    select g).ToList();
        }

        public PRODUTO_TRANSFERENCIA_ITEM ObterProdutoTransferenciaItem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_ITEMs where g.CODIGO == codigo select g).SingleOrDefault();
        }
        public int InserirProdutoTransferenciaItem(PRODUTO_TRANSFERENCIA_ITEM _transf)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.PRODUTO_TRANSFERENCIA_ITEMs.InsertOnSubmit(_transf);
                db.SubmitChanges();

                return _transf.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoTransferenciaItem2(PRODUTO_TRANSFERENCIA_ITEM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PRODUTO_TRANSFERENCIA_ITEM _transf = ObterProdutoTransferenciaItem(_novo.CODIGO);

            if (_novo != null)
            {
                _transf.CODIGO = _novo.CODIGO;
                _transf.PRODUTO_TRANSFERENCIA = _novo.PRODUTO_TRANSFERENCIA;
                _transf.CODIGO_BARRA = _novo.CODIGO_BARRA;
                _transf.NOME = _novo.NOME;
                _transf.COR = _novo.COR;
                _transf.COLECAO = _novo.COLECAO;
                _transf.TAMANHO = _novo.TAMANHO;
                _transf.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _transf.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirProdutoTransferenciaItem(int codigoTransferencia, string produto, string cor, string tamanho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                PRODUTO_TRANSFERENCIA_ITEM transf = ObterProdutoTransferenciaItem(codigoTransferencia, produto, cor, tamanho);
                if (transf != null)
                {
                    db.PRODUTO_TRANSFERENCIA_ITEMs.DeleteOnSubmit(transf);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProdutoTransferenciaItem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                PRODUTO_TRANSFERENCIA_ITEM _transf = ObterProdutoTransferenciaItem(codigo);
                if (_transf != null)
                {
                    db.PRODUTO_TRANSFERENCIA_ITEMs.DeleteOnSubmit(_transf);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<PRODUTO_TRANSFERENCIA_SOL> ObterProdutoTransferenciaSol(string codigoTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_SOLs
                    where g.PRODUTO_TRANSFERENCIA.ToString() == codigoTransferencia
                    select g).ToList();
        }
        public PRODUTO_TRANSFERENCIA_SOL ObterProdutoTransferenciaSol(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PRODUTO_TRANSFERENCIA_SOLs where g.CODIGO == codigo select g).SingleOrDefault();
        }
        public int InserirProdutoTransferenciaSol(PRODUTO_TRANSFERENCIA_SOL transSol)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.PRODUTO_TRANSFERENCIA_SOLs.InsertOnSubmit(transSol);
                db.SubmitChanges();

                return transSol.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarProdutoTransferenciaSol(PRODUTO_TRANSFERENCIA_SOL _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PRODUTO_TRANSFERENCIA_SOL transSol = ObterProdutoTransferenciaSol(_novo.CODIGO);

            if (_novo != null)
            {
                transSol.CODIGO = _novo.CODIGO;
                transSol.PRODUTO_TRANSFERENCIA = _novo.PRODUTO_TRANSFERENCIA;
                transSol.CODIGO_BARRA = _novo.CODIGO_BARRA;
                transSol.PRODUTO = _novo.PRODUTO;
                transSol.NOME = _novo.NOME;
                transSol.COR = _novo.COR;
                transSol.COLECAO = _novo.COLECAO;
                transSol.TAMANHO = _novo.TAMANHO;
                transSol.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                transSol.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirProdutoTransferenciaSol(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                PRODUTO_TRANSFERENCIA_SOL tranSol = ObterProdutoTransferenciaSol(codigo);
                if (tranSol != null)
                {
                    db.PRODUTO_TRANSFERENCIA_SOLs.DeleteOnSubmit(tranSol);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_PRODUTOTRANSF_QTDEResult> ObterProdutoTransferenciaQtde(int codigoTrans, string produto, string cor, string tamanho, int query)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_PRODUTOTRANSF_QTDE(codigoTrans, produto, cor, tamanho, query) select m).ToList();
        }

        public SP_OBTER_SALDO_LOJA_DEVResult ObterSaldoLojaDev(string produto, string cor, int tamanho, string filial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_OBTER_SALDO_LOJA_DEV(produto, cor, tamanho, filial) select m).SingleOrDefault();
        }
        public SP_GERAR_ROMANEIO_SAIDA_ESTOQUEResult GerarRomaneioSaidaEstoque(string filial, string filialOrigem, string nfEntrada, string serieNfEntrada, string tipoProduto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_GERAR_ROMANEIO_SAIDA_ESTOQUE(filial, filialOrigem, nfEntrada, serieNfEntrada, tipoProduto) select m).SingleOrDefault();
        }

        public SP_OBTER_SALDO_LOJA_DEV_DEFEITOResult ObterSaldoLojaDevDefeito(string produto, string cor, int tamanho, string filial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_OBTER_SALDO_LOJA_DEV_DEFEITO(produto, cor, tamanho, filial) select m).SingleOrDefault();
        }
        public SP_GERAR_ROMANEIO_SAIDA_ESTOQUE_DEFEITOResult GerarRomaneioSaidaEstoqueDefeito(string filial, string filialOrigem, string nfEntrada, string serieNfEntrada, string tipoProduto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_GERAR_ROMANEIO_SAIDA_ESTOQUE_DEFEITO(filial, filialOrigem, nfEntrada, serieNfEntrada, tipoProduto) select m).SingleOrDefault();
        }

        public SP_OBTER_SALDO_LOJA_DEV_INTERNOResult ObterSaldoLojaDevInterno(string produto, string cor, int tamanho, string filial, string codUsuario)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_OBTER_SALDO_LOJA_DEV_INTERNO(produto, cor, tamanho, filial, codUsuario) select m).SingleOrDefault();
        }
        public SP_GERAR_ROMANEIO_SAIDA_ESTOQUE_INTERNOResult GerarRomaneioSaidaEstoqueInterno(string filial, string filialOrigem, string nfEntrada, string serieNfEntrada, string codUsuario)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_GERAR_ROMANEIO_SAIDA_ESTOQUE_INTERNO(filial, filialOrigem, nfEntrada, serieNfEntrada, codUsuario) select m).SingleOrDefault();
        }

        public ESTOQUE_PROD_SAI ObterRomaneioEstoqueSaida(string romaneioProduto, string filial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            return (from g in dbLinx.ESTOQUE_PROD_SAIs
                    where g.ROMANEIO_PRODUTO == romaneioProduto
                    && g.FILIAL == filial
                    select g).SingleOrDefault();
        }
        public void ExcluirRomaneioEstoqueSaida(string romaneioProduto, string filial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            try
            {
                ESTOQUE_PROD_SAI romSaida = ObterRomaneioEstoqueSaida(romaneioProduto, filial);
                if (romSaida != null)
                {
                    dbLinx.ESTOQUE_PROD_SAIs.DeleteOnSubmit(romSaida);
                    dbLinx.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<EMPRESA> ObterEmpresa()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.EMPRESAs orderby g.NOME select g).ToList();
        }
        public EMPRESA ObterEmpresa(string nome)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.EMPRESAs where g.NOME.Trim() == nome.Trim() select g).FirstOrDefault();
        }

        public List<EMPRESA_ASSINANTE> ObterEmpresaAssinante(int empresa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.EMPRESA_ASSINANTEs
                    where g.EMPRESA == empresa
                    orderby g.ASSINANTE
                    select g).ToList();
        }

        /// <summary>
        /// Obtém o sequencial para a tabela e campo passados como parâmetro. Exemplo: VENDAS_LOTE.PEDIDO_EXTERNO
        /// </summary>
        /// <param name="tabelaCampo"></param>
        /// <returns></returns>
        public string ObterSequencialTabelaLinx(string tabelaCampo)
        {
            string sequencia = "";
            db = new DCDataContext(Constante.ConnectionString);
            db.LX_SEQUENCIAL("VENDAS_LOTE.PEDIDO_EXTERNO", 1, ref sequencia, true, "");

            return sequencia;
        }

        public List<SP_OBTER_FILIAIS_CONFIGURACAOResult> ListarFiliaisConfiguracao(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 600;
            return (from m in db.SP_OBTER_FILIAIS_CONFIGURACAO(filial) select m).ToList();
        }

        public FILIAIS_CONFIGURACAO BuscarFiliaisConfiguracao(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from f in db.FILIAIS_CONFIGURACAOs where f.CODIGO_FILIAL.Equals(filial) select f).SingleOrDefault();
        }

        public void IncluirFiliaisConfiguracao(FILIAIS_CONFIGURACAO filiaisConfiguracao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            try
            {
                db.FILIAIS_CONFIGURACAOs.InsertOnSubmit(filiaisConfiguracao);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizarFiliaisConfiguracao(FILIAIS_CONFIGURACAO filiaisConfiguracaoSelecionado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FILIAIS_CONFIGURACAO filiaisConfiguracao = new FILIAIS_CONFIGURACAO();
            filiaisConfiguracao = BuscarFiliaisConfiguracao(filiaisConfiguracaoSelecionado.CODIGO_FILIAL);

            if (filiaisConfiguracao != null)
            {
                filiaisConfiguracao.CODIGO_FILIAL = filiaisConfiguracaoSelecionado.CODIGO_FILIAL;
                filiaisConfiguracao.PASTA_IMAGENS_DEPOSITO = filiaisConfiguracaoSelecionado.PASTA_IMAGENS_DEPOSITO;
                filiaisConfiguracao.AGORA_REDE = filiaisConfiguracaoSelecionado.AGORA_REDE;
                filiaisConfiguracao.AGORA_SUPERVISORES = filiaisConfiguracaoSelecionado.AGORA_SUPERVISORES;
                filiaisConfiguracao.MOD_ADMINISTRACAO = filiaisConfiguracaoSelecionado.MOD_ADMINISTRACAO;
                filiaisConfiguracao.MOD_GESTAO = filiaisConfiguracaoSelecionado.MOD_GESTAO;
                filiaisConfiguracao.MOD_ATACADO = filiaisConfiguracaoSelecionado.MOD_ATACADO;
                filiaisConfiguracao.MOD_FINANCEIRO = filiaisConfiguracaoSelecionado.MOD_FINANCEIRO;
                filiaisConfiguracao.MOD_CONTABILIDADE = filiaisConfiguracaoSelecionado.MOD_CONTABILIDADE;
                filiaisConfiguracao.MOD_FISCAL = filiaisConfiguracaoSelecionado.MOD_FISCAL;
                filiaisConfiguracao.MOD_ADM_NOTA_FISCAL = filiaisConfiguracaoSelecionado.MOD_ADM_NOTA_FISCAL;
                filiaisConfiguracao.MOD_ADMINISTRACAO_LOJA = filiaisConfiguracaoSelecionado.MOD_ADMINISTRACAO_LOJA;
                filiaisConfiguracao.MOD_GERENCIAMENTO_LOJA = filiaisConfiguracaoSelecionado.MOD_GERENCIAMENTO_LOJA;
                filiaisConfiguracao.MOD_ACOMPANHAMENTO_MENSAL = filiaisConfiguracaoSelecionado.MOD_ACOMPANHAMENTO_MENSAL;
                filiaisConfiguracao.MOD_CONTROLE_ESTOQUE = filiaisConfiguracaoSelecionado.MOD_CONTROLE_ESTOQUE;
                filiaisConfiguracao.MOD_CONTAGEM = filiaisConfiguracaoSelecionado.MOD_CONTAGEM;
                filiaisConfiguracao.MOD_RH = filiaisConfiguracaoSelecionado.MOD_RH;
                filiaisConfiguracao.MOD_REPRESENTANTE = filiaisConfiguracaoSelecionado.MOD_REPRESENTANTE;
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

        public List<RH_CARGO> BuscarCargos(Char Agrupado)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.RH_CARGOs where f.AGRUPADO.Equals(Agrupado) orderby f.DESC_CARGO select f).ToList();
        }

        public void IncluirLokLookbook(LOK_LOOKBOOK lokLookbook)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOK_LOOKBOOKs.InsertOnSubmit(lokLookbook);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<INVFOTO> ObterInvFoto()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.INVFOTOs select f).ToList();
        }

        public INVFOTO ObterInvFoto(string produto, string cor, string foto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.INVFOTOs
                    where f.PRODUTO == produto
                    && f.COR == cor
                    && f.FOTO == foto
                    select f).FirstOrDefault();
        }
        public void AtualizarInvFoto(INVFOTO invFoto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            INVFOTO l = ObterInvFoto(invFoto.PRODUTO, invFoto.COR, invFoto.FOTO);

            if (l != null)
            {
                l.OK = invFoto.OK;

                try
                {
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<LOK_LOK> ObterLok()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.LOK_LOKs where f.OK == false select f).ToList();
        }
        public LOK_LOK ObterLok(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from f in dbIntra.LOK_LOKs where f.CODIGO == codigo select f).SingleOrDefault();
        }
        public void IncluirLokLok(LOK_LOK lok)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOK_LOKs.InsertOnSubmit(lok);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarLokLok(LOK_LOK lok)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            LOK_LOK l = ObterLok(lok.CODIGO);

            if (l != null)
            {
                l.OK = lok.OK;

                try
                {
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
