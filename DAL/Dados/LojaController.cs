using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace DAL
{
    public class LojaController
    {
        DCDataContext db;
        INTRADataContext dbIntra;
        LINXDataContext dbLinx;

        // Find your Account Sid and Token at twilio.com/console
        const string accountSid = "AC85e68951e2abe258b2488bd133767936";
        const string authToken = "3ccc2ffd4c6b06e66a4aed098ae7ae78";
        const string fromNumber = "+18084000759";

        public List<USUARIOLOJA> BuscaLojas(int CODIGO_USUARIO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            if (CODIGO_USUARIO == 0)
                return (from ul in db.USUARIOLOJAs orderby ul.CODIGO_USUARIO orderby ul.CODIGO_LOJA, ul.CODIGO_USUARIO select ul).ToList();
            else
                return (from ul in db.USUARIOLOJAs where ul.CODIGO_USUARIO == CODIGO_USUARIO orderby ul.CODIGO_LOJA select ul).ToList();
        }
        public List<FILIAI> BuscaLojas()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from f in db.FILIAIs where f.TIPO_FILIAL.Equals("LOJA") || f.TIPO_FILIAL.Equals("FRANQUIA") select f).ToList(); //teste fech/to fundo fixo Mogi
        }
        public FILIAI BuscaPorCodigoLoja(string CODIGO_LOJA)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from f in db.FILIAIs where f.COD_FILIAL.Equals(CODIGO_LOJA) select f).SingleOrDefault();
        }
        public DEPOSITO_BAIXA BuscaDepositoBaixa(int codigoBaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from d in db.DEPOSITO_BAIXAs where d.CODIGO_BAIXA == codigoBaixa select d).SingleOrDefault();
        }
        public USUARIOLOJA BuscaPorCodigoUsuarioLoja(int CODIGO_USUARIOLOJA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from ul in db.USUARIOLOJAs where ul.CODIGO_USUARIOLOJA == CODIGO_USUARIOLOJA select ul).SingleOrDefault();
        }
        public USUARIOLOJA BuscaStaffPorCodigoLoja(string CODIGO_LOJA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from ul in db.USUARIOLOJAs where ul.CODIGO_LOJA.Equals(CODIGO_LOJA) select ul).SingleOrDefault();
        }
        public void Insere(USUARIOLOJA usuarioLoja)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.USUARIOLOJAs.InsertOnSubmit(usuarioLoja);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Atualiza(USUARIOLOJA usuarioLojaNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            USUARIOLOJA usuarioLojaOld = BuscaPorCodigoUsuarioLoja(usuarioLojaNew.CODIGO_USUARIOLOJA);

            if (usuarioLojaOld != null)
            {
                usuarioLojaOld.CODIGO_USUARIOLOJA = usuarioLojaNew.CODIGO_USUARIOLOJA;
                usuarioLojaOld.CODIGO_LOJA = usuarioLojaNew.CODIGO_LOJA;
                usuarioLojaOld.CODIGO_USUARIO = usuarioLojaNew.CODIGO_USUARIO;
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
        public int Exclui(int codigoUsuarioLoja)
        {
            int ok = 0;

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                USUARIOLOJA usuarioLoja = BuscaPorCodigoUsuarioLoja(codigoUsuarioLoja);

                if (usuarioLoja != null)
                {
                    //List<RH_CANDIDATO> candidato = new CandidatoController().BuscaCandidatosLoja(usuarioLoja.CODIGO_LOJA);

                    db.USUARIOLOJAs.DeleteOnSubmit(usuarioLoja);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ok;
        }


        public List<SP_OBTER_VENDA_PRODUTO_DIARIOResult> ObterVendaProdutosDiario(DateTime dataIni, DateTime dataFim, string colecaoLinx, string colecaoIntra, int desenvProdutoOrigem, string griffe, string grupoProduto, string fornecedor, string produto, string descProduto, string produtoAcabado, string signedNome, string tipoProduto, string subgrupoProduto, string tecido, string corFornecedor)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_VENDA_PRODUTO_DIARIO(dataIni, dataFim, colecaoLinx, colecaoIntra, desenvProdutoOrigem, griffe, grupoProduto, fornecedor, produto, descProduto, produtoAcabado, signedNome, tipoProduto, subgrupoProduto, tecido, corFornecedor) select m).ToList();
        }

        public List<SP_OBTER_GINCANA2017Result> ObterGincana2017()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_GINCANA2017() select m).ToList();
        }
        public List<SP_OBTER_GINCANA_INV2018Result> ObterGincanaINV2018()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_GINCANA_INV2018() select m).ToList();
        }
        public List<SP_OBTER_GINCANA_NAT2018Result> ObterGincanaNAT2018()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_GINCANA_NAT2018() select m).ToList();
        }
        public List<SP_OBTER_GINCANA_NAT2019Result> ObterGincanaNAT2019()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_GINCANA_NAT2019() select m).ToList();
        }
        public List<SP_OBTER_GINCANA_NAT2020Result> ObterGincanaNAT2020()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_GINCANA_NAT2020() select m).ToList();
        }

        public List<SP_OBTER_GINCANA_NAT2021Result> ObterGincanaNAT2021()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_GINCANA_NAT2021() select m).ToList();
        }


        public List<SP_OBTER_ESTOQUE_LOJAResult> ObterEstoqueLoja(string filial, string produto, string grupoProduto, string griffe, string colecao, decimal precoIni, decimal precoFim, int qtdeEstoqueIni, int qtdeEstoqueFim)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_ESTOQUE_LOJA(filial, produto, grupoProduto, griffe, colecao, precoIni, precoFim, qtdeEstoqueIni, qtdeEstoqueFim) select m).ToList();
        }

        public List<SP_OBTER_PRODUTO_RECEBIDOLOJAResult> ObterProdutoRecebidoLoja(string filial, string griffe, string grupoProduto, string produto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_PRODUTO_RECEBIDOLOJA(filial, griffe, grupoProduto, produto) select m).ToList();
        }


        public List<SP_OBTER_PAINEL_RESULTADO_PRODUTOResult> ObterPainelResultadoProduto(DateTime dataIni, DateTime dataFim, string colecao, string colecaoOrigem, string produto, string griffe, string grupoProduto, string codCategoria)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_PAINEL_RESULTADO_PRODUTO(dataIni, dataFim, colecao, colecaoOrigem, produto, griffe, grupoProduto, codCategoria) select m).ToList();
        }

        public List<SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOLResult> ObterPainelResultadoProdutoConsolidado(DateTime dataIni, DateTime dataFim, string colecao, string colecaoOrigem, string griffe, string grupoProduto, string codCategoria)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_PAINEL_RESULTADO_PRODUTO_CONSOL(dataIni, dataFim, colecao, colecaoOrigem, griffe, grupoProduto, codCategoria) select m).ToList();
        }

        public List<SP_OBTER_PAINEL_RESULTADO_PRODUTO_ONLINEResult> ObterPainelResultadoProdutoOnline(DateTime dataIni, DateTime dataFim, string colecao, string colecaoOrigem, string produto, string griffe, string grupoProduto, string codCategoria)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from m in dbLinx.SP_OBTER_PAINEL_RESULTADO_PRODUTO_ONLINE(dataIni, dataFim, colecao, colecaoOrigem, produto, griffe, grupoProduto, codCategoria) select m).ToList();
        }

        public List<SP_OBTER_PRODUTO_TRANSFRELResult> ObterTransLojaRel(string codigoFilial, string produto, DateTime? dataIni, DateTime? dataFim, char? motivo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from m in dbIntra.SP_OBTER_PRODUTO_TRANSFREL(codigoFilial, produto, dataIni, dataFim, motivo) select m).ToList();
        }



        /*****************************************************************************************************/
        // AÇÃO  - LOJINHA
        //SP_VENDA_LOJINHA_SEQ
        public SP_VENDA_LOJINHA_SEQResult ObterSequenciaVendaLojinha()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_VENDA_LOJINHA_SEQ() select i).SingleOrDefault();
        }

        public List<VENDA_LOJINHA> ObterVendaLojinha(string seq)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.VENDA_LOJINHAs
                    where m.SEQ == Convert.ToInt32(seq)
                    orderby m.CODIGO
                    select m).ToList();
        }
        public VENDA_LOJINHA ObterVendaLojinha(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.VENDA_LOJINHAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public bool ClienteJaComprou(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            var clienteJaComprou = (from m in dbIntra.VENDA_LOJINHAs
                                    where m.CPF == cpf && m.DATA_FECHAMENTO != null
                                    select m).FirstOrDefault();

            if (clienteJaComprou != null)
                return true;

            return false;
        }
        public List<VENDA_LOJINHA> ObterVendaLojinhaPorCPF(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.VENDA_LOJINHAs
                    where m.CPF == cpf
                    select m).ToList();
        }
        public void InserirVendaLojinha(VENDA_LOJINHA vendaLojinha)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.VENDA_LOJINHAs.InsertOnSubmit(vendaLojinha);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarVendaLojinhaPorCPF(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            var lstVendaLojinha = ObterVendaLojinhaPorCPF(cpf);

            foreach (var vl in lstVendaLojinha)
            {
                if (vl != null)
                {
                    AtualizarVendaLojinha(vl);
                }
            }

        }
        public void AtualizarVendaLojinha(VENDA_LOJINHA vendaLojinha)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            VENDA_LOJINHA _vendaLojinha = ObterVendaLojinha(vendaLojinha.CODIGO);

            if (_vendaLojinha != null)
            {
                _vendaLojinha.DATA_FECHAMENTO = DateTime.Now;
            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirVendaLojinhaPorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            VENDA_LOJINHA vendaLojinha = ObterVendaLojinha(codigo);

            if (vendaLojinha != null)
            {
                try
                {
                    dbIntra.VENDA_LOJINHAs.DeleteOnSubmit(vendaLojinha);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirVendaLojinhaPorSequencia(string seq)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            List<VENDA_LOJINHA> lstVendaLojinha = ObterVendaLojinha(seq);

            foreach (var p in lstVendaLojinha)
            {
                if (p != null)
                {
                    try
                    {
                        dbIntra.VENDA_LOJINHAs.DeleteOnSubmit(p);
                        dbIntra.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public VENDA_LOJINHA_FUNSAL ObterSalarioFuncionario(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.VENDA_LOJINHA_FUNSALs
                    where m.CPF == cpf
                    select m).SingleOrDefault();
        }

        public SP_OBTER_PRODUTO_VENDA_LOJINHAResult ObterProdutoVendaLojinha(string produto, string produtoBarra)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_PRODUTO_VENDA_LOJINHA(produto, produtoBarra) select i).SingleOrDefault();
        }
        /*****************************************************************************************************/
        /*****************************************************************************************************/

        public List<SP_OBTER_VALOR_LINX_INTRANETResult> ObterValorLinxIntranet(DateTime data)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_VALOR_LINX_INTRANET(data) select i).ToList();
        }

        public ACOMPANHAMENTO_ALUGUEL_BOLETO ObterBoletoAluguel(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from ul in db.ACOMPANHAMENTO_ALUGUEL_BOLETOs
                    where ul.CODIGO == codigo
                    select ul).SingleOrDefault();
        }
        public ACOMPANHAMENTO_ALUGUEL_BOLETO ObterBoletoAluguel(string codigoFilial, int lancamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from ul in db.ACOMPANHAMENTO_ALUGUEL_BOLETOs
                    where ul.CODIGO_FILIAL == codigoFilial
                    && ul.LANCAMENTO == lancamento
                    select ul).SingleOrDefault();
        }
        public int InserirBoletoAluguel(ACOMPANHAMENTO_ALUGUEL_BOLETO boleto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.ACOMPANHAMENTO_ALUGUEL_BOLETOs.InsertOnSubmit(boleto);
                db.SubmitChanges();

                return boleto.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AtualizarBoletoAluguel(ACOMPANHAMENTO_ALUGUEL_BOLETO boleto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            ACOMPANHAMENTO_ALUGUEL_BOLETO _boleto = ObterBoletoAluguel(boleto.CODIGO);

            if (_boleto != null)
            {
                _boleto.CODIGO_FILIAL = boleto.CODIGO_FILIAL;
                _boleto.DATA_VENCIMENTO = boleto.DATA_VENCIMENTO;
                _boleto.VALOR = boleto.VALOR;
                _boleto.DIRETORIO_BOLETO = boleto.DIRETORIO_BOLETO;
                _boleto.OBSERVACAO = boleto.OBSERVACAO;
                _boleto.LANCAMENTO = boleto.LANCAMENTO;
            }

            try
            {
                db.SubmitChanges();

                return _boleto.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirBoletoAluguel(int codigoBoleto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                ACOMPANHAMENTO_ALUGUEL_BOLETO boleto = ObterBoletoAluguel(codigoBoleto);

                if (boleto != null)
                {
                    db.ACOMPANHAMENTO_ALUGUEL_BOLETOs.DeleteOnSubmit(boleto);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ACOMPANHAMENTO_ALUGUEL ObterBoletoAluguelDespesa(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from ai in db.ACOMPANHAMENTO_ALUGUELs where ai.CODIGO_ACOMPANHAMENTO_ALUGUEL == codigo select ai).SingleOrDefault();
        }
        public void InserirBoletoAluguelDespesa(ACOMPANHAMENTO_ALUGUEL aluguel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.ACOMPANHAMENTO_ALUGUELs.InsertOnSubmit(aluguel);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarBoletoAluguelDespesa(ACOMPANHAMENTO_ALUGUEL aluguel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            ACOMPANHAMENTO_ALUGUEL _aluguel = ObterBoletoAluguelDespesa(aluguel.CODIGO_ACOMPANHAMENTO_ALUGUEL);

            if (_aluguel != null)
            {
                _aluguel.CODIGO_ALUGUEL_DESPESA = aluguel.CODIGO_ALUGUEL_DESPESA;
                _aluguel.VALOR = aluguel.VALOR;
                _aluguel.CODIGO_FILIAL = aluguel.CODIGO_FILIAL;
                _aluguel.CODIGO_MESANO = aluguel.CODIGO_MESANO;
                _aluguel.DATA_VENCIMENTO = aluguel.DATA_VENCIMENTO;
                _aluguel.ACOMPANHAMENTO_ALUGUEL_BOLETO = aluguel.ACOMPANHAMENTO_ALUGUEL_BOLETO;
                _aluguel.LANCAMENTO = aluguel.LANCAMENTO;
                _aluguel.ITEM = aluguel.ITEM;
                _aluguel.DESCONTO = aluguel.DESCONTO;
                _aluguel.COMPETENCIA = aluguel.COMPETENCIA;
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
        public void ExcluirBoletoAluguelDespesa(int codigoAluguel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                ACOMPANHAMENTO_ALUGUEL aluguel = ObterBoletoAluguelDespesa(codigoAluguel);

                if (aluguel != null)
                {
                    db.ACOMPANHAMENTO_ALUGUELs.DeleteOnSubmit(aluguel);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_BOLETO_LANCAMENTOResult> ObterBoletoLancamento(int lancamento)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_BOLETO_LANCAMENTO(lancamento) select i).ToList();
        }
        public List<SP_OBTER_BOLETO_LANCAMENTO_DESPESASResult> ObterBoletoLancamentoDespesas(int lancamento)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_BOLETO_LANCAMENTO_DESPESAS(lancamento) select i).ToList();
        }


        public List<SP_OBTER_ANIVERSARIANTESResult> ObterAniversariantes(string codigoFilialCompra, string cpf, int? mes, string nome)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_ANIVERSARIANTES(codigoFilialCompra, cpf, mes, nome) select i).ToList();
        }
        public ANIVERSARIANTE ObterAniversariante(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from ai in dbIntra.ANIVERSARIANTEs where ai.CODIGO == codigo select ai).SingleOrDefault();
        }
        public void AtualizarAniversariante(ANIVERSARIANTE aniver)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            ANIVERSARIANTE _aniver = ObterAniversariante(aniver.CODIGO);

            if (_aniver != null)
            {
                _aniver.DATA_BAIXA = aniver.DATA_BAIXA;
                _aniver.COD_FILIAL_BAIXA = aniver.COD_FILIAL_BAIXA;

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

        public List<SP_OBTER_CAMP_GUARULHOSResult> ObterCampGuarulhos(string codigoFilialCompra, string cpf, string nome)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_CAMP_GUARULHOS(codigoFilialCompra, cpf, nome) select i).ToList();
        }
        public CAMP_GUARULHO ObterCampGuarulhos(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (from ai in dbIntra.CAMP_GUARULHOs where ai.CODIGO == codigo select ai).SingleOrDefault();
        }
        public void AtualizarCampGuarulhos(CAMP_GUARULHO gru)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            CAMP_GUARULHO _gru = ObterCampGuarulhos(gru.CODIGO);

            if (_gru != null)
            {
                _gru.DATA_BAIXA = gru.DATA_BAIXA;
                _gru.COD_FILIAL_BAIXA = gru.COD_FILIAL_BAIXA;

            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SP_OBTER_TICKET_DIARIO_CLIENTEResult> ObterTicketDiarioPorCliente(DateTime dataIni, DateTime dataFim, string codigoFilial, string cpf, int qtdeTickets)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_TICKET_DIARIO_CLIENTE(dataIni, dataFim, codigoFilial, cpf, qtdeTickets) select i).ToList();
        }
        public List<SP_OBTER_TICKET_MENSAL_CLIENTEResult> ObterTicketMensalPorCliente(int ano, int mes, string codigoFilial, string cpf, int qtdeTickets)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_TICKET_MENSAL_CLIENTE(ano, mes, codigoFilial, cpf, qtdeTickets) select i).ToList();
        }
        public List<SP_OBTER_TICKET_POR_CLIENTEResult> ObterTicketPorCliente(int ano, int mes, string cpf)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_TICKET_POR_CLIENTE(ano, mes, cpf) select i).ToList();
        }

        public List<SP_OBTER_VENDA_PRODUTO_TICKETResult> ObterProdutoPorTicket(string ticket, string codigoFilial, DateTime dataVEnda)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_VENDA_PRODUTO_TICKET(ticket, codigoFilial, dataVEnda) select i).ToList();
        }

        public List<SP_OBTER_WAPP_SALDOHCResult> ObterSaldoWAPP(int ano, int mes, string codigoFilial, string cpf, decimal saldoHC)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_WAPP_SALDOHC(ano, mes, codigoFilial, cpf, saldoHC) select i).ToList();
        }


        public List<SP_OBTER_HIST_VENDA_CANCELADAResult> ObterVendaCanceladaHist(string codigoFilial, string ticket, string codigoCliente)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_HIST_VENDA_CANCELADA(codigoFilial, ticket, codigoCliente) select i).ToList();
        }

        public CLIENTE_VENDA_ACOMP ObterClienteVendaAcompanhado(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from ai in dbIntra.CLIENTE_VENDA_ACOMPs where ai.CPF == cpf select ai).SingleOrDefault();
        }
        public void InserirClienteVendaAcompanhado(CLIENTE_VENDA_ACOMP cliVendaA)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.CLIENTE_VENDA_ACOMPs.InsertOnSubmit(cliVendaA);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirClienteVendaAcompanhado(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                CLIENTE_VENDA_ACOMP cli = ObterClienteVendaAcompanhado(cpf);

                if (cli != null)
                {
                    dbIntra.CLIENTE_VENDA_ACOMPs.DeleteOnSubmit(cli);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_VALE_MERCADORIAResult> ObterValeMercadoria(int ano, int mes, string codigoFilial)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_VALE_MERCADORIA(ano, mes, codigoFilial) select i).ToList();
        }

        public List<LOJA_VENDA> ObterVendaPorClienteEData(string codigoCliente, DateTime dataIni, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from xx in db.LOJA_VENDAs
                    where xx.CODIGO_CLIENTE == codigoCliente
                    && xx.DATA_VENDA >= dataIni
                    && xx.DATA_VENDA <= dataFim
                    orderby xx.DATA_DIGITACAO
                    select xx).ToList();

        }
        public LJ_FIDELIDADE_PROGRAMA ObterProgramaHandclub(DateTime dataIniResgate, DateTime dataFimResgate)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from xx in dbLinx.LJ_FIDELIDADE_PROGRAMAs
                    where xx.DATA_INICIO_RESGATE >= dataIniResgate
                    && xx.DATA_FIM_RESGATE <= dataFimResgate
                    select xx).SingleOrDefault();

        }
        public LJ_FIDELIDADE_CAMPANHA ObterCampanhaHandclub(string codigoCampanha)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from xx in dbLinx.LJ_FIDELIDADE_CAMPANHAs
                    where xx.CODIGO_CAMPANHA == codigoCampanha
                    select xx).SingleOrDefault();

        }

        public LJ_FIDELIDADE_PONTO ObterSaldoPontos(string codigoCliente, DateTime dataIniResgate, DateTime dataFimResgate)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            var codigoPrograma = ObterProgramaHandclub(dataIniResgate, dataFimResgate).COD_FIDELIDADE_PROGRAMA;

            return (from xx in dbLinx.LJ_FIDELIDADE_PONTOs
                    where xx.CODIGO_CLIENTE == codigoCliente
                    && xx.COD_FIDELIDADE_PROGRAMA == codigoPrograma
                    select xx).SingleOrDefault();
        }

        public List<LJ_FIDELIDADE_PONTO_CAMPANHA> ObterPontosCampanha(string codigoCliente, DateTime dataIniResgate, DateTime dataFimResgate)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);

            var codigoPrograma = ObterProgramaHandclub(dataIniResgate, dataFimResgate).COD_FIDELIDADE_PROGRAMA;

            return (from xx in dbLinx.LJ_FIDELIDADE_PONTO_CAMPANHAs
                    where xx.CODIGO_CLIENTE == codigoCliente
                    && xx.COD_FIDELIDADE_PROGRAMA == codigoPrograma
                    select xx).ToList();
        }
        public SP_OBTER_PONTO_HANDCLUB_SMSResult ObterPontosCampanhaHandclubSMS(string codigoCliente)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_PONTO_HANDCLUB_SMS(codigoCliente) select i).SingleOrDefault();
        }

        public List<SP_OBTER_HISTORICO_CLIENTE_HANDResult> ObterHistoricoClienteHand(string cpf)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from i in dbLinx.SP_OBTER_HISTORICO_CLIENTE_HAND(cpf) select i).ToList();
        }


        public void InserirWhatsAppHC(CLIENTE_WHATSAPP_HC wappHC)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.CLIENTE_WHATSAPP_HCs.InsertOnSubmit(wappHC);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public SP_GERAR_VENDA_PRODUTO_SEMANAResult GerarVendaProdutoSemana()
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from i in dbLinx.SP_GERAR_VENDA_PRODUTO_SEMANA() select i).SingleOrDefault();
        }
        public LINX_MOVIMENTO ObterLinxMovimento(DateTime dataMovimento)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from ul in dbIntra.LINX_MOVIMENTOs
                    where ul.DATA_MOVIMENTO == dataMovimento
                    select ul).SingleOrDefault();


        }
        public void InserirLinxMovimento(LINX_MOVIMENTO linxMovimento)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LINX_MOVIMENTOs.InsertOnSubmit(linxMovimento);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_VENDA_PRODUTO_SEMANAResult> ObterVendaProdutoSemana(DateTime dataIni, string colecao, int desenvOrigem, string grupoProduto, string produto, string nome, string tecido, string corFornecedor, string griffe, string produtoAcabado, string signed, string signedNome, string fornecedor, string colecaoLINX, string subGrupoProduto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDA_PRODUTO_SEMANA(dataIni, colecao, desenvOrigem, grupoProduto, produto, nome, tecido, corFornecedor, griffe, produtoAcabado, signed, signedNome, fornecedor, colecaoLINX, subGrupoProduto) select i).ToList();
        }
        public List<SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINEResult> ObterVendaProdutoSemanaOnline(DateTime dataIni, string colecao, int desenvOrigem, string grupoProduto, string produto, string nome, string tecido, string corFornecedor, string griffe, string produtoAcabado, string signed, string signedNome, string fornecedor, string colecaoLINX, string subGrupoProduto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDA_PRODUTO_SEMANA_ONLINE(dataIni, colecao, desenvOrigem, grupoProduto, produto, nome, tecido, corFornecedor, griffe, produtoAcabado, signed, signedNome, fornecedor, colecaoLINX, subGrupoProduto, 1) select i).ToList();
        }

        public List<SP_OBTER_VENDA_PRODUTO_SEMANA_FILIALResult> ObterVendaProdutoSemanaPorFilial(string produto, string cor, DateTime dataIni, DateTime dataFim)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDA_PRODUTO_SEMANA_FILIAL(produto, cor, dataIni, dataFim) select i).ToList();
        }
        public List<FN_OBTER_VENDA_SEMANA_PRODUTOResult> ObterVendaProdutoSemanaHistorico(string produto, string cor, DateTime dataEmissao)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.FN_OBTER_VENDA_SEMANA_PRODUTO(produto, cor, dataEmissao) select i).ToList();
        }
        public List<FN_OBTER_VENDA_SEMANA_PRODUTO_ONLINEResult> ObterVendaProdutoSemanaOnlineHistorico(string produto, string cor, DateTime dataEmissao)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.FN_OBTER_VENDA_SEMANA_PRODUTO_ONLINE(produto, cor, dataEmissao) select i).ToList();
        }

        public List<LOJA_VENDA_SEMANA> ObterSemanaVenda()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VENDA_SEMANAs
                    where m.DATA_FIM <= DateTime.Today.AddDays(7)
                    orderby m.DATA_INI descending
                    select m).ToList();
        }
        public LOJA_VENDA_SEMANA ObterSemanaVenda(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VENDA_SEMANAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }


        public List<SP_OBTER_SMS_CLIENTEResult> ObterSMSClientePorCampanha(string smsCampanha, string codigoCliente)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from i in dbLinx.SP_OBTER_SMS_CLIENTE(smsCampanha, codigoCliente) select i).ToList();
        }
        public List<SMS_CAMPANHA> ObterSMSCampanha()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from ai in dbIntra.SMS_CAMPANHAs select ai).ToList();
        }
        public SMS_CAMPANHA ObterSMSCampanha(string codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from ai in dbIntra.SMS_CAMPANHAs where ai.CODIGO == codigo select ai).SingleOrDefault();
        }
        public void InserirSMSClienteHist(SMS_CLIENTE_HISTORICO smsHist)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.SMS_CLIENTE_HISTORICOs.InsertOnSubmit(smsHist);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string EnviarSMSHandclubHist(int ddd, int telefone, string mensagem)
        {
            TwilioClient.Init(accountSid, authToken);

            try
            {
                var message = MessageResource.Create(
                    body: mensagem,
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    to: new Twilio.Types.PhoneNumber("+55" + ddd.ToString() + telefone.ToString() + "")
                );

                return "SMS enviado com sucesso. SID: " + message.Sid;

            }
            catch (ApiException ex)
            {
                return "ERRO: " + ex.Message;
            }

        }
        public string EnviarSMSHandclubHist(string telefonecompleto, string mensagem)
        {
            TwilioClient.Init(accountSid, authToken);

            try
            {
                var message = MessageResource.Create(
                    body: mensagem,
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    to: new Twilio.Types.PhoneNumber("+" + telefonecompleto)
                );

                return "SMS enviado com sucesso. SID: " + message.Sid;

            }
            catch (ApiException ex)
            {
                return "ERRO: " + ex.Message;
            }

        }

        public SMS_CLIENTE_HISTORICO EnviarSMSCampanha(int ddd, int telefone, string mensagem)
        {
            TwilioClient.Init(accountSid, authToken);

            var sms = new SMS_CLIENTE_HISTORICO();
            var toNumber = "+55" + ddd.ToString() + telefone.ToString() + "";

            try
            {
                var message = MessageResource.Create(
                    body: mensagem,
                    from: new Twilio.Types.PhoneNumber(fromNumber),
                    to: new Twilio.Types.PhoneNumber(toNumber),
                    smartEncoded: true
                );

                sms.SID = message.Sid;
                sms.ACCOUNTSID = message.AccountSid;
                sms.MENSAGEM = mensagem;
                sms.PARA_NUMERO = "";
                sms.DE_NUMERO = fromNumber;
                sms.PRECO = message.Price;
                sms.PRECO_UN = message.PriceUnit;
                sms.DATA_ENVIO = DateTime.Now;
                sms.ERRO = "SMS enviado com sucesso. SID: " + message.Sid;

            }
            catch (ApiException ex)
            {
                sms.ERRO = ex.Message;
            }

            return sms;

        }


        public List<LOJA_AGENDA_VISITA> ObterAgendaVisitaPorUsuario(int codigoUsuario, int ano, int mes, int dia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_AGENDA_VISITAs
                    where m.USUARIO == codigoUsuario
                    && m.DATA_VISITA.Year == ano
                    && m.DATA_VISITA.Month == mes
                    && m.DATA_VISITA.Day == dia
                    select m).ToList();
        }
        public LOJA_AGENDA_VISITA ObterAgendaVisitaPorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_AGENDA_VISITAs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public LOJA_AGENDA_VISITA ObterAgendaVisitaCor(int codigoUsuario, int ano, int mes, int dia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_AGENDA_VISITAs
                    where m.USUARIO == codigoUsuario
                    && m.DATA_VISITA.Year == ano
                    && m.DATA_VISITA.Month == mes
                    && m.DATA_VISITA.Day == dia
                    && m.COR != null
                    select m).FirstOrDefault();
        }
        public LOJA_AGENDA_VISITA ObterAgendaVisitaPorUsuarioFilial(int codigoUsuario, int ano, int mes, int dia, string codigoFilial)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_AGENDA_VISITAs
                    where m.USUARIO == codigoUsuario
                    && m.DATA_VISITA.Year == ano
                    && m.DATA_VISITA.Month == mes
                    && m.DATA_VISITA.Day == dia
                    && m.CODIGO_FILIAL == codigoFilial
                    select m).FirstOrDefault();
        }

        public void InserirAgendaVisita(LOJA_AGENDA_VISITA agendaVisita)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOJA_AGENDA_VISITAs.InsertOnSubmit(agendaVisita);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAgendaVisita(LOJA_AGENDA_VISITA agendaVisita)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            LOJA_AGENDA_VISITA _agendaVisita = ObterAgendaVisitaPorCodigo(agendaVisita.CODIGO);

            if (_agendaVisita != null)
            {
                _agendaVisita.CODIGO_FILIAL = agendaVisita.CODIGO_FILIAL;
                _agendaVisita.DATA_VISITA = agendaVisita.DATA_VISITA;
                _agendaVisita.DATA_BAIXA = agendaVisita.DATA_BAIXA;
                _agendaVisita.OBS = agendaVisita.OBS;
                _agendaVisita.COR = agendaVisita.COR;
            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirAgendaVisita(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            LOJA_AGENDA_VISITA agendaVisita = ObterAgendaVisitaPorCodigo(codigo);

            if (agendaVisita != null)
            {
                try
                {
                    dbIntra.LOJA_AGENDA_VISITAs.DeleteOnSubmit(agendaVisita);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<SP_OBTER_CALENDARIO_VISITASUPERResult> ObterCalendarioVisitaSuper(string ano, string mes, int usuario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_CALENDARIO_VISITASUPER(ano, mes, usuario) select i).ToList();
        }
        public List<SP_OBTER_CALENDARIO_VISITASUPER_SEMANAResult> ObterCalendarioVisitaSuperSemana(int ano, int usuario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from i in dbIntra.SP_OBTER_CALENDARIO_VISITASUPER_SEMANA(ano, usuario) select i).ToList();
        }


        public List<SP_OBTER_CALENDARIO_ANALISECOTAResult> ObterCalendarioAnaliseCota(string ano, string mes, int usuario, string codigoFilial, char filialSel)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from i in dbIntra.SP_OBTER_CALENDARIO_ANALISECOTA(ano, mes, usuario, codigoFilial, filialSel) select i).ToList();
        }

        public List<SP_OBTER_CALENDARIO_ANALISECOTA_ANOResult> ObterCalendarioAnaliseCotaAno(int ano, int usuario, string codigoFilial, char filialSel)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from i in dbLinx.SP_OBTER_CALENDARIO_ANALISECOTA_ANO(ano, usuario, codigoFilial, filialSel) select i).ToList();
        }

        public List<SP_OBTER_VENDA_PORGRIFFEResult> ObterVendaPorGriffe(DateTime dataIni, DateTime dataFim, bool? filialEtc, string codigoFilial, int codigoUsuario)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from i in dbLinx.SP_OBTER_VENDA_PORGRIFFE(dataIni, dataFim, filialEtc, codigoFilial, codigoUsuario) select i).ToList();
        }



        public List<LOJA_VITRINE_TIPO> ObterVitrineTipo()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_TIPOs
                    select m).ToList();
        }
        public LOJA_VITRINE_TIPO ObterVitrineTipoPorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_TIPOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }


        public List<SP_OBTER_VITRINE_FILIALResult> ObterVitrineFilial(DateTime dataIni, DateTime dataFim, string codigoFilial, char? favorito, char? sugestao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VITRINE_FILIAL(dataIni, dataFim, codigoFilial, favorito, sugestao) select i).ToList();
        }

        public List<LOJA_VITRINE> ObterVitrine()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINEs
                    select m).ToList();
        }
        public List<LOJA_VITRINE> ObterVitrinePorData(DateTime dataIni, DateTime dataFim)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINEs
                    where m.DATA_INI == dataIni
                    && m.DATA_FIM == dataFim
                    select m).ToList();
        }
        public LOJA_VITRINE ObterVitrinePorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINEs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public LOJA_VITRINE ObterVitrinePorFilialEData(string codigoFilial, DateTime dataIni, DateTime dataFim)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINEs
                    where m.CODIGO_FILIAL == codigoFilial
                    && m.DATA_INI == dataIni
                    && m.DATA_FIM == dataFim
                    select m).SingleOrDefault();
        }
        public void InserirVitrine(LOJA_VITRINE lojaVitrine)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOJA_VITRINEs.InsertOnSubmit(lojaVitrine);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarVitrine(LOJA_VITRINE lojaVitrine)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            LOJA_VITRINE _lojaVitrine = ObterVitrinePorCodigo(lojaVitrine.CODIGO);

            if (_lojaVitrine != null)
            {
                _lojaVitrine.CODIGO_FILIAL = lojaVitrine.CODIGO_FILIAL;
                _lojaVitrine.DATA_INI = lojaVitrine.DATA_INI;
                _lojaVitrine.DATA_FIM = lojaVitrine.DATA_FIM;
                _lojaVitrine.FAVORITO = lojaVitrine.FAVORITO;
                _lojaVitrine.SUGESTAO = lojaVitrine.SUGESTAO;
                _lojaVitrine.DATA_FINALIZADO = lojaVitrine.DATA_FINALIZADO;
                _lojaVitrine.DATA_ANALISADO = lojaVitrine.DATA_ANALISADO;
                _lojaVitrine.OBSERVACAO = lojaVitrine.OBSERVACAO;

            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirVitrine(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            LOJA_VITRINE lojaVitrine = ObterVitrinePorCodigo(codigo);

            if (lojaVitrine != null)
            {
                try
                {
                    dbIntra.LOJA_VITRINEs.DeleteOnSubmit(lojaVitrine);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public List<LOJA_VITRINE_MUNDO> ObterVitrineMundo()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDOs
                    select m).ToList();
        }
        public LOJA_VITRINE_MUNDO ObterVitrineMundoPorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public LOJA_VITRINE_MUNDO ObterVitrineMundoPorVitrineETipo(int codigoLojaVitrine, int codigoTipo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDOs
                    where m.LOJA_VITRINE == codigoLojaVitrine
                    && m.LOJA_VITRINE_TIPO == codigoTipo
                    select m).SingleOrDefault();
        }
        public void InserirVitrineMundo(LOJA_VITRINE_MUNDO lojaVitrineMundo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOJA_VITRINE_MUNDOs.InsertOnSubmit(lojaVitrineMundo);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarVitrineMundo(LOJA_VITRINE_MUNDO lojaVitrineMundo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            LOJA_VITRINE_MUNDO _lojaVitrineMundo = ObterVitrineMundoPorCodigo(lojaVitrineMundo.CODIGO);

            if (_lojaVitrineMundo != null)
            {
                _lojaVitrineMundo.LOJA_VITRINE = lojaVitrineMundo.LOJA_VITRINE;
                _lojaVitrineMundo.LOJA_VITRINE_TIPO = lojaVitrineMundo.LOJA_VITRINE_TIPO;
                _lojaVitrineMundo.FAVORITO = lojaVitrineMundo.FAVORITO;
            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirVitrineMundo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            LOJA_VITRINE_MUNDO lojaVitrineMundo = ObterVitrineMundoPorCodigo(codigo);

            if (lojaVitrineMundo != null)
            {
                try
                {
                    dbIntra.LOJA_VITRINE_MUNDOs.DeleteOnSubmit(lojaVitrineMundo);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public List<LOJA_VITRINE_MUNDO_FOTO> ObterVitrineMundoFoto()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDO_FOTOs
                    select m).ToList();
        }
        public LOJA_VITRINE_MUNDO_FOTO ObterVitrineMundoFotoPorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDO_FOTOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<LOJA_VITRINE_MUNDO_FOTO> ObterVitrineMundoFotoPorMundo(int codigoLojaVitrineMundo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDO_FOTOs
                    where m.LOJA_VITRINE_MUNDO == codigoLojaVitrineMundo
                    select m).ToList();
        }
        public LOJA_VITRINE_MUNDO_FOTO ObterVitrineMundoFotoPorMundoEOrdem(int codigoLojaVitrineMundo, int ordem)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VITRINE_MUNDO_FOTOs
                    where m.LOJA_VITRINE_MUNDO == codigoLojaVitrineMundo
                    && m.ORDEM == ordem
                    select m).FirstOrDefault();
        }
        public void InserirVitrineMundoFoto(LOJA_VITRINE_MUNDO_FOTO lojaVitrineMundoFoto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOJA_VITRINE_MUNDO_FOTOs.InsertOnSubmit(lojaVitrineMundoFoto);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarVitrineMundoFoto(LOJA_VITRINE_MUNDO_FOTO lojaVitrineMundoFoto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            LOJA_VITRINE_MUNDO_FOTO _lojaVitrineMundoFoto = ObterVitrineMundoFotoPorCodigo(lojaVitrineMundoFoto.CODIGO);

            if (_lojaVitrineMundoFoto != null)
            {
                _lojaVitrineMundoFoto.LOJA_VITRINE_MUNDO = lojaVitrineMundoFoto.LOJA_VITRINE_MUNDO;
                _lojaVitrineMundoFoto.FOTO = lojaVitrineMundoFoto.FOTO;
                _lojaVitrineMundoFoto.PRINCIPAL = lojaVitrineMundoFoto.PRINCIPAL;
                _lojaVitrineMundoFoto.FAVORITO = lojaVitrineMundoFoto.FAVORITO;
                _lojaVitrineMundoFoto.HARMONIA = lojaVitrineMundoFoto.HARMONIA;
                _lojaVitrineMundoFoto.POSICAO = lojaVitrineMundoFoto.POSICAO;
                _lojaVitrineMundoFoto.MONTAGEM = lojaVitrineMundoFoto.MONTAGEM;
                _lojaVitrineMundoFoto.CUBOS = lojaVitrineMundoFoto.CUBOS;
                _lojaVitrineMundoFoto.ACESSORIOS = lojaVitrineMundoFoto.ACESSORIOS;
                _lojaVitrineMundoFoto.ORDEM = lojaVitrineMundoFoto.ORDEM;
                _lojaVitrineMundoFoto.USUARIO = lojaVitrineMundoFoto.USUARIO;
                _lojaVitrineMundoFoto.DATA_QUALIFICACAO = lojaVitrineMundoFoto.DATA_QUALIFICACAO;
                _lojaVitrineMundoFoto.OBSERVACAO = lojaVitrineMundoFoto.OBSERVACAO;

            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirVitrineMundoFoto(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            LOJA_VITRINE_MUNDO_FOTO lojaVitrineMundoFoto = ObterVitrineMundoFotoPorCodigo(codigo);

            if (lojaVitrineMundoFoto != null)
            {
                try
                {
                    dbIntra.LOJA_VITRINE_MUNDO_FOTOs.DeleteOnSubmit(lojaVitrineMundoFoto);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public SP_GERAR_VITRINE_MUNDOSResult GerarVitrineMundos(DateTime dataIni, DateTime dataFim, string codigoFilial)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_GERAR_VITRINE_MUNDOS(dataIni, dataFim, codigoFilial) select i).SingleOrDefault();
        }
        public List<SP_OBTER_ECOM_VENDA_VENDEDORESResult> ObterVendaOnlineEcom(DateTime? dataIni, DateTime? dataFim, string vendedor, string nomeVendedor, string codigoFilial)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_ECOM_VENDA_VENDEDORES(dataIni, dataFim, vendedor, nomeVendedor, codigoFilial) select i).ToList();
        }

        public List<SP_OBTER_VENDEDOR_RANKACUMResult> ObterVendedorRankingAcum()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDEDOR_RANKACUM() select i).ToList();
        }
        public List<SP_OBTER_VENDEDOR_RANKSEMANAResult> ObterVendedorRankingSemana()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDEDOR_RANKSEMANA() select i).ToList();
        }

        public List<SP_OBTER_VENDEDOR_RANKACUM2020Result> ObterVendedorRankingAcum2020()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDEDOR_RANKACUM2020() select i).ToList();
        }
        public List<SP_OBTER_VENDEDOR_RANKSEMANA2020Result> ObterVendedorRankingSemana2020()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from i in dbIntra.SP_OBTER_VENDEDOR_RANKSEMANA2020() select i).ToList();
        }



        public List<LOJA_VENDA_REMARC> ObterLojaVendaRemarc()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VENDA_REMARCs
                    select m).ToList();
        }
        public LOJA_VENDA_REMARC ObterLojaVendaRemarc(string produto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_VENDA_REMARCs
                    where m.PRODUTO == produto
                    select m).SingleOrDefault();
        }
        public void InserirLojaVendaRemarc(LOJA_VENDA_REMARC lojaVendaRemarc)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                dbIntra.LOJA_VENDA_REMARCs.InsertOnSubmit(lojaVendaRemarc);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarLojaVendaRemarc(LOJA_VENDA_REMARC lojaVendaRemarc)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            LOJA_VENDA_REMARC _lojaVendaRemarc = ObterLojaVendaRemarc(lojaVendaRemarc.PRODUTO);

            if (_lojaVendaRemarc != null)
            {
                _lojaVendaRemarc.PRODUTO = lojaVendaRemarc.PRODUTO;
                _lojaVendaRemarc.PRECO = lojaVendaRemarc.PRECO;

            }

            try
            {
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirLojaVendaRemarc(string produto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            LOJA_VENDA_REMARC lojaVendaRemarc = ObterLojaVendaRemarc(produto);

            if (lojaVendaRemarc != null)
            {
                try
                {
                    dbIntra.LOJA_VENDA_REMARCs.DeleteOnSubmit(lojaVendaRemarc);
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
