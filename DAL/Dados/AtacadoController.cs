using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class AtacadoController
    {
        DCDataContext db;
        LINXDataContext LINXdb;
        INTRADataContext Intradb;

        public List<SP_RELATORIO_FATURAMENTO_ATACADOResult> ObterRelatorioFaturamentoAtacado(string colecao, string subColecao, string status, string nomeRepresentante, string nomeCliente)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 500;

            return db.SP_RELATORIO_FATURAMENTO_ATACADO(colecao.Trim(), subColecao.Trim(), nomeRepresentante.Trim(), nomeCliente.Trim())
                .Where(r => r.STATUS == status.Trim() || status == "" || status.Equals("0")).ToList();
        }

        public List<SP_OBTER_COMISSAO_REPRESENTANTEResult> ObterComissaoRepresentante(DateTime dataIni, DateTime dataFim, string representante)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;
            return db.SP_OBTER_COMISSAO_REPRESENTANTE(dataIni, dataFim, representante).ToList();
        }

        public int AtualizarClienteAtacado(string clienteAtacado, string email, string ddd, string telefone, decimal? limiteCredito, DateTime? bloqueioFat, bool? semCredito, string obs, string obsFaturamento, string dataSerasa, string dataFundacao, string qtdeProtesto, string tipoBloqueio, string inscricaoEstadual)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);

            return LINXdb.SP_ATUALIZAR_CLI_ATACADO(clienteAtacado, email, ddd, telefone, limiteCredito, bloqueioFat, semCredito, obs, obsFaturamento, dataSerasa, dataFundacao, qtdeProtesto, tipoBloqueio, inscricaoEstadual);
        }

        public List<SP_OBTER_CLIENTES_INADIMPLENTESResult> ObterClientesInadimplentes(string representante, string colecao, string codCliente, string clienteAtacado)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return LINXdb.SP_OBTER_CLIENTES_INADIMPLENTES(representante, colecao, codCliente, clienteAtacado).ToList();
        }

        public List<SP_OBTER_CLIENTES_FATURA_ATRASOResult> ObterClientesFaturaAtraso(string colecao, string codCliente)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return LINXdb.SP_OBTER_CLIENTES_FATURA_ATRASO(colecao, codCliente).ToList();
        }

        public List<SP_OBTER_HIST_VENDA_CLIENTE_ATACResult> ObterHistVendaClienteAtacado(string clienteAtacado)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return LINXdb.SP_OBTER_HIST_VENDA_CLIENTE_ATAC(clienteAtacado).ToList();
        }

        public List<SP_OBTER_PEDIDO_CLIENTEResult> ObterPedidoClienteAtacado(string colecao, string clienteAtacado)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return LINXdb.SP_OBTER_PEDIDO_CLIENTE(colecao, clienteAtacado).ToList();
        }

        //ATACADO_REP_EMAIL
        public List<ATACADO_REP_EMAIL> ObterAtacadoRepresentanteEmail()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.ATACADO_REP_EMAILs
                    orderby m.REPRESENTANTE ascending, m.FILIAL ascending, m.DATA_ENVIO descending
                    select m).ToList();
        }
        public ATACADO_REP_EMAIL ObterAtacadoRepresentanteEmail(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.ATACADO_REP_EMAILs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<ATACADO_REP_EMAIL> ObterAtacadoRepresentanteEmail(string representante, string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.ATACADO_REP_EMAILs
                    where m.REPRESENTANTE.Trim() == representante.Trim() &&
                        m.FILIAL.Trim() == filial.Trim()
                    orderby m.REPRESENTANTE ascending, m.FILIAL ascending, m.DATA_ENVIO descending
                    select m).ToList();
        }
        public int InserirAtacadoRepresentanteEmail(ATACADO_REP_EMAIL _atacRepEmail)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ATACADO_REP_EMAILs.InsertOnSubmit(_atacRepEmail);
                db.SubmitChanges();

                return _atacRepEmail.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM ATACADO_REP_EMAIL

        public List<ATACADO_REP_COMISSAO> ObterComissaoRep()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.ATACADO_REP_COMISSAOs orderby m.REPRESENTANTE, m.FILIAL select m).ToList();
        }
        public ATACADO_REP_COMISSAO ObterComissaoRep(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.ATACADO_REP_COMISSAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<ATACADO_REP_COMISSAO> ObterComissaoRep(string rep)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.ATACADO_REP_COMISSAOs
                    where m.REPRESENTANTE.Trim() == rep.Trim()
                    orderby m.FILIAL
                    select m).ToList();
        }
        public void InserirComissaoRep(ATACADO_REP_COMISSAO _comissaoREP)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.ATACADO_REP_COMISSAOs.InsertOnSubmit(_comissaoREP);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarComissaoRep(ATACADO_REP_COMISSAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            ATACADO_REP_COMISSAO _comissaoREP = ObterComissaoRep(_novo.CODIGO);

            if (_novo != null)
            {
                _comissaoREP.CODIGO = _novo.CODIGO;
                _comissaoREP.REPRESENTANTE = _novo.REPRESENTANTE;
                _comissaoREP.FILIAL = _novo.FILIAL;
                _comissaoREP.COMISSAO = _novo.COMISSAO;
                _comissaoREP.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _comissaoREP.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

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
        public void ExcluirComissaoRep(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                ATACADO_REP_COMISSAO _comissaoREP = ObterComissaoRep(codigo);
                if (_comissaoREP != null)
                {
                    db.ATACADO_REP_COMISSAOs.DeleteOnSubmit(_comissaoREP);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ATACADO_BLOQ_CLIENTE> ObterHistClienteBloqueado(string clifor)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ATACADO_BLOQ_CLIENTEs
                    where m.CLIFOR == clifor
                    orderby m.COLECAO, m.DATA_INCLUSAO
                    select m).ToList();
        }
        public List<ATACADO_BLOQ_CLIENTE> ObterHistClienteBloqueado(string clifor, string colecao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ATACADO_BLOQ_CLIENTEs
                    where m.CLIFOR == clifor
                    && m.COLECAO == colecao
                    orderby m.COLECAO, m.DATA_INCLUSAO
                    select m).ToList();
        }
        public ATACADO_BLOQ_CLIENTE ObterHistClienteBloqueadoPorArquivo(string clifor, string colecao, string nomeArquivo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ATACADO_BLOQ_CLIENTEs
                    where m.CLIFOR == clifor
                    && m.COLECAO == colecao
                    && m.ARQUIVO_PDF == nomeArquivo
                    orderby m.COLECAO, m.DATA_INCLUSAO
                    select m).SingleOrDefault();
        }
        public void InserirHistClienteBloqueado(ATACADO_BLOQ_CLIENTE _cliBloq)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ATACADO_BLOQ_CLIENTEs.InsertOnSubmit(_cliBloq);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirArquivoPDF(string clifor, string colecao, string arquivoPDF)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            ATACADO_BLOQ_CLIENTE atacBloqCliente = ObterHistClienteBloqueadoPorArquivo(clifor, colecao, arquivoPDF);

            if (atacBloqCliente != null)
            {
                atacBloqCliente.ARQUIVO_PDF = "";

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public List<SP_OBTER_VENDA_CLI_REPResult> ObterVendaColecaoClienteRepresentante(string colecaoDe, string colecaoAte, string representante, bool colunaRepresentante)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return LINXdb.SP_OBTER_VENDA_CLI_REP(colecaoDe, colecaoAte, representante, colunaRepresentante).ToList();
        }

        public List<ATACADO_REP> ObterRepresentanteUF()
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ATACADO_REPs orderby m.REPRESENTANTE select m).ToList();
        }

        public List<ATACADO_REP_COTA> ObterClienteCota(string colecao)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ATACADO_REP_COTAs
                    where m.COLECAO == colecao
                    orderby m.CLIENTE
                    select m).ToList();
        }
        public ATACADO_REP_COTA ObterClienteCota(string colecao, string cliente)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.ATACADO_REP_COTAs
                    where m.COLECAO == colecao && m.CLIENTE.Trim() == cliente.Trim()
                    orderby m.CLIENTE
                    select m).SingleOrDefault();
        }
        public void InserirClienteCota(ATACADO_REP_COTA _clienteCota)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.ATACADO_REP_COTAs.InsertOnSubmit(_clienteCota);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarClienteCota(ATACADO_REP_COTA _novo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            ATACADO_REP_COTA atacadoCotaCliente = ObterClienteCota(_novo.COLECAO, _novo.CLIENTE);

            if (_novo != null)
            {
                atacadoCotaCliente.CODIGO = _novo.CODIGO;
                atacadoCotaCliente.COLECAO = _novo.COLECAO;
                atacadoCotaCliente.CLIENTE = _novo.CLIENTE;
                atacadoCotaCliente.QTDE = _novo.QTDE;
                atacadoCotaCliente.VALOR = _novo.VALOR;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public List<SP_OBTER_CLIENTE_COLECAO_COTAResult> ObterClienteColecaoCota(string colecao, string representante, string cliente, string uf)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return LINXdb.SP_OBTER_CLIENTE_COLECAO_COTA(colecao, representante, cliente, uf).ToList();
        }

        public List<SP_OBTER_CLIENTE_COLECAO_COTA_COLResult> ObterClienteColecaoCotaCol(string colecao)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return LINXdb.SP_OBTER_CLIENTE_COLECAO_COTA_COL(colecao).ToList();
        }

        public List<SP_OBTER_CLIENTE_ATC_BLOQResult> ObterClienteAtacadoBloq(string colecao, string clifor, string nomeCliente, string cnpj, string uf, char? fatBloqueado, char? semCredito)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            LINXdb.CommandTimeout = 0;
            return LINXdb.SP_OBTER_CLIENTE_ATC_BLOQ(colecao, clifor, nomeCliente, cnpj, uf, fatBloqueado, semCredito).ToList();
        }

        public List<COND_ATAC_PGTO> ObterAtacadoCondPgto()
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);
            return (from m in LINXdb.COND_ATAC_PGTOs
                    orderby m.DESC_COND_PGTO
                    select m).ToList();
        }

        public int InserirNetshoesEtiqueta(NETSHOES_ETIQUETA etiqueta)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            try
            {
                Intradb.NETSHOES_ETIQUETAs.InsertOnSubmit(etiqueta);
                Intradb.SubmitChanges();

                return etiqueta.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /* TABELAS ATACADO PEDIDO */



        /*  */





    }
}
