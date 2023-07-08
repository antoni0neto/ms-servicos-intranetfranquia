using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class RHController
    {
        DCDataContext db;
        INTRADataContext dbIntra;

        //FERIADO
        public List<RH_FERIADO> ObterFeriado()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FERIADOs
                    orderby m.DATA
                    select m).ToList();
        }
        public RH_FERIADO ObterFeriado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FERIADOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<RH_FERIADO> ObterFeriado(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FERIADOs
                    where
                        m.CODIGO_FILIAL == filial.Trim()
                    orderby m.DATA
                    select m).ToList();
        }
        public int InserirFeriado(RH_FERIADO _feriado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_FERIADOs.InsertOnSubmit(_feriado);
                db.SubmitChanges();

                return _feriado.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFeriado(RH_FERIADO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_FERIADO _feriado = ObterFeriado(_novo.CODIGO);

            if (_novo != null)
            {
                _feriado.CODIGO = _novo.CODIGO;
                _feriado.DESCRICAO = _novo.DESCRICAO;
                _feriado.DATA = _novo.DATA;
                _feriado.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _feriado.TIPO = _novo.TIPO;
                if (_novo.USUARIO_INCLUSAO != null)
                    _feriado.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                if (_novo.DATA_INCLUSAO != null)
                    _feriado.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _feriado.STATUS = _novo.STATUS;
                _feriado.TRABALHADO = _novo.TRABALHADO;

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
        public void ExcluirFeriado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_FERIADO _feriado = ObterFeriado(codigo);
                if (_feriado != null)
                {
                    db.RH_FERIADOs.DeleteOnSubmit(_feriado);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM FERIADO

        public List<RH_PONTO_BATIDA_TIPO> ObterBatidaTipo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDA_TIPOs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public RH_PONTO_BATIDA_TIPO ObterBatidaTipo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDA_TIPOs where m.CODIGO == codigo select m).SingleOrDefault();
        }

        //PONTO_PERIODO_TRAB
        public List<RH_PONTO_PERIODO_TRAB> ObterPeriodoTrabalho()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_PERIODO_TRABs
                    orderby m.DESCRICAO, m.DIA_INICIAL
                    select m).ToList();
        }
        public RH_PONTO_PERIODO_TRAB ObterPeriodoTrabalho(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_PERIODO_TRABs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public RH_PONTO_PERIODO_TRAB ObterPeriodoTrabalho(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_PERIODO_TRABs where m.DESCRICAO == descricao select m).SingleOrDefault();
        }
        public int InserirPeriodoTrabalho(RH_PONTO_PERIODO_TRAB _perTrabalho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_PONTO_PERIODO_TRABs.InsertOnSubmit(_perTrabalho);
                db.SubmitChanges();

                return _perTrabalho.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPeriodoTrabalho(RH_PONTO_PERIODO_TRAB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_PONTO_PERIODO_TRAB _perTrabalho = ObterPeriodoTrabalho(_novo.CODIGO);

            if (_novo != null)
            {
                _perTrabalho.CODIGO = _novo.CODIGO;
                _perTrabalho.DESCRICAO = _novo.DESCRICAO;
                _perTrabalho.SUB_DESCRICAO = _novo.SUB_DESCRICAO;
                _perTrabalho.DIA_INICIAL = _novo.DIA_INICIAL;
                _perTrabalho.DIA_FINAL = _novo.DIA_FINAL;
                _perTrabalho.HORA_INICIAL = _novo.HORA_INICIAL;
                _perTrabalho.HORA_FINAL = _novo.HORA_FINAL;

                if (_novo.USUARIO_ALTERACAO != null)
                    _perTrabalho.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                if (_novo.DATA_ALTERACAO != null)
                    _perTrabalho.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                _perTrabalho.STATUS = _novo.STATUS;

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
        public void ExcluirPeriodoTrabalho(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_PONTO_PERIODO_TRAB _perTrabalho = ObterPeriodoTrabalho(codigo);
                if (_perTrabalho != null)
                {
                    db.RH_PONTO_PERIODO_TRABs.DeleteOnSubmit(_perTrabalho);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PONTO_PERIODO_TRAB

        public List<RH_FUNCIONARIO_PERIODO_EXP> ObterPeriodoExperiencia()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIO_PERIODO_EXPs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public RH_FUNCIONARIO_PERIODO_EXP ObterPeriodoExperiencia(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIO_PERIODO_EXPs where m.CODIGO == codigo select m).SingleOrDefault();
        }

        //FUNCIONARIO
        public List<RH_FUNCIONARIO> ObterFuncionario()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIOs select m).ToList();
        }
        public List<SP_OBTER_FUNCIONARIOResult> ObterFuncionario(int? codigoFuncionario, string seqVendedor, string filial, string cpf, string nome)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_OBTER_FUNCIONARIO(codigoFuncionario, seqVendedor, filial, cpf, nome) select h).ToList();
        }
        public List<SP_OBTER_FUNCIONARIOResult> ObterFuncionarioComCodigo(int? codigoFuncionario, string seqVendedor, string filial, string cpf, string nome)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h
                    in db.SP_OBTER_FUNCIONARIO(codigoFuncionario, seqVendedor, filial, cpf, nome)
                    select h).ToList()
                    .Select(c => new SP_OBTER_FUNCIONARIOResult()
                    {
                        CODIGO = c.CODIGO,
                        VENDEDOR = c.VENDEDOR,
                        DATA_DEMISSAO = c.DATA_DEMISSAO,
                        NOME = string.Format("{0} - {1}", c.VENDEDOR.Trim(), c.NOME.Trim()),
                        VENDEDOR_APELIDO = c.NOME //USADO APENAS PARA ORDENACAO
                    }).Distinct().OrderBy(c => c.VENDEDOR_APELIDO).ToList();
        }
        public RH_FUNCIONARIO ObterFuncionario(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirFuncionario(RH_FUNCIONARIO _funcionario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_FUNCIONARIOs.InsertOnSubmit(_funcionario);
                db.SubmitChanges();

                return _funcionario.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.Connection.State != System.Data.ConnectionState.Closed)
                    db.Connection.Close();
            }
        }
        public void AtualizarFuncionario(RH_FUNCIONARIO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_FUNCIONARIO _funcionario = ObterFuncionario(_novo.CODIGO);

            if (_novo != null)
            {
                _funcionario.CODIGO = _novo.CODIGO;
                _funcionario.NOME = _novo.NOME;
                _funcionario.CPF = _novo.CPF;
                _funcionario.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _funcionario.VENDEDOR = _novo.VENDEDOR;
                _funcionario.DESC_CARGO = _novo.DESC_CARGO;
                _funcionario.RH_FUNCIONARIO_PERIODO_EXP = _novo.RH_FUNCIONARIO_PERIODO_EXP;
                _funcionario.DATA_ADMISSAO = _novo.DATA_ADMISSAO;
                _funcionario.DATA_DEMISSAO = _novo.DATA_DEMISSAO;
                _funcionario.FOTO = _novo.FOTO;
                _funcionario.FOTO_CINZA = _novo.FOTO_CINZA;
                _funcionario.BATE_PONTO = _novo.BATE_PONTO;
                _funcionario.SEXO = _novo.SEXO;

                _funcionario.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _funcionario.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _funcionario.STATUS = _novo.STATUS;
                _funcionario.WAPP_BAIXA = _novo.WAPP_BAIXA;

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
        public void ExcluirFuncionario(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_FUNCIONARIO _funcionario = ObterFuncionario(codigo);
                if (_funcionario != null)
                {
                    db.RH_FUNCIONARIOs.DeleteOnSubmit(_funcionario);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM FUNCIONARIO

        //FUNCIONARIO
        public List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> ObterFuncionarioPeriodoTrab()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs
                    orderby m.RH_PONTO_PERIODO_TRAB1.DESCRICAO
                    select m).ToList();
        }
        public RH_FUNCIONARIO_PONTO_PERIODO_TRAB ObterFuncionarioPeriodoTrab(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public RH_FUNCIONARIO_PONTO_PERIODO_TRAB ObterFuncionarioPeriodoTrab(int funcionario, int periodoTrabalho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs
                    where m.RH_FUNCIONARIO == funcionario &&
                    m.RH_PONTO_PERIODO_TRAB == periodoTrabalho
                    select m).SingleOrDefault();
        }
        public List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> ObterFuncionarioPeriodoTrab(string funcionario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs
                    where
                        m.RH_FUNCIONARIO == Convert.ToInt32(funcionario)
                    orderby m.RH_PONTO_PERIODO_TRAB1.DESCRICAO
                    select m).ToList();
        }
        public int InserirFuncionarioPeriodoTrab(RH_FUNCIONARIO_PONTO_PERIODO_TRAB _funcPerTrab)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs.InsertOnSubmit(_funcPerTrab);
                db.SubmitChanges();

                return _funcPerTrab.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFuncionarioPeriodoTrab(RH_FUNCIONARIO_PONTO_PERIODO_TRAB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_FUNCIONARIO_PONTO_PERIODO_TRAB _funcPerTrab = ObterFuncionarioPeriodoTrab(_novo.CODIGO);

            if (_novo != null)
            {
                _funcPerTrab.CODIGO = _novo.CODIGO;
                _funcPerTrab.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _funcPerTrab.RH_PONTO_PERIODO_TRAB = _novo.RH_PONTO_PERIODO_TRAB;

                if (_novo.DATA_INCLUSAO != null)
                    _funcPerTrab.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _funcPerTrab.STATUS = _novo.STATUS;

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
        public void ExcluirFuncionarioPeriodoTrab(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_FUNCIONARIO_PONTO_PERIODO_TRAB _funcPerTrab = ObterFuncionarioPeriodoTrab(codigo);
                if (_funcPerTrab != null)
                {
                    db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs.DeleteOnSubmit(_funcPerTrab);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirFuncionarioPeriodoTrab(string funcionario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                List<RH_FUNCIONARIO_PONTO_PERIODO_TRAB> _funcPerTrab = ObterFuncionarioPeriodoTrab(funcionario);
                foreach (RH_FUNCIONARIO_PONTO_PERIODO_TRAB per in _funcPerTrab)
                {
                    if (per != null)
                    {
                        db.RH_FUNCIONARIO_PONTO_PERIODO_TRABs.DeleteOnSubmit(per);
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM FUNCIONARIO

        //PONTO_BATIDA
        public List<RH_PONTO_BATIDA> ObterPontoBatida()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDAs
                    select m).ToList();
        }
        public List<RH_PONTO_BATIDA> ObterPontoBatida(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDAs
                    where
                        m.CODIGO_FILIAL == filial
                        && (m.STATUS != 'E' || m.STATUS == null)
                    orderby m.CODIGO_FILIAL, m.RH_FUNCIONARIO1.NOME, m.FUNCIONARIO_NOME, m.DATA
                    select m).ToList();
        }
        public List<RH_PONTO_BATIDA> ObterPontoBatida(string filial, DateTime data)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDAs
                    where
                        m.CODIGO_FILIAL == filial &&
                        m.DATA == data
                    orderby m.DATA
                    select m).ToList();
        }
        public List<RH_PONTO_BATIDA> ObterPontoBatida(string filial, string funcionario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDAs
                    where
                        m.CODIGO_FILIAL == filial &&
                        m.RH_FUNCIONARIO == Convert.ToInt32(funcionario)
                    orderby m.DATA
                    select m).ToList();
        }
        public RH_PONTO_BATIDA ObterPontoBatida(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public RH_PONTO_BATIDA ObterPontoBatida(string filial, DateTime data, string funcionario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_PONTO_BATIDAs
                    where
                        m.CODIGO_FILIAL == filial &&
                        m.DATA == data &&
                        m.RH_FUNCIONARIO == Convert.ToInt32(funcionario)
                    select m).SingleOrDefault();
        }
        public int InserirPontoBatida(RH_PONTO_BATIDA _batida)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_PONTO_BATIDAs.InsertOnSubmit(_batida);
                db.SubmitChanges();

                return _batida.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarPontoBatida(RH_PONTO_BATIDA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_PONTO_BATIDA _batida = ObterPontoBatida(_novo.CODIGO);

            if (_novo != null)
            {
                _batida.CODIGO = _novo.CODIGO;
                _batida.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _batida.DATA = _novo.DATA;
                if (_novo.RH_FUNCIONARIO != null)
                    _batida.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _batida.RH_PONTO_PERIODO_TRAB = _novo.RH_PONTO_PERIODO_TRAB;
                _batida.RH_PONTO_BATIDA_TIPO = _novo.RH_PONTO_BATIDA_TIPO;
                _batida.ENTRADA1 = _novo.ENTRADA1;
                _batida.SAIDA1 = _novo.SAIDA1;
                _batida.ENTRADA2 = _novo.ENTRADA2;
                _batida.SAIDA2 = _novo.SAIDA2;
                _batida.ENTRADA3 = _novo.ENTRADA3;
                _batida.SAIDA3 = _novo.SAIDA3;
                _batida.HORAS_NORMAIS = _novo.HORAS_NORMAIS;
                _batida.HORAS_EXTRAS = _novo.HORAS_EXTRAS;
                _batida.STATUS = _novo.STATUS;

                if (_novo.USUARIO_INCLUSAO != null)
                    _batida.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                if (_novo.USUARIO_ALTERACAO != null)
                    _batida.USUARIO_ALTERACAO = _novo.USUARIO_ALTERACAO;
                if (_novo.USUARIO_EXCLUSAO != null)
                    _batida.USUARIO_EXCLUSAO = _novo.USUARIO_EXCLUSAO;

                if (_novo.DATA_INCLUSAO != null)
                    _batida.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                if (_novo.DATA_ALTERACAO != null)
                    _batida.DATA_ALTERACAO = _novo.DATA_ALTERACAO;
                if (_novo.DATA_EXCLUSAO != null)
                    _batida.DATA_EXCLUSAO = _novo.DATA_EXCLUSAO;

                _batida.OBSERVACAO = _novo.OBSERVACAO;
                if (_novo.FUNCIONARIO_NOME != null)
                    _batida.FUNCIONARIO_NOME = _novo.FUNCIONARIO_NOME;

                if (_novo.USUARIO_ABERTURA_DIA != null)
                    _batida.USUARIO_ABERTURA_DIA = _novo.USUARIO_ABERTURA_DIA;
                if (_novo.DATA_ABERTURA_DIA != null)
                    _batida.DATA_ABERTURA_DIA = _novo.DATA_ABERTURA_DIA;

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
        public void ExcluirPontoBatida(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_PONTO_BATIDA _batida = ObterPontoBatida(codigo);
                if (_batida != null)
                {
                    db.RH_PONTO_BATIDAs.DeleteOnSubmit(_batida);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PONTO_BATIDA

        //SP_GERAR_BATIDA_DIAResult
        public List<SP_GERAR_BATIDA_DIAResult> GerarBatidaPonto(string codigoFilial, DateTime data, int usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_GERAR_BATIDA_DIA(codigoFilial, data, usuario) select h).ToList();
        }
        //FIM SP_GERAR_BATIDA_DIAResult

        //SP_CRIAR_FUNCIONARIO_LINXResult
        public SP_CRIAR_FUNCIONARIO_LINXResult CriarFuncionarioLINX(
            string vendedorCodigo,
            string senha,
            string codigoFilial,
            string apelido,
            string nomeVendedor,
            decimal comissao,
            string rg,
            string endereco,
            string complemento,
            string cidade,
            string cep,
            string telefone,
            string uf,
            string cpf,
            char gerente,
            bool operaCaixa,
            bool acessoGerencial,
            DateTime admissao,
            DateTime? demissao,
            string descCargo,
            char acao
        )
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from h in db.SP_CRIAR_FUNCIONARIO_LINX(
                vendedorCodigo,
                senha,
                codigoFilial,
                apelido,
                nomeVendedor,
                comissao,
                rg,
                endereco,
                complemento,
                cidade,
                cep,
                telefone,
                uf,
                cpf,
                gerente,
                operaCaixa,
                acessoGerencial,
                admissao,
                demissao,
                descCargo,
                acao
           )
                    select h).SingleOrDefault();
        }
        //FIM SP_CRIAR_FUNCIONARIO_LINXResult

        //SP_OBTER_REL_BATIDAResult
        public List<SP_OBTER_REL_BATIDAResult> ObterRelatorioBatida(string codigoFilial, DateTime dataIni, DateTime dataFim, int? codigoFuncionario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_OBTER_REL_BATIDA(codigoFilial, dataIni, dataFim, codigoFuncionario) select h).ToList();
        }
        //FIM SP_OBTER_REL_BATIDAResult

        /*CONTROLE WORKFLOW*/
        //RH_SOL_TIPO
        public List<RH_SOL_TIPO> ObterTipoSolicitacao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TIPOs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public RH_SOL_TIPO ObterTipoSolicitacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TIPOs where m.CODIGO == codigo select m).SingleOrDefault();
        }

        //RH_SOL_TIPO
        public List<RH_SOL_STATUS> ObterStatusSolicitacao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_STATUS
                    orderby m.ORDEM
                    select m).ToList();
        }
        public RH_SOL_STATUS ObterStatusSolicitacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_STATUS where m.CODIGO == codigo select m).SingleOrDefault();
        }

        //RH_SOL_WORKFLOW
        public List<RH_SOL_WORKFLOW> ObterWorkflowSolicitacao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_WORKFLOWs
                    select m).ToList();
        }
        public RH_SOL_WORKFLOW ObterWorkflowSolicitacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_WORKFLOWs
                    where m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public List<RH_SOL_WORKFLOW> ObterWorkflowSolicitacaoHistorico(int codigoWF)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_WORKFLOWs
                    where m.CODIGO == codigoWF || m.CODIGO_PAI == codigoWF
                    orderby m.DATA_INCLUSAO
                    select m).ToList();
        }
        public int InserirWorkflowSolicitacao(RH_SOL_WORKFLOW _wf)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_WORKFLOWs.InsertOnSubmit(_wf);
                db.SubmitChanges();

                return _wf.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int IniciarWorkflowSolicitacao(int status, int tipo, string motivo, int usuario)
        {
            RH_SOL_WORKFLOW wf = new RH_SOL_WORKFLOW();
            wf.CODIGO_PAI = null;
            wf.RH_SOL_STATUS = status;
            wf.RH_SOL_TIPO = tipo;
            wf.MOTIVO = motivo.ToUpper();
            wf.DATA_INCLUSAO = DateTime.Now;
            wf.USUARIO_INCLUSAO = usuario;

            int retorno = InserirWorkflowSolicitacao(wf);

            return retorno;
        }
        public int AtualizarWorkflowSolicitacao(int codigoWF, int status, int tipo, string motivo, int usuario)
        {
            RH_SOL_WORKFLOW wf = new RH_SOL_WORKFLOW();
            wf.CODIGO_PAI = codigoWF;
            wf.RH_SOL_STATUS = status;
            wf.RH_SOL_TIPO = tipo;
            wf.MOTIVO = motivo.ToUpper();
            wf.DATA_INCLUSAO = DateTime.Now;
            wf.USUARIO_INCLUSAO = usuario;

            int retorno = InserirWorkflowSolicitacao(wf);

            return retorno;
        }
        public void ExcluirWorkflowSolicitacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_WORKFLOW _wf = ObterWorkflowSolicitacao(codigo);
                if (_wf != null)
                {
                    db.RH_SOL_WORKFLOWs.DeleteOnSubmit(_wf);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_TRANSF_TEMP
        public List<RH_SOL_TRANSF_TEMP> ObterSolTransfTemp()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TRANSF_TEMPs
                    select m).ToList();
        }
        public RH_SOL_TRANSF_TEMP ObterSolTransfTemp(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TRANSF_TEMPs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_TRANSF_TEMP> ObterSolTransfTemp(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TRANSF_TEMPs
                    where
                        m.CODIGO_FILIAL_ATUAL == filial.Trim()
                    orderby m.RH_FUNCIONARIO1.NOME
                    select m).ToList();
        }
        public int InserirSolTransfTemp(RH_SOL_TRANSF_TEMP _solTransfTemp)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_TRANSF_TEMPs.InsertOnSubmit(_solTransfTemp);
                db.SubmitChanges();

                return _solTransfTemp.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolTransfTemp(RH_SOL_TRANSF_TEMP _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_TRANSF_TEMP _solTransfTemp = ObterSolTransfTemp(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solTransfTemp.CODIGO = _novo.CODIGO;
                _solTransfTemp.CODIGO_FILIAL_ATUAL = _novo.CODIGO_FILIAL_ATUAL;
                _solTransfTemp.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _solTransfTemp.CODIGO_FILIAL_PARA = _novo.CODIGO_FILIAL_PARA;
                _solTransfTemp.DATA_INICIAL = _novo.DATA_INICIAL;
                _solTransfTemp.DATA_FINAL = _novo.DATA_FINAL;
                _solTransfTemp.DESC_CARGO = _novo.DESC_CARGO;
                _solTransfTemp.PERIODO_TRABALHO = _novo.PERIODO_TRABALHO;
                _solTransfTemp.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solTransfTemp.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

                _solTransfTemp.COMISSAO = _novo.COMISSAO;
                _solTransfTemp.OBSERVACAO = _novo.OBSERVACAO;

                _solTransfTemp.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolTransfTemp(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_TRANSF_TEMP _solTransfTemp = ObterSolTransfTemp(codigoWorkFlow);
                if (_solTransfTemp != null)
                {
                    db.RH_SOL_TRANSF_TEMPs.DeleteOnSubmit(_solTransfTemp);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_TRANSFERENCIA
        public List<RH_SOL_TRANSFERENCIA> ObterSolTransferencia()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TRANSFERENCIAs
                    select m).ToList();
        }
        public RH_SOL_TRANSFERENCIA ObterSolTransferencia(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TRANSFERENCIAs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_TRANSFERENCIA> ObterSolTransferencia(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_TRANSFERENCIAs
                    where
                        m.CODIGO_FILIAL_ATUAL == filial.Trim()
                    orderby m.RH_FUNCIONARIO1.NOME
                    select m).ToList();
        }
        public int InserirSolTransferencia(RH_SOL_TRANSFERENCIA _solTransf)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_TRANSFERENCIAs.InsertOnSubmit(_solTransf);
                db.SubmitChanges();

                return _solTransf.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolTransferencia(RH_SOL_TRANSFERENCIA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_TRANSFERENCIA _solTransf = ObterSolTransferencia(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solTransf.CODIGO = _novo.CODIGO;
                _solTransf.CODIGO_FILIAL_REGISTRO = _novo.CODIGO_FILIAL_REGISTRO;
                _solTransf.CODIGO_FILIAL_ATUAL = _novo.CODIGO_FILIAL_ATUAL;
                _solTransf.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _solTransf.CODIGO_FILIAL_PARA = _novo.CODIGO_FILIAL_PARA;
                _solTransf.DATA_INICIAL = _novo.DATA_INICIAL;
                _solTransf.DESC_CARGO = _novo.DESC_CARGO;
                _solTransf.SALARIO = _novo.SALARIO;
                _solTransf.PERIODO_TRABALHO = _novo.PERIODO_TRABALHO;
                _solTransf.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solTransf.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _solTransf.COMISSAO = _novo.COMISSAO;
                _solTransf.OBSERVACAO = _novo.OBSERVACAO;

                _solTransf.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolTransferencia(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_TRANSFERENCIA _solTransf = ObterSolTransferencia(codigoWorkFlow);
                if (_solTransf != null)
                {
                    db.RH_SOL_TRANSFERENCIAs.DeleteOnSubmit(_solTransf);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_ADMISSAO
        public List<RH_SOL_ADMISSAO> ObterSolAdmissao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ADMISSAOs
                    select m).ToList();
        }
        public RH_SOL_ADMISSAO ObterSolAdmissao(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ADMISSAOs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_ADMISSAO> ObterSolAdmissao(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ADMISSAOs
                    where
                        m.CODIGO_FILIAL == filial.Trim()
                    orderby m.NOME_FUNCIONARIO
                    select m).ToList();
        }
        public int InserirSolAdmissao(RH_SOL_ADMISSAO _solAdmissao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_ADMISSAOs.InsertOnSubmit(_solAdmissao);
                db.SubmitChanges();

                return _solAdmissao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolAdmissao(RH_SOL_ADMISSAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_ADMISSAO _solAdmissao = ObterSolAdmissao(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solAdmissao.CODIGO = _novo.CODIGO;
                _solAdmissao.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _solAdmissao.NOME_FUNCIONARIO = _novo.NOME_FUNCIONARIO;
                _solAdmissao.CPF = _novo.CPF;
                _solAdmissao.RG = _novo.RG;
                _solAdmissao.DESC_CARGO = _novo.DESC_CARGO;
                _solAdmissao.SEXO = _novo.SEXO;
                _solAdmissao.DATA_INI_PREVISTO = _novo.DATA_INI_PREVISTO;
                _solAdmissao.TELEFONE = _novo.TELEFONE;
                _solAdmissao.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solAdmissao.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _solAdmissao.OBSERVACAO = _novo.OBSERVACAO;
                _solAdmissao.PERIODO_TRABALHO = _novo.PERIODO_TRABALHO;

                _solAdmissao.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolAdmissao(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_ADMISSAO _solAdmissao = ObterSolAdmissao(codigoWorkFlow);
                if (_solAdmissao != null)
                {
                    db.RH_SOL_ADMISSAOs.DeleteOnSubmit(_solAdmissao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_DEMISSAO
        public List<RH_SOL_DEMISSAO> ObterSolDemissao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_DEMISSAOs
                    select m).ToList();
        }
        public RH_SOL_DEMISSAO ObterSolDemissao(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_DEMISSAOs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_DEMISSAO> ObterSolDemissao(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_DEMISSAOs
                    where
                        m.CODIGO_FILIAL == filial.Trim()
                    orderby m.RH_FUNCIONARIO1.NOME
                    select m).ToList();
        }
        public int InserirSolDemissao(RH_SOL_DEMISSAO _solDemissao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_DEMISSAOs.InsertOnSubmit(_solDemissao);
                db.SubmitChanges();

                return _solDemissao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolDemissao(RH_SOL_DEMISSAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_DEMISSAO _solDemissao = ObterSolDemissao(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solDemissao.CODIGO = _novo.CODIGO;
                _solDemissao.CODIGO_FILIAL_REGISTRO = _novo.CODIGO_FILIAL_REGISTRO;
                _solDemissao.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _solDemissao.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _solDemissao.CPF = _novo.CPF;
                _solDemissao.DATA_PED_DEMISSAO = _novo.DATA_PED_DEMISSAO;
                _solDemissao.DATA_REAL_DEMISSAO = _novo.DATA_REAL_DEMISSAO;
                _solDemissao.FOTO_CARTA_PEDIDO = _novo.FOTO_CARTA_PEDIDO;
                _solDemissao.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solDemissao.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _solDemissao.OBSERVACAO = _novo.OBSERVACAO;

                _solDemissao.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolDemissao(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_DEMISSAO _solDemissao = ObterSolDemissao(codigoWorkFlow);
                if (_solDemissao != null)
                {
                    db.RH_SOL_DEMISSAOs.DeleteOnSubmit(_solDemissao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_FERIAS
        public List<RH_SOL_FERIA> ObterSolFerias()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_FERIAs
                    select m).ToList();
        }
        public RH_SOL_FERIA ObterSolFerias(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_FERIAs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_FERIA> ObterSolFerias(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_FERIAs
                    where
                        m.CODIGO_FILIAL == filial.Trim()
                    orderby m.RH_FUNCIONARIO1.NOME
                    select m).ToList();
        }
        public int InserirSolFerias(RH_SOL_FERIA _solFerias)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_FERIAs.InsertOnSubmit(_solFerias);
                db.SubmitChanges();

                return _solFerias.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolFerias(RH_SOL_FERIA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_FERIA _solFerias = ObterSolFerias(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solFerias.CODIGO = _novo.CODIGO;
                _solFerias.CODIGO_FILIAL_REGISTRO = _novo.CODIGO_FILIAL_REGISTRO;
                _solFerias.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _solFerias.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _solFerias.DATA_INICIO = _novo.DATA_INICIO;
                _solFerias.DIAS_GOZO = _novo.DIAS_GOZO;
                _solFerias.AUTORIZADO_POR = _novo.AUTORIZADO_POR;
                _solFerias.OBSERVACAO = _novo.OBSERVACAO;
                _solFerias.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solFerias.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

                _solFerias.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolFerias(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_FERIA _solFerias = ObterSolFerias(codigoWorkFlow);
                if (_solFerias != null)
                {
                    db.RH_SOL_FERIAs.DeleteOnSubmit(_solFerias);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_ALTER_CSP
        public List<RH_SOL_ALTER_CSP> ObterSolAlteracaoCSP()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ALTER_CSPs
                    select m).ToList();
        }
        public RH_SOL_ALTER_CSP ObterSolAlteracaoCSP(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ALTER_CSPs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_ALTER_CSP> ObterSolAlteracaoCSP(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ALTER_CSPs
                    where
                        m.CODIGO_FILIAL == filial.Trim()
                    orderby m.RH_FUNCIONARIO1.NOME
                    select m).ToList();
        }
        public int InserirSolAlteracaoCSP(RH_SOL_ALTER_CSP _solAlterCSP)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_ALTER_CSPs.InsertOnSubmit(_solAlterCSP);
                db.SubmitChanges();

                return _solAlterCSP.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolAlteracaoCSP(RH_SOL_ALTER_CSP _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_ALTER_CSP _solAlterCSP = ObterSolAlteracaoCSP(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solAlterCSP.CODIGO = _novo.CODIGO;
                _solAlterCSP.CODIGO_FILIAL_REGISTRO = _novo.CODIGO_FILIAL_REGISTRO;
                _solAlterCSP.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _solAlterCSP.RH_FUNCIONARIO = _novo.RH_FUNCIONARIO;
                _solAlterCSP.DATA_INICIAL = _novo.DATA_INICIAL;
                _solAlterCSP.DESC_CARGO = _novo.DESC_CARGO;
                _solAlterCSP.SALARIO = _novo.SALARIO;
                _solAlterCSP.PERIODO_TRABALHO = _novo.PERIODO_TRABALHO;
                _solAlterCSP.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solAlterCSP.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

                _solAlterCSP.OBSERVACAO = _novo.OBSERVACAO;
                _solAlterCSP.COMISSAO = _novo.COMISSAO;

                _solAlterCSP.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolAlteracaoCSP(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_ALTER_CSP _solAlterCSP = ObterSolAlteracaoCSP(codigoWorkFlow);
                if (_solAlterCSP != null)
                {
                    db.RH_SOL_ALTER_CSPs.DeleteOnSubmit(_solAlterCSP);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //RH_SOL_ALTER_VENDA
        public List<RH_SOL_ALTER_VENDA> ObterSolAlteracaoVenda()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ALTER_VENDAs
                    select m).ToList();
        }
        public RH_SOL_ALTER_VENDA ObterSolAlteracaoVenda(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ALTER_VENDAs where m.RH_SOL_WORKFLOW == codigoWorkFlow select m).SingleOrDefault();
        }
        public List<RH_SOL_ALTER_VENDA> ObterSolAlteracaoVenda(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.RH_SOL_ALTER_VENDAs
                    where
                        m.CODIGO_FILIAL == filial.Trim()
                    orderby m.RH_FUNCIONARIO1.NOME
                    select m).ToList();
        }
        public int InserirSolAlteracaoVenda(RH_SOL_ALTER_VENDA _solAlterVenda)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.RH_SOL_ALTER_VENDAs.InsertOnSubmit(_solAlterVenda);
                db.SubmitChanges();

                return _solAlterVenda.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarSolAlteracaoVenda(RH_SOL_ALTER_VENDA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            RH_SOL_ALTER_VENDA _solAlterVenda = ObterSolAlteracaoVenda(_novo.RH_SOL_WORKFLOW);

            if (_novo != null)
            {
                _solAlterVenda.CODIGO = _novo.CODIGO;
                _solAlterVenda.CODIGO_FILIAL = _novo.CODIGO_FILIAL;
                _solAlterVenda.RH_FUNCIONARIO_DE = _novo.RH_FUNCIONARIO_DE;
                _solAlterVenda.RH_FUNCIONARIO_PARA = _novo.RH_FUNCIONARIO_PARA;
                _solAlterVenda.TICKET = _novo.TICKET;
                _solAlterVenda.DATA_TICKET = _novo.DATA_TICKET;
                _solAlterVenda.TERMINAL = _novo.TERMINAL;
                _solAlterVenda.VALOR_VENDA = _novo.VALOR_VENDA;

                _solAlterVenda.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _solAlterVenda.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

                _solAlterVenda.RH_SOL_WORKFLOW = _novo.RH_SOL_WORKFLOW;

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
        public void ExcluirSolAlteracaoVenda(int codigoWorkFlow)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                RH_SOL_ALTER_VENDA _solAlterVenda = ObterSolAlteracaoVenda(codigoWorkFlow);
                if (_solAlterVenda != null)
                {
                    db.RH_SOL_ALTER_VENDAs.DeleteOnSubmit(_solAlterVenda);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //SP_OBTER_SOL_PAINELResult
        public List<SP_OBTER_SOL_PAINELResult> ObterPainelSolicitacao(string codigoFilial, int? codigoTipoSolicitacao, int? codigoStatus)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_OBTER_SOL_PAINEL(codigoFilial, codigoTipoSolicitacao, codigoStatus) select h).ToList();
        }
        //FIM SP_OBTER_SOL_PAINELResult

        public List<SP_OBTER_NAO_CONFORMIDADES_RHResult> ObterNaoConformidadesRH(string codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_OBTER_NAO_CONFORMIDADES_RH(codigoFilial) select h).ToList();
        }

        public List<PROPAY_DRE_FOLHA> ObterProPayFolhaDREPorCompetencia(DateTime competencia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROPAY_DRE_FOLHAs
                    where
                        m.COMPETENCIA == competencia
                    select m).ToList();
        }
        public void InserirProPayFolhaDRE(PROPAY_DRE_FOLHA propay)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.PROPAY_DRE_FOLHAs.InsertOnSubmit(propay);
                dbIntra.SubmitChanges();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirProPayFolhaDRE(DateTime competencia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var propayDre = ObterProPayFolhaDREPorCompetencia(competencia);
                foreach (var pp in propayDre)
                    dbIntra.PROPAY_DRE_FOLHAs.DeleteOnSubmit(pp);

                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RH_FOLHA_LOJA> ObterFolhaLojaPorCompetencia(DateTime competencia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.RH_FOLHA_LOJAs
                    where
                        m.COMPETENCIA == competencia
                    select m).ToList();
        }
        public RH_FOLHA_LOJA ObterFolhaLoja(DateTime competencia, string cpf)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.RH_FOLHA_LOJAs
                    where
                        m.COMPETENCIA == competencia
                        && m.CPF == cpf
                    select m).SingleOrDefault();
        }
        public void InserirFolhaLoja(RH_FOLHA_LOJA folhaLoja)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.RH_FOLHA_LOJAs.InsertOnSubmit(folhaLoja);
                dbIntra.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFolhaLoja(RH_FOLHA_LOJA _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            RH_FOLHA_LOJA folhaLoja = ObterFolhaLoja(_novo.COMPETENCIA, _novo.CPF);

            if (_novo != null)
            {
                folhaLoja.COMPETENCIA = _novo.COMPETENCIA;
                folhaLoja.MATRICULA = _novo.MATRICULA;
                folhaLoja.CPF = _novo.CPF;
                folhaLoja.NOME = _novo.NOME;
                folhaLoja.CARGO = _novo.CARGO;
                folhaLoja.LOJA = _novo.LOJA;
                folhaLoja.SAL_MINIMO_GARA = _novo.SAL_MINIMO_GARA;
                folhaLoja.COMISSAO = _novo.COMISSAO;
                folhaLoja.PREMIO_PONTA = _novo.PREMIO_PONTA;
                folhaLoja.PREMIO_COTA = _novo.PREMIO_COTA;
                folhaLoja.PREMIO_CONVCOLET = _novo.PREMIO_CONVCOLET;
                folhaLoja.SUPERVISOR = _novo.SUPERVISOR;
                folhaLoja.GERENTE = _novo.GERENTE;
                folhaLoja.EMPRESA = _novo.EMPRESA;
                folhaLoja.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                folhaLoja.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                folhaLoja.DATA_CALCULO = _novo.DATA_CALCULO;
                folhaLoja.USUARIO_CALCULO = _novo.USUARIO_CALCULO;

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
        public void ExcluirFolhaLoja(DateTime competencia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                var folhaLoja = ObterFolhaLojaPorCompetencia(competencia);
                foreach (var ff in folhaLoja)
                    dbIntra.RH_FOLHA_LOJAs.DeleteOnSubmit(ff);

                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SP_GERAR_FOLHA_FUNCIONARIO_HISTResult GerarFolhaFuncionarioHist(string cpf, DateTime competencia)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from h in dbIntra.SP_GERAR_FOLHA_FUNCIONARIO_HIST(competencia, cpf) select h).SingleOrDefault();
        }

        public SP_GERAR_FOLHAResult GerarFolha(DateTime competencia, int codigoUsuario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            dbIntra.CommandTimeout = 0;
            return (from h in dbIntra.SP_GERAR_FOLHA(competencia, codigoUsuario) select h).SingleOrDefault();
        }
        public List<SP_OBTER_CALCULO_COMISSAOResult> ObterCalculoComissao(DateTime competencia, string codigoFilial, string cpf, string vendedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from h in dbIntra.SP_OBTER_CALCULO_COMISSAO(competencia, codigoFilial, cpf, vendedor) select h).ToList();
        }

        public List<SP_OBTER_VALOR_VENDA_VENDEDORResult> ObterValorVendaVendedor(DateTime dataIni, DateTime dataFim, string codigoFilial, string cpf, string vendedor)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from h in dbIntra.SP_OBTER_VALOR_VENDA_VENDEDOR(dataIni, dataFim, codigoFilial, cpf, vendedor) select h).ToList();
        }

        public List<SP_OBTER_RH_SEMANAResult> ObterSemanaRH2120(string semana)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from h in dbIntra.SP_OBTER_RH_SEMANA(semana) select h).ToList();
        }
        public List<SP_OBTER_TRAB_DOMFERResult> ObterTrabDomFer(string semana, int codigoSuper)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from h in dbIntra.SP_OBTER_TRAB_DOMFER(semana, codigoSuper) select h).ToList();
        }


        public List<LOJA_TRAB_DOMFER> ObterTrabDomFer()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_TRAB_DOMFERs
                    select m).ToList();
        }
        public LOJA_TRAB_DOMFER ObterTrabDomFer(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.LOJA_TRAB_DOMFERs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirTrabDomFer(LOJA_TRAB_DOMFER trabDomFer)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.LOJA_TRAB_DOMFERs.InsertOnSubmit(trabDomFer);
                dbIntra.SubmitChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTrabDomFer(LOJA_TRAB_DOMFER _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            LOJA_TRAB_DOMFER trabDomFer = ObterTrabDomFer(_novo.CODIGO);

            if (_novo != null)
            {
                trabDomFer.TOT_DOMINGO = _novo.TOT_DOMINGO;
                trabDomFer.TOT_FERIADO = _novo.TOT_FERIADO;
                trabDomFer.DATA_ATUALIZACAO = _novo.DATA_ATUALIZACAO;
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
