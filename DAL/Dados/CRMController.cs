using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class CRMController
    {
        INTRADataContext dbIntra;
        LINXDataContext dbLinx;

        public List<TRIMESTRE> ObterTrimestre()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.TRIMESTREs
                    orderby m.DATA
                    select m).ToList();
        }

        public List<CRM_ACAO> ObterAcao()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_ACAOs
                    orderby m.ACAO
                    select m).ToList();
        }
        public CRM_ACAO ObterAcaoPorCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_ACAOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }


        public List<CRM_CLIENTE_ACAO> ObterClienteAcaoPorCodigoAcao(int codigoAcao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_ACAOs
                    where m.CRM_ACAO == codigoAcao
                    orderby m.NOME
                    select m).ToList();
        }
        public List<CRM_CLIENTE_ACAO> ObterClienteAcao(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_ACAOs
                    where m.CPF == cpf
                    orderby m.DATA_ACAO descending
                    select m).ToList();
        }
        public CRM_CLIENTE_ACAO ObterClienteAcao(string cpf, int codigoAcao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_ACAOs
                    where m.CPF == cpf && m.CRM_ACAO == codigoAcao
                    select m).SingleOrDefault();
        }
        public void InserirClienteAcao(CRM_CLIENTE_ACAO clienteAcao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CRM_CLIENTE_ACAOs.InsertOnSubmit(clienteAcao);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarClienteAcao(CRM_CLIENTE_ACAO clienteAcao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            CRM_CLIENTE_ACAO _clienteAcao = ObterClienteAcao(clienteAcao.CPF, clienteAcao.CRM_ACAO);

            if (_clienteAcao != null)
            {
                _clienteAcao.CPF = clienteAcao.CPF;
                _clienteAcao.CODIGO_FILIAL = clienteAcao.CODIGO_FILIAL;
                _clienteAcao.NOME = clienteAcao.NOME;
                _clienteAcao.TELEFONE = clienteAcao.TELEFONE;
                _clienteAcao.CELULAR = clienteAcao.CELULAR;
                _clienteAcao.ENDERECO = clienteAcao.ENDERECO;
                _clienteAcao.USUARIO = clienteAcao.USUARIO;
                _clienteAcao.DATA_BAIXA = clienteAcao.DATA_BAIXA;
                _clienteAcao.OBSERVACAO = clienteAcao.OBSERVACAO;

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
        public void ExcluirClienteAcao(string cpf, int codigoAcao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                CRM_CLIENTE_ACAO _clienteAcao = ObterClienteAcao(cpf, codigoAcao);
                if (_clienteAcao != null)
                {
                    dbIntra.CRM_CLIENTE_ACAOs.DeleteOnSubmit(_clienteAcao);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<CRM_TIPO> ObterCRMTipo()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_TIPOs
                    orderby m.TIPO
                    select m).ToList();
        }
        public CRM_TIPO ObterCRMTipoCodigo(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_TIPOs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }


        public List<CRM_CLIENTE_TIPO> ObterClienteTipoHist(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_TIPOs
                    where m.CPF == cpf
                    orderby m.DATA descending
                    select m).ToList();
        }
        public CRM_CLIENTE_TIPO ObterClienteTipoHistPorCodigo(string cpf, int codigoTipo, DateTime data)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_TIPOs
                    where m.CPF == cpf
                    && m.CRM_TIPO == codigoTipo
                    && m.DATA == data
                    select m).SingleOrDefault();
        }
        public void InserirClienteTipoHist(CRM_CLIENTE_TIPO tipoCliente)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CRM_CLIENTE_TIPOs.InsertOnSubmit(tipoCliente);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarClienteTipoHist(CRM_CLIENTE_TIPO tipoCliente)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            CRM_CLIENTE_TIPO _tipoCliente = ObterClienteTipoHistPorCodigo(tipoCliente.CPF, tipoCliente.CRM_TIPO, tipoCliente.DATA);

            if (_tipoCliente != null)
            {
                _tipoCliente.OBS = tipoCliente.OBS;

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
        public void ExcluirClienteTipoHist(string cpf, int codigoTipo, DateTime data)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                CRM_CLIENTE_TIPO _tipoCliente = ObterClienteTipoHistPorCodigo(cpf, codigoTipo, data);
                if (_tipoCliente != null)
                {
                    dbIntra.CRM_CLIENTE_TIPOs.DeleteOnSubmit(_tipoCliente);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CRM_HANDCLUB_ZERADO ObterClienteHandZerado(int codigoPrograma, string codigoCliente)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_HANDCLUB_ZERADOs
                    where m.COD_FIDELIDADE_PROGRAMA == codigoPrograma
                    && m.CODIGO_CLIENTE == codigoCliente
                    select m).SingleOrDefault();
        }
        public void InserirClienteHandZerado(CRM_HANDCLUB_ZERADO handZerado)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CRM_HANDCLUB_ZERADOs.InsertOnSubmit(handZerado);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirClienteHandZerado(int codigoPrograma, string codigoCliente)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                CRM_HANDCLUB_ZERADO handZerado = ObterClienteHandZerado(codigoPrograma, codigoCliente);
                if (handZerado != null)
                {
                    dbIntra.CRM_HANDCLUB_ZERADOs.DeleteOnSubmit(handZerado);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_CRM_CICLOCLIENTEResult> ObterCRMCicloCliente(DateTime dataTri)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_CICLOCLIENTE(dataTri) select j).ToList();
        }

        public List<SP_OBTER_CRM_RFVResult> ObterCRMRFV(DateTime dataTri)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_RFV(dataTri) select j).ToList();
        }

        public List<SP_OBTER_CRM_RFV_SCOREResult> ObterCRMRFVSCORE(DateTime dataTri)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_RFV_SCORE(dataTri) select j).ToList();
        }

        public List<SP_OBTER_CRM_CLIENTEResult> ObterCRMCliente(DateTime dataTri, string cpf, string nome, char? status, int recencia, int frequencia, int valor, string cicloAtual, string cicloAnt)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_CLIENTE(dataTri, cpf, nome, status, recencia, frequencia, valor, cicloAtual, cicloAnt) select j).ToList();
        }

        public SP_OBTER_CRM_CLIENTE_DETResult ObterCRMClienteDET(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_CLIENTE_DET(cpf) select j).SingleOrDefault();
        }

        public List<SP_OBTER_CRM_ULTIMOS_PRODUTOSResult> ObterCRMUltimosProdutos(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_ULTIMOS_PRODUTOS(cpf) select j).ToList();
        }

        public List<SP_OBTER_CRM_MAISCOMPRAResult> ObterCRMMaisCompra(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_MAISCOMPRA(cpf) select j).ToList();
        }

        public List<SP_OBTER_CRM_HISTMENSALResult> ObterCRMHistMensal(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_HISTMENSAL(cpf) select j).ToList();
        }

        public List<SP_OBTER_CRM_HISTMENSAL_PRODUTOResult> ObterCRMHistMensalProduto(string cpf, string tipo, int ano, int mes)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from j in dbIntra.SP_OBTER_CRM_HISTMENSAL_PRODUTO(cpf, tipo, ano, mes) select j).ToList();
        }

        public HBF_ATU_HANDCLUB_ZERAHANDResult ZerarHandclub(int codigoPrograma = 0)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            dbLinx.CommandTimeout = 0;
            return (from j in dbLinx.HBF_ATU_HANDCLUB_ZERAHAND(codigoPrograma) select j).SingleOrDefault();
        }

        public List<CRM_CLIENTE_CUPOM> ObterClienteCupom()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_CUPOMs
                    orderby m.DATA_CRIACAO ascending
                    select m).ToList();
        }
        public List<CRM_CLIENTE_CUPOM> ObterClienteCupom(string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_CUPOMs
                    where m.CPF == cpf
                    orderby m.DATA_CRIACAO ascending
                    select m).ToList();
        }
        public CRM_CLIENTE_CUPOM ObterClienteCupomPorCodigo(string cpf, string cupom, int? versao = null)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.CRM_CLIENTE_CUPOMs
                    where m.CPF == cpf && m.CUPOM == cupom && m.VERSAO == versao
                    select m).SingleOrDefault();
        }
        public int InserirClienteCupom(CRM_CLIENTE_CUPOM cliCupom)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.CRM_CLIENTE_CUPOMs.InsertOnSubmit(cliCupom);
                dbIntra.SubmitChanges();

                return cliCupom.ID_CUPOM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarClienteCupom(CRM_CLIENTE_CUPOM cliCupom)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            CRM_CLIENTE_CUPOM _cliCupom = ObterClienteCupomPorCodigo(cliCupom.CPF, cliCupom.CUPOM, cliCupom.VERSAO);

            if (_cliCupom != null)
            {
                _cliCupom.DATA_EXCLUSAO = cliCupom.DATA_EXCLUSAO;
                _cliCupom.MOTIVO = cliCupom.MOTIVO;

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
        public void ExcluirClienteCupom(string cpf, string cupom, int? versao = null)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                CRM_CLIENTE_CUPOM cliCupom = ObterClienteCupomPorCodigo(cpf, cupom, versao);
                if (cliCupom != null)
                {
                    dbIntra.CRM_CLIENTE_CUPOMs.DeleteOnSubmit(cliCupom);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
