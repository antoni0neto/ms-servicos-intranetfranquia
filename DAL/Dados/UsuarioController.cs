using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class UsuarioController
    {
        BaseController baseController = new BaseController();

        DCDataContext db;

        public USUARIO ValidaUsuario(string usuario, string senha)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from u in db.USUARIOs where u.USUARIO1 == usuario && u.SENHA == senha && u.DATA_EXCLUSAO == null select u).Take(1).SingleOrDefault();
        }

        public USUARIO ValidaUsuarioSuper(string usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from u in db.USUARIOs where u.USUARIO1 == usuario && u.CODIGO_PERFIL == 3 select u).SingleOrDefault();
        }

        public List<USUARIO> BuscaUsuarios()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from u in db.USUARIOs orderby u.NOME_USUARIO, u.USUARIO1 select u).ToList();
        }


        public List<DEPOSITO_DOCUMENTO> BuscaDocumentos(int codigoDeposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dd in db.DEPOSITO_DOCUMENTOs where dd.CODIGO_DEPOSITO == codigoDeposito select dd).ToList();
        }

        public List<DEPOSITO_DOCUMENTO> BuscaDocumentosParaLancamento(int codigoDeposito)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dd in db.DEPOSITO_DOCUMENTOs where dd.CODIGO_DEPOSITO == codigoDeposito && dd.NUMERO_LANCAMENTO == null select dd).ToList();
        }

        public TIPO_LANCAMENTO BuscarTipoLancamento(string tipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dd in db.TIPO_LANCAMENTOs where dd.TIPO == tipo select dd).SingleOrDefault();
        }
        public TIPO_LANCAMENTO BuscarTipoLancamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dd in db.TIPO_LANCAMENTOs where dd.CODIGO == codigo select dd).SingleOrDefault();
        }

        public TELEFONIA_TIPO_DESTINO BuscaTipoDestino(int tipoDestino)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from td in db.TELEFONIA_TIPO_DESTINOs where td.CODIGO == tipoDestino select td).SingleOrDefault();
        }

        public TELEFONIA_TIPO_TELEFONE BuscaTipoTelefone(int tipoTelefone)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from tt in db.TELEFONIA_TIPO_TELEFONEs where tt.CODIGO == tipoTelefone select tt).SingleOrDefault();
        }

        public List<TELEFONIA_CONTROLE_CHAMADA> BuscaChamadas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.TELEFONIA_CONTROLE_CHAMADAs select c).ToList();
        }

        public List<DEPOSITO_COMPROVANTE> BuscaComprovantes()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.DEPOSITO_COMPROVANTEs where c.CONFERIDO == false select c).ToList();
        }

        public List<DEPOSITO_COMPROVANTE> BuscaComprovantesPendentes(int filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.DEPOSITO_COMPROVANTEs where c.CODIGO_FILIAL == filial & c.DATA_FIM >= dataInicio & c.DATA_INICIO <= dataFim & c.CONFERIDO == false select c).ToList();
        }

        public List<DEPOSITO_COMPROVANTE> BuscaComprovantesRealizados(int filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.DEPOSITO_COMPROVANTEs where c.CODIGO_FILIAL == filial & c.DATA_FIM >= dataInicio & c.DATA_INICIO <= dataFim & c.CONFERIDO == true select c).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositosPendentes(int filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from d in db.DEPOSITO_HISTORICOs where d.CODIGO_FILIAL == filial & d.DATA >= dataInicio & d.DATA <= dataFim & d.CONFERIDO == false select d).ToList();
        }

        public List<DEPOSITO_HISTORICO> BuscaDepositosRealizados(int filial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from d in db.DEPOSITO_HISTORICOs where d.CODIGO_FILIAL == filial & d.DATA >= dataInicio & d.DATA <= dataFim & d.CONFERIDO == true select d).ToList();
        }

        public List<TELEFONIA_TIPO_TELEFONE> BuscaTiposTelefone()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from tt in db.TELEFONIA_TIPO_TELEFONEs select tt).ToList();
        }

        public List<TELEFONIA_TIPO_DESTINO> BuscaTiposDestino()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from td in db.TELEFONIA_TIPO_DESTINOs select td).ToList();
        }

        public List<IMPORTACAO_COLECAO> BuscaColecoes()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.IMPORTACAO_COLECAOs select c).ToList();
        }

        public List<IMPORTACAO_PROFORMA> BuscaProformas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.IMPORTACAO_PROFORMAs select p).ToList();
        }

        public List<IMPORTACAO_NEL> BuscaNels()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.IMPORTACAO_NELs select n).ToList();
        }

        public List<IMPORTACAO_NEL> BuscaNels(int colecao, int janela)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.IMPORTACAO_NELs where n.CODIGO_COLECAO == colecao & n.CODIGO_JANELA == janela select n).ToList();
        }

        public List<IMPORTACAO_CONTAINER> BuscaContainers()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.IMPORTACAO_CONTAINERs select c).ToList();
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

        public NOTA_RETIRADA BuscaFilialNotaRetirada(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nr in db.NOTA_RETIRADAs where nr.CODIGO_FILIAL == filial & nr.FLAG_ARQUIVO == null select nr).Take(1).SingleOrDefault();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaFilialNotaRetiradaItem(int filial)
        {
            NOTA_RETIRADA notaRetirada = BuscaFilialNotaRetirada(filial);

            if (notaRetirada != null)
            {
                db = new DCDataContext(Constante.ConnectionStringIntranet);

                return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == notaRetirada.CODIGO_NOTA_RETIRADA select nri).ToList();
            }
            else
                return null;
        }

        public List<NOTA_RETIRADA_ITEM> BuscaProdutoDefeitoSegundaQualidade(string codigoProduto, string cor, string tamanho)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs
                    where nri.CODIGO_PRODUTO == Convert.ToInt32(codigoProduto) &
                          nri.COR_PRODUTO.Trim().Equals(cor.Trim()) &
                          nri.TAMANHO.Trim().Equals(tamanho.Trim()) &
                          nri.CODIGO_DESTINO.Equals("2") &
                          nri.DATA_DESTINO == null
                    select nri).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaPorFilial(int codigoNotaRetirada, string filialOrigem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada && nri.FILIAL_ORIGEM.Trim() == filialOrigem.Trim() select nri).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaCmax(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada & nri.TIPO_NOTA.Equals("Cmax") select nri).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaHbf(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada & nri.TIPO_NOTA.Equals("Hbf") select nri).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaCalcados(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada & nri.TIPO_NOTA.Equals("Calcados") select nri).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaOutros(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada & nri.TIPO_NOTA.Equals("outros") select nri).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaLugzi(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada & nri.TIPO_NOTA.Equals("Lugzi") select nri).ToList();
        }

        public int BuscaUltimaNotaRetirada(int codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nr in db.NOTA_RETIRADAs where nr.CODIGO_FILIAL == codigoFilial select nr.CODIGO_NOTA_RETIRADA).Max();
        }

        public int BuscaUltimaNotaTransferencia(int codigoFilial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nr in db.NOTA_TRANSFERENCIAs where nr.CODIGO_FILIAL == codigoFilial select nr.CODIGO).Max();
        }

        public NOTA_RETIRADA BuscaNotaRetiradaEmAberto(int filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nr in db.NOTA_RETIRADAs where nr.CODIGO_FILIAL == filial & nr.FLAG_ARQUIVO == null select nr).Take(1).SingleOrDefault();
        }

        public NOTA_RETIRADA_ITEM BuscaNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA_ITEM == codigoNotaRetiradaItem select nri).SingleOrDefault();
        }

        public NOTA_RETIRADA BuscaNotaRetirada(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nr in db.NOTA_RETIRADAs where nr.CODIGO_NOTA_RETIRADA == codigoNotaRetirada select nr).SingleOrDefault();
        }

        public NOTA_TRANSFERENCIA BuscaNotaTransferencia(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nt in db.NOTA_TRANSFERENCIAs where nt.CODIGO == codigoNotaTransferencia select nt).SingleOrDefault();
        }

        public List<NOTA_TRANSFERENCIA_ITEM> BuscaNotaTransferenciaItem(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nt in db.NOTA_TRANSFERENCIA_ITEMs where nt.CODIGO_NOTA_TRANSFER == codigoNotaTransferencia select nt).ToList();
        }

        public List<NOTA_RETIRADA_ITEM> BuscaNotaRetiradaItens(int codigoNotaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA == codigoNotaRetirada select nri).ToList();
        }

        public List<FECHAMENTO_CAIXA> BuscaCaixas()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.FECHAMENTO_CAIXAs select c).ToList();
        }

        public USUARIO BuscaPorCodigoUsuario(int CODIGO_FUNCIONARIO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from u in db.USUARIOs where u.CODIGO_USUARIO == CODIGO_FUNCIONARIO select u).SingleOrDefault();
        }

        public DEPOSITO_DOCUMENTO BuscaDepositoDocumento(int codigoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dd in db.DEPOSITO_DOCUMENTOs where dd.CODIGO_DOCUMENTO == codigoDocumento select dd).SingleOrDefault();
        }

        public TELEFONIA_CONTROLE_CHAMADA BuscaPorCodigoChamada(int CODIGO_CHAMADA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from cct in db.TELEFONIA_CONTROLE_CHAMADAs where cct.CODIGO == CODIGO_CHAMADA select cct).SingleOrDefault();
        }

        public DEPOSITO_COMPROVANTE BuscaPorCodigoComprovante(int CODIGO_COMPROVANTE)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dc in db.DEPOSITO_COMPROVANTEs where dc.CODIGO == CODIGO_COMPROVANTE select dc).SingleOrDefault();
        }

        public List<DEPOSITO_COMPROVANTE> BuscaDepositosComprovantes(int codigoFilial, DateTime dataInicio, DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from dc in db.DEPOSITO_COMPROVANTEs where dc.CODIGO_FILIAL == codigoFilial & dc.DATA_INICIO.Date == dataInicio.Date & dc.DATA_FIM.Date == dataFim.Date select dc).ToList();
        }

        public IMPORTACAO_COLECAO BuscaPorCodigoColecao(int CODIGO_COLECAO)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.IMPORTACAO_COLECAOs where c.CODIGO_COLECAO == CODIGO_COLECAO select c).SingleOrDefault();
        }

        public IMPORTACAO_PROFORMA BuscaPorCodigoProforma(int CODIGO_PROFORMA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.IMPORTACAO_PROFORMAs where p.CODIGO_PROFORMA == CODIGO_PROFORMA select p).SingleOrDefault();
        }

        public IMPORTACAO_NEL BuscaPorCodigoNel(int CODIGO_NEL)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.IMPORTACAO_NELs where n.CODIGO_NEL == CODIGO_NEL select n).SingleOrDefault();
        }

        public IMPORTACAO_CONTAINER BuscaPorCodigoContainer(int CODIGO_CONTAINER)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.IMPORTACAO_CONTAINERs where c.CODIGO_CONTAINER == CODIGO_CONTAINER select c).SingleOrDefault();
        }

        public IMPORTACAO_PACK_GRADE BuscaPorCodigoPackGrade(int codigoPackGrade)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from pg in db.IMPORTACAO_PACK_GRADEs where pg.CODIGO_PACK_GRADE == codigoPackGrade select pg).SingleOrDefault();
        }

        public IMPORTACAO_PACK_GROUP BuscaPorCodigoPackGroup(int codigoPackGroup)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from pg in db.IMPORTACAO_PACK_GROUPs where pg.CODIGO_PACK_GROUP == codigoPackGroup select pg).SingleOrDefault();
        }

        public NOTA_RETIRADA_ITEM BuscaPorCodigoNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nri in db.NOTA_RETIRADA_ITEMs where nri.CODIGO_NOTA_RETIRADA_ITEM == codigoNotaRetiradaItem select nri).SingleOrDefault();
        }

        public NOTA_TRANSFERENCIA_ITEM BuscaPorCodigoNotaTransferenciaItem(int codigoNotaTransferenciaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from nti in db.NOTA_TRANSFERENCIA_ITEMs where nti.CODIGO == codigoNotaTransferenciaItem select nti).SingleOrDefault();
        }

        public List<FECHAMENTO_ITEM_CAIXA> BuscaPorCodigoCaixa(int CODIGO_CAIXA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from it in db.FECHAMENTO_ITEM_CAIXAs where it.CODIGO_CAIXA == CODIGO_CAIXA select it).ToList();
        }

        public FECHAMENTO_CAIXA BuscaCaixaPorCodigo(int CODIGO_CAIXA)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from c in db.FECHAMENTO_CAIXAs where c.CODIGO_CAIXA == CODIGO_CAIXA select c).SingleOrDefault();
        }

        public List<USUARIO> BuscaSupervisores()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from u in db.USUARIOs where u.CODIGO_PERFIL == 2 select u).ToList();
        }

        public USUARIO BuscaPorUsuario(string usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from u in db.USUARIOs where u.USUARIO1 == usuario select u).SingleOrDefault();
        }

        public IMPORTACAO_COLECAO BuscaColecaoPelaDescricao(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.IMPORTACAO_COLECAOs where c.DESCRICAO.Equals(descricao) select c).SingleOrDefault();
        }

        public IMPORTACAO_PROFORMA BuscaProformaPelaDescricao(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from p in db.IMPORTACAO_PROFORMAs where p.DESCRICAO_PROFORMA.Equals(descricao) select p).SingleOrDefault();
        }

        public IMPORTACAO_NEL BuscaNelPelaDescricao(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from n in db.IMPORTACAO_NELs where n.DESCRICAO_NEL.Equals(descricao) select n).SingleOrDefault();
        }

        public IMPORTACAO_CONTAINER BuscaContainerPelaDescricao(string descricao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.IMPORTACAO_CONTAINERs where c.DESCRICAO.Equals(descricao) select c).SingleOrDefault();
        }

        public IMPORTACAO_PACK_GRADE BuscaPackGrade(string codigoGrupo, string descricaoTipo, int codigoGriffe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from pg in db.IMPORTACAO_PACK_GRADEs where pg.CODIGO_GRUPO.Equals(codigoGrupo) & pg.DESCRICAO_TIPO.Equals(descricaoTipo) & pg.CODIGO_GRIFFE == codigoGriffe select pg).SingleOrDefault();
        }

        public IMPORTACAO_PACK_GROUP BuscaPackGroup(string codigoGrupo, int codigoGriffe, string tipo, int qtdeTotal)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from pg in db.IMPORTACAO_PACK_GROUPs
                    where pg.CODIGO_GRUPO.Equals(codigoGrupo) &
                          pg.CODIGO_GRIFFE == codigoGriffe &
                          pg.DESCRICAO_TIPO.Equals(tipo) &
                          pg.QTDE_TOTAL == qtdeTotal
                    select pg).SingleOrDefault();
        }

        public void InsereCaixa(FECHAMENTO_CAIXA caixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FECHAMENTO_CAIXAs.InsertOnSubmit(caixa);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereItemCaixa(FECHAMENTO_ITEM_CAIXA itemCaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FECHAMENTO_ITEM_CAIXAs.InsertOnSubmit(itemCaixa);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereHistoricoReceita(FECHAMENTO_CAIXA caixa)
        {
            FUNDO_FIXO_CONTA_RECEITA contaReceita = baseController.BuscaContaReceita(caixa.CODIGO_FILIAL);

            if (contaReceita != null)
            {
                FUNDO_FIXO_HISTORICO_RECEITA fundoReceita = new FUNDO_FIXO_HISTORICO_RECEITA();

                fundoReceita.CODIGO_CONTA_RECEITA = contaReceita.CODIGO;
                fundoReceita.CODIGO_FILIAL = caixa.CODIGO_FILIAL;
                fundoReceita.DATA = DateTime.Now;
                fundoReceita.VALOR = Convert.ToDecimal(caixa.VALOR_RETIRADA);

                db = new DCDataContext(Constante.ConnectionStringIntranet);

                try
                {
                    db.FUNDO_FIXO_HISTORICO_RECEITAs.InsertOnSubmit(fundoReceita);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void InsereHistoricoDeposito(FECHAMENTO_CAIXA caixa)
        {
            decimal valorDeposito = Convert.ToDecimal(caixa.VALOR_DINHEIRO - (caixa.VALOR_RETIRADA + caixa.VALOR_DEVOLUCAO));

            if (valorDeposito > 0)
            {
                DEPOSITO_HISTORICO fundoDeposito = new DEPOSITO_HISTORICO();

                fundoDeposito.CODIGO_FILIAL = caixa.CODIGO_FILIAL;
                fundoDeposito.DATA = caixa.DATA_FECHAMENTO;
                fundoDeposito.VALOR_DEPOSITO = valorDeposito;

                db = new DCDataContext(Constante.ConnectionStringIntranet);

                try
                {
                    db.DEPOSITO_HISTORICOs.InsertOnSubmit(fundoDeposito);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void InsereHistoricoDespesas(List<FECHAMENTO_ITEM_CAIXA> listaItemCaixa, int codigoFilial, DateTime dataFechamento)
        {
            foreach (FECHAMENTO_ITEM_CAIXA itemCaixa in listaItemCaixa)
            {
                FUNDO_FIXO_HISTORICO_DESPESA fundoDespesa = new FUNDO_FIXO_HISTORICO_DESPESA();

                fundoDespesa.CODIGO_CONTA_DESPESA = itemCaixa.CODIGO_CONTA.ToString();
                fundoDespesa.CODIGO_FILIAL = codigoFilial;
                fundoDespesa.DATA = dataFechamento;
                fundoDespesa.VALOR = Convert.ToDecimal(itemCaixa.VALOR);

                db = new DCDataContext(Constante.ConnectionStringIntranet);

                try
                {
                    db.FUNDO_FIXO_HISTORICO_DESPESAs.InsertOnSubmit(fundoDespesa);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void InsereHistoricoFechamento(FUNDO_FIXO_HISTORICO_FECHAMENTO historicoFechamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FUNDO_FIXO_HISTORICO_FECHAMENTOs.InsertOnSubmit(historicoFechamento);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereDocumento(DEPOSITO_DOCUMENTO depositoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.DEPOSITO_DOCUMENTOs.InsertOnSubmit(depositoDocumento);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insere(USUARIO usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.USUARIOs.InsertOnSubmit(usuario);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Atualiza(USUARIO userNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            USUARIO usuario = BuscaPorCodigoUsuario(userNew.CODIGO_USUARIO);

            if (usuario != null)
            {
                usuario.NOME_USUARIO = userNew.NOME_USUARIO;
                usuario.CODIGO_PERFIL = userNew.CODIGO_PERFIL;
                usuario.USUARIO1 = userNew.USUARIO1;
                usuario.SENHA = userNew.SENHA;
                usuario.EMAIL = userNew.EMAIL;
                usuario.CPF = userNew.CPF;
                usuario.REPRESENTANTE_CLIFOR = userNew.REPRESENTANTE_CLIFOR;
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

        public void AtualizaChamada(TELEFONIA_CONTROLE_CHAMADA controleChamadaNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            TELEFONIA_CONTROLE_CHAMADA controleChamada = BuscaPorCodigoChamada(controleChamadaNew.CODIGO);

            if (controleChamada != null)
            {
                controleChamada.CODIGO_USUARIO_LIGACAO = controleChamadaNew.CODIGO_USUARIO_LIGACAO;
                controleChamada.DATA_OCORRENCIA = controleChamadaNew.DATA_OCORRENCIA;
                controleChamada.DDD = controleChamadaNew.DDD;
                controleChamada.TELEFONE = controleChamadaNew.TELEFONE;
                controleChamada.DESTINO = controleChamadaNew.DESTINO;
                controleChamada.CODIGO_TIPO_TELEFONE = controleChamadaNew.CODIGO_TIPO_TELEFONE;
                controleChamada.CODIGO_TIPO_DESTINO = controleChamadaNew.CODIGO_TIPO_DESTINO;
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

        public void AtualizaComprovante(DEPOSITO_COMPROVANTE depositoComprovanteNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DEPOSITO_COMPROVANTE depositoComprovante = BuscaPorCodigoComprovante(depositoComprovanteNew.CODIGO);

            if (depositoComprovante != null)
            {
                depositoComprovante.CODIGO_FILIAL = depositoComprovanteNew.CODIGO_FILIAL;
                depositoComprovante.DATA_INICIO = depositoComprovanteNew.DATA_INICIO;
                depositoComprovante.DATA_FIM = depositoComprovanteNew.DATA_FIM;
                depositoComprovante.VALOR = depositoComprovanteNew.VALOR;
                depositoComprovante.OBS = depositoComprovanteNew.OBS;
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

        public void InsereHistoricoDespesas(FUNDO_FIXO_HISTORICO_DESPESA historicoDespesas)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.FUNDO_FIXO_HISTORICO_DESPESAs.InsertOnSubmit(historicoDespesas);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaHistoricoFechamento(FUNDO_FIXO_HISTORICO_FECHAMENTO _historicoFechamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FUNDO_FIXO_HISTORICO_FECHAMENTO historicoFechamento = (from hd in db.FUNDO_FIXO_HISTORICO_FECHAMENTOs where hd.CODIGO == _historicoFechamento.CODIGO select hd).SingleOrDefault();

            if (historicoFechamento != null)
            {
                historicoFechamento.CODIGO_BAIXA = _historicoFechamento.CODIGO_BAIXA;
                historicoFechamento.DATA_CONF = _historicoFechamento.DATA_CONF;
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

        public void AtualizaHistoricoDespesas(FUNDO_FIXO_HISTORICO_DESPESA historicoDespesasNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            //FUNDO_FIXO_HISTORICO_DESPESA historicoDespesas = baseController.BuscaDespesaPorCodigo(historicoDespesasNew.CODIGO);
            FUNDO_FIXO_HISTORICO_DESPESA historicoDespesas = (from hd in db.FUNDO_FIXO_HISTORICO_DESPESAs where hd.CODIGO == historicoDespesasNew.CODIGO select hd).SingleOrDefault();

            if (historicoDespesas != null)
            {
                //historicoDespesas.CODIGO = historicoDespesasNew.CODIGO;
                historicoDespesas.CODIGO_CONTA_DESPESA = historicoDespesasNew.CODIGO_CONTA_DESPESA;
                historicoDespesas.CODIGO_FECHAMENTO = historicoDespesasNew.CODIGO_FECHAMENTO;
                historicoDespesas.CODIGO_FILIAL = historicoDespesasNew.CODIGO_FILIAL;
                historicoDespesas.DATA = historicoDespesasNew.DATA;
                historicoDespesas.VALOR = historicoDespesasNew.VALOR;
                historicoDespesas.CODIGO_DESPESA_LOJA = historicoDespesasNew.CODIGO_DESPESA_LOJA;
                historicoDespesas.NUMERO_LANCAMENTO = historicoDespesasNew.NUMERO_LANCAMENTO;
                historicoDespesas.DESC_DESPESA = historicoDespesasNew.DESC_DESPESA;
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



        public void AtualizaHistoricoReceitas(FUNDO_FIXO_HISTORICO_RECEITA historicoReceitasNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FUNDO_FIXO_HISTORICO_RECEITA historicoReceitas = (from hd in db.FUNDO_FIXO_HISTORICO_RECEITAs where hd.CODIGO == historicoReceitasNew.CODIGO select hd).SingleOrDefault();

            if (historicoReceitas != null)
            {
                historicoReceitas.CODIGO_CONTA_RECEITA = historicoReceitasNew.CODIGO_CONTA_RECEITA;
                historicoReceitas.CODIGO_FECHAMENTO = historicoReceitasNew.CODIGO_FECHAMENTO;
                historicoReceitas.CODIGO_FILIAL = historicoReceitasNew.CODIGO_FILIAL;
                historicoReceitas.DATA = historicoReceitasNew.DATA;
                historicoReceitas.VALOR = historicoReceitasNew.VALOR;
                historicoReceitas.NUMERO_LANCAMENTO = historicoReceitasNew.NUMERO_LANCAMENTO;
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

        public void ReabreHistoricoReceitas(int codigoFechamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            List<FUNDO_FIXO_HISTORICO_RECEITA> historicoReceitas = (from hd in db.FUNDO_FIXO_HISTORICO_RECEITAs where hd.CODIGO_FECHAMENTO == codigoFechamento select hd).ToList();

            if (historicoReceitas != null)
            {
                foreach (FUNDO_FIXO_HISTORICO_RECEITA historicoReceita in historicoReceitas)
                {
                    historicoReceita.CODIGO_FECHAMENTO = null;

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

        public List<FUNDO_FIXO_HISTORICO_RECEITA> BuscaHistoricoReceitas(int codigoFechamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from hd in db.FUNDO_FIXO_HISTORICO_RECEITAs where hd.CODIGO_FECHAMENTO == codigoFechamento select hd).ToList();
        }

        public List<FUNDO_FIXO_HISTORICO_RECEITA> BuscaHistoricoReceitasLancados(int codigoFechamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from hd in db.FUNDO_FIXO_HISTORICO_RECEITAs where hd.CODIGO_FECHAMENTO == codigoFechamento select hd).ToList();
        }

        public void InsereColecao(IMPORTACAO_COLECAO colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMPORTACAO_COLECAOs.InsertOnSubmit(colecao);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereChamada(TELEFONIA_CONTROLE_CHAMADA controleChamada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.TELEFONIA_CONTROLE_CHAMADAs.InsertOnSubmit(controleChamada);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereComprovante(DEPOSITO_COMPROVANTE depositoComprovante)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.DEPOSITO_COMPROVANTEs.InsertOnSubmit(depositoComprovante);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereProforma(IMPORTACAO_PROFORMA proforma)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMPORTACAO_PROFORMAs.InsertOnSubmit(proforma);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereNel(IMPORTACAO_NEL nel)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMPORTACAO_NELs.InsertOnSubmit(nel);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereContainer(IMPORTACAO_CONTAINER container)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMPORTACAO_CONTAINERs.InsertOnSubmit(container);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserePackGrade(IMPORTACAO_PACK_GRADE packGrade)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMPORTACAO_PACK_GRADEs.InsertOnSubmit(packGrade);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InserePackGroup(IMPORTACAO_PACK_GROUP packGroup)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.IMPORTACAO_PACK_GROUPs.InsertOnSubmit(packGroup);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaColecao(IMPORTACAO_COLECAO colecaoNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_COLECAO colecao = BuscaPorCodigoColecao(colecaoNew.CODIGO_COLECAO);

            if (colecao != null)
            {
                colecao.CODIGO_COLECAO = colecaoNew.CODIGO_COLECAO;
                colecao.DESCRICAO = colecaoNew.DESCRICAO;
                colecao.TAXA = colecaoNew.TAXA;
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

        public void AtualizaProforma(IMPORTACAO_PROFORMA proformaNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_PROFORMA proforma = BuscaPorCodigoProforma(proformaNew.CODIGO_PROFORMA);

            if (proforma != null)
            {
                proforma.CODIGO_PROFORMA = proformaNew.CODIGO_PROFORMA;
                proforma.DESCRICAO_PROFORMA = proformaNew.DESCRICAO_PROFORMA;
                proforma.DATA_PAGAMENTO = proformaNew.DATA_PAGAMENTO;
                proforma.VALOR_PAGAMENTO = proformaNew.VALOR_PAGAMENTO;
                proforma.CODIGO_COLECAO = proformaNew.CODIGO_COLECAO;
                proforma.CODIGO_JANELA = proformaNew.CODIGO_JANELA;
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

        public void AtualizaNel(IMPORTACAO_NEL nelNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_NEL nel = BuscaPorCodigoNel(nelNew.CODIGO_NEL);

            if (nel != null)
            {
                nel.CODIGO_NEL = nelNew.CODIGO_NEL;
                nel.DESCRICAO_NEL = nelNew.DESCRICAO_NEL;
                nel.DATA_PAGAMENTO = nelNew.DATA_PAGAMENTO;
                nel.VALOR_PAGAMENTO = nelNew.VALOR_PAGAMENTO;
                nel.DATA_DRAFT = nelNew.DATA_DRAFT;
                nel.ORIGINAL = nelNew.ORIGINAL;
                nel.DATA_RECEBIMENTO = nelNew.DATA_RECEBIMENTO;
                nel.DATA_DESPACHANTE = nelNew.DATA_DESPACHANTE;
                nel.DATA_LI = nelNew.DATA_LI;
                nel.DATA_BOOKING = nelNew.DATA_BOOKING;
                nel.DATA_EMBARQUE = nelNew.DATA_EMBARQUE;
                nel.DESCRICAO_DI = nelNew.DESCRICAO_DI;
                nel.DESCRICAO_BL = nelNew.DESCRICAO_BL;
                nel.PACK = nelNew.PACK;
                nel.COMERCIAL_INVOCE = nelNew.COMERCIAL_INVOCE;
                nel.DATA_PREVISAO_CHEGADA = nelNew.DATA_PREVISAO_CHEGADA;
                nel.DATA_DESEMBARACO = nelNew.DATA_DESEMBARACO;
                nel.STATUS_LIBERACAO = nelNew.STATUS_LIBERACAO;
                nel.CODIGO_CONTAINER = nelNew.CODIGO_CONTAINER;
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

        public void AtualizaContainer(IMPORTACAO_CONTAINER containerNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_CONTAINER container = BuscaPorCodigoContainer(containerNew.CODIGO_CONTAINER);

            if (container != null)
            {
                container.CODIGO_CONTAINER = containerNew.CODIGO_CONTAINER;
                container.DESCRICAO = containerNew.DESCRICAO;
                container.CODIGO_COLECAO = containerNew.CODIGO_COLECAO;
                container.CODIGO_JANELA = containerNew.CODIGO_JANELA;
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

        public void AtualizaPackGrade(IMPORTACAO_PACK_GRADE packGradeNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_PACK_GRADE packGrade = BuscaPorCodigoPackGrade(packGradeNew.CODIGO_PACK_GRADE);

            if (packGrade != null)
            {
                packGrade.CODIGO_PACK_GRADE = packGradeNew.CODIGO_PACK_GRADE;
                packGrade.CODIGO_GRUPO = packGradeNew.CODIGO_GRUPO;
                packGrade.DESCRICAO_TIPO = packGradeNew.DESCRICAO_TIPO;
                packGrade.CODIGO_GRIFFE = packGradeNew.CODIGO_GRIFFE;
                packGrade.QTDE_XP_PACK_A = packGradeNew.QTDE_XP_PACK_A;
                packGrade.QTDE_PP_PACK_A = packGradeNew.QTDE_PP_PACK_A;
                packGrade.QTDE_PQ_PACK_A = packGradeNew.QTDE_PQ_PACK_A;
                packGrade.QTDE_MD_PACK_A = packGradeNew.QTDE_MD_PACK_A;
                packGrade.QTDE_GD_PACK_A = packGradeNew.QTDE_GD_PACK_A;
                packGrade.QTDE_GG_PACK_A = packGradeNew.QTDE_GG_PACK_A;
                packGrade.QTDE_XP_PACK_B = packGradeNew.QTDE_XP_PACK_B;
                packGrade.QTDE_PP_PACK_B = packGradeNew.QTDE_PP_PACK_B;
                packGrade.QTDE_PQ_PACK_B = packGradeNew.QTDE_PQ_PACK_B;
                packGrade.QTDE_MD_PACK_B = packGradeNew.QTDE_MD_PACK_B;
                packGrade.QTDE_GD_PACK_B = packGradeNew.QTDE_GD_PACK_B;
                packGrade.QTDE_GG_PACK_B = packGradeNew.QTDE_GG_PACK_B;
                packGrade.QTDE_XP_PACK_C = packGradeNew.QTDE_XP_PACK_C;
                packGrade.QTDE_PP_PACK_C = packGradeNew.QTDE_PP_PACK_C;
                packGrade.QTDE_PQ_PACK_C = packGradeNew.QTDE_PQ_PACK_C;
                packGrade.QTDE_MD_PACK_C = packGradeNew.QTDE_MD_PACK_C;
                packGrade.QTDE_GD_PACK_C = packGradeNew.QTDE_GD_PACK_C;
                packGrade.QTDE_GG_PACK_C = packGradeNew.QTDE_GG_PACK_C;
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

        public void AtualizaPackGroup(IMPORTACAO_PACK_GROUP packGroupNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            IMPORTACAO_PACK_GROUP packGroup = BuscaPorCodigoPackGroup(packGroupNew.CODIGO_PACK_GROUP);

            if (packGroup != null)
            {
                packGroup.CODIGO_PACK_GROUP = packGroupNew.CODIGO_PACK_GROUP;
                packGroup.CODIGO_GRUPO = packGroupNew.CODIGO_GRUPO;
                packGroup.CODIGO_GRIFFE = packGroupNew.CODIGO_GRIFFE;
                packGroup.QTDE_TOTAL = packGroupNew.QTDE_TOTAL;
                packGroup.QTDE_PACK_A = packGroupNew.QTDE_PACK_A;
                packGroup.QTDE_PACK_B = packGroupNew.QTDE_PACK_B;
                packGroup.QTDE_PACK_C = packGroupNew.QTDE_PACK_C;
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

        public void InsereNotaRetiradaItem(NOTA_RETIRADA_ITEM notaRetiradaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.NOTA_RETIRADA_ITEMs.InsertOnSubmit(notaRetiradaItem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereNotaRetirada(NOTA_RETIRADA notaRetirada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.NOTA_RETIRADAs.InsertOnSubmit(notaRetirada);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereNotaTransferenciaItem(NOTA_TRANSFERENCIA_ITEM notaTransferenciaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.NOTA_TRANSFERENCIA_ITEMs.InsertOnSubmit(notaTransferenciaItem);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereNotaTransferencia(NOTA_TRANSFERENCIA notaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.NOTA_TRANSFERENCIAs.InsertOnSubmit(notaTransferencia);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereNotaDefeitoRetorno(NOTA_DEFEITO_RETORNO notaDefeitoRetorno)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                db.NOTA_DEFEITO_RETORNOs.InsertOnSubmit(notaDefeitoRetorno);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaNotaRetiradaItem(NOTA_RETIRADA_ITEM notaRetiradaItemNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_RETIRADA_ITEM notaRetiradaItem = BuscaNotaRetiradaItem(notaRetiradaItemNew.CODIGO_NOTA_RETIRADA_ITEM);

            if (notaRetiradaItem != null)
            {
                notaRetiradaItem.CODIGO_PRODUTO = notaRetiradaItemNew.CODIGO_PRODUTO;
                notaRetiradaItem.COR_PRODUTO = notaRetiradaItemNew.COR_PRODUTO;
                notaRetiradaItem.DESCRICAO_PRODUTO = notaRetiradaItemNew.DESCRICAO_PRODUTO;
                notaRetiradaItem.TAMANHO = notaRetiradaItemNew.TAMANHO;
                notaRetiradaItem.CODIGO_ORIGEM_DEFEITO = notaRetiradaItemNew.CODIGO_ORIGEM_DEFEITO;
                notaRetiradaItem.CODIGO_DEFEITO = notaRetiradaItemNew.CODIGO_DEFEITO;
                notaRetiradaItem.CODIGO_DESTINO = notaRetiradaItemNew.CODIGO_DESTINO;
                notaRetiradaItem.TIPO_NOTA = notaRetiradaItemNew.TIPO_NOTA;
                notaRetiradaItem.DATA_DESTINO = notaRetiradaItemNew.DATA_DESTINO;
                notaRetiradaItem.DATA_RETORNO = notaRetiradaItemNew.DATA_RETORNO;
                notaRetiradaItem.DATA_LANCAMENTO = notaRetiradaItemNew.DATA_LANCAMENTO;
                notaRetiradaItem.CODIGO_NOTA_RETIRADA = notaRetiradaItemNew.CODIGO_NOTA_RETIRADA;
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

        public void AtualizaNotaRetiradaItem(int codigoNotaRetiradaItem, string destino, int codigoProduto, string descricaoProduto, string corProduto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_RETIRADA_ITEM notaRetiradaItem = BuscaNotaRetiradaItem(codigoNotaRetiradaItem);

            if (notaRetiradaItem != null)
            {
                notaRetiradaItem.CODIGO_DESTINO = destino;
                notaRetiradaItem.CODIGO_PRODUTO = codigoProduto;
                notaRetiradaItem.DESCRICAO_PRODUTO = descricaoProduto;
                //notaRetiradaItem.COR_PRODUTO = corProduto;

                if (destino.Equals("0"))
                {
                    notaRetiradaItem.CODIGO_DESTINO = null;
                    notaRetiradaItem.DATA_DESTINO = null;
                    notaRetiradaItem.DATA_RETORNO = null;
                }
                else
                    notaRetiradaItem.DATA_DESTINO = DateTime.Now;
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

        public void AtualizaNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_RETIRADA_ITEM notaRetiradaItem = BuscaNotaRetiradaItem(codigoNotaRetiradaItem);

            if (notaRetiradaItem != null)
                notaRetiradaItem.DATA_RETORNO = DateTime.Now;

            try
            {
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaNotaRetiradaItem(int codigoNotaRetiradaItem, DateTime data, string destino)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_RETIRADA_ITEM notaRetiradaItem = BuscaNotaRetiradaItem(codigoNotaRetiradaItem);

            if (notaRetiradaItem != null)
            {
                notaRetiradaItem.DATA_DESTINO = data;
                notaRetiradaItem.CODIGO_DESTINO = destino;

                //if (!destino.Equals("1"))
                //notaRetiradaItem.DATA_RETORNO = data;
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

        public void AtualizaNotaRetirada(NOTA_RETIRADA notaRetiradaNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_RETIRADA notaRetirada = BuscaNotaRetirada(notaRetiradaNew.CODIGO_NOTA_RETIRADA);

            if (notaRetirada != null)
            {
                notaRetirada.CODIGO_FILIAL = notaRetiradaNew.CODIGO_FILIAL;

                notaRetirada.NUMERO_NOTA_LUGZI = notaRetiradaNew.NUMERO_NOTA_LUGZI;
                notaRetirada.DATA_NOTA_LUGZI = notaRetiradaNew.DATA_NOTA_LUGZI;

                notaRetirada.NUMERO_NOTA_LUGZI_TRAN = notaRetiradaNew.NUMERO_NOTA_LUGZI_TRAN;
                notaRetirada.DATA_NOTA_LUGZI_TRAN = notaRetiradaNew.DATA_NOTA_LUGZI_TRAN;

                notaRetirada.NUMERO_NOTA_CMAX = notaRetiradaNew.NUMERO_NOTA_CMAX;
                notaRetirada.DATA_NOTA_CMAX = notaRetiradaNew.DATA_NOTA_CMAX;

                notaRetirada.NUMERO_NOTA_CMAX_MOSTR = notaRetiradaNew.NUMERO_NOTA_CMAX_MOSTR;
                notaRetirada.DATA_NOTA_CMAX_MOSTR = notaRetiradaNew.DATA_NOTA_CMAX_MOSTR;

                notaRetirada.NUMERO_NOTA_CMAX_TRAN = notaRetiradaNew.NUMERO_NOTA_CMAX_TRAN;
                notaRetirada.DATA_NOTA_CMAX_TRAN = notaRetiradaNew.DATA_NOTA_CMAX_TRAN;

                notaRetirada.NUMERO_NOTA_LUCIANA_TRAN = notaRetiradaNew.NUMERO_NOTA_LUCIANA_TRAN;
                notaRetirada.DATA_NOTA_LUCIANA_TRAN = notaRetiradaNew.DATA_NOTA_LUCIANA_TRAN;

                notaRetirada.NUMERO_NOTA_MOSTRUARIO = notaRetiradaNew.NUMERO_NOTA_MOSTRUARIO;
                notaRetirada.DATA_NOTA_MOSTRUARIO = notaRetiradaNew.DATA_NOTA_MOSTRUARIO;

                notaRetirada.NUMERO_NOTA_TAGZY = notaRetiradaNew.NUMERO_NOTA_TAGZY;
                notaRetirada.DATA_NOTA_TAGZY = notaRetiradaNew.DATA_NOTA_TAGZY;

                notaRetirada.NUMERO_NOTA_HBF = notaRetiradaNew.NUMERO_NOTA_HBF;
                notaRetirada.DATA_NOTA_HBF = notaRetiradaNew.DATA_NOTA_HBF;
                notaRetirada.NUMERO_NOTA_CALCADOS = notaRetiradaNew.NUMERO_NOTA_CALCADOS;
                notaRetirada.DATA_NOTA_CALCADOS = notaRetiradaNew.DATA_NOTA_CALCADOS;
                notaRetirada.NUMERO_NOTA_OUTROS = notaRetiradaNew.NUMERO_NOTA_OUTROS;
                notaRetirada.DATA_NOTA_OUTROS = notaRetiradaNew.DATA_NOTA_OUTROS;

                notaRetirada.FLAG_ARQUIVO = notaRetiradaNew.FLAG_ARQUIVO;
                notaRetirada.BAIXADO = notaRetiradaNew.BAIXADO;
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

        public void AtualizaNotaRetirada(int codigoNotaRetirada, string notaCmax, string notaHbf, string notaCalcados, string notaOutros, string notaLugzi)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_RETIRADA notaRetirada = BuscaNotaRetirada(codigoNotaRetirada);

            if (notaRetirada != null)
            {
                notaRetirada.NUMERO_NOTA_CMAX = notaCmax;
                notaRetirada.NUMERO_NOTA_HBF = notaHbf;
                notaRetirada.NUMERO_NOTA_CALCADOS = notaCalcados;
                notaRetirada.NUMERO_NOTA_OUTROS = notaOutros;
                notaRetirada.NUMERO_NOTA_LUGZI = notaLugzi;
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

        public void AtualizaNotaTransferencia(NOTA_TRANSFERENCIA notaTransferenciaNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            NOTA_TRANSFERENCIA notaTransferencia = BuscaNotaTransferencia(notaTransferenciaNew.CODIGO);

            if (notaTransferencia != null)
            {
                notaTransferencia.CODIGO_FILIAL = notaTransferenciaNew.CODIGO_FILIAL;
                notaTransferencia.DATA_INICIO = notaTransferenciaNew.DATA_INICIO;
                notaTransferencia.DATA_FIM = notaTransferenciaNew.DATA_FIM;
                notaTransferencia.DESCRICAO = notaTransferenciaNew.DESCRICAO;
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

        public void AtualizaCaixa(FECHAMENTO_CAIXA caixaNew, int codigoCaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            FECHAMENTO_CAIXA caixa = BuscaCaixaPorCodigo(codigoCaixa);

            if (caixa != null)
            {
                caixa.VALOR_DINHEIRO = Convert.ToDecimal(caixaNew.VALOR_DINHEIRO);
                caixa.A_CRED_VISTA = caixaNew.A_CRED_VISTA;
                caixa.A_DEB_VISTA = caixaNew.A_DEB_VISTA;
                caixa.A_PARC_ADMIN = caixaNew.A_PARC_ADMIN;
                caixa.A_PARC_ESTAB_2X = caixaNew.A_PARC_ESTAB_2X;
                caixa.A_PARC_ESTAB_3X = caixaNew.A_PARC_ESTAB_3X;
                caixa.A_PARC_ESTAB_4X = caixaNew.A_PARC_ESTAB_4X;
                caixa.A_PARC_ESTAB_5X = caixaNew.A_PARC_ESTAB_5X;
                caixa.A_PARC_ESTAB_6X = caixaNew.A_PARC_ESTAB_6X;

                caixa.A_PARC_ESTAB_7X = caixaNew.A_PARC_ESTAB_7X;
                caixa.A_PARC_ESTAB_8X = caixaNew.A_PARC_ESTAB_8X;
                caixa.A_PARC_ESTAB_9X = caixaNew.A_PARC_ESTAB_9X;
                caixa.A_PARC_ESTAB_10X = caixaNew.A_PARC_ESTAB_10X;

                caixa.AMERICAN = caixaNew.AMERICAN;
                //caixa.CODIGO_CAIXA = caixaNew.CODIGO_CAIXA;
                caixa.CODIGO_FILIAL = caixaNew.CODIGO_FILIAL;
                caixa.data = caixaNew.data;
                caixa.DATA_FECHAMENTO = caixaNew.DATA_FECHAMENTO;
                caixa.H_CRED_VISTA = caixaNew.H_CRED_VISTA;
                caixa.H_DEB_VISTA = caixaNew.H_DEB_VISTA;
                caixa.H_PARC_ADMIN = caixaNew.H_PARC_ADMIN;
                caixa.H_PARC_ESTAB_2X = caixaNew.H_PARC_ESTAB_2X;
                caixa.H_PARC_ESTAB_3X = caixaNew.H_PARC_ESTAB_3X;
                caixa.H_PARC_ESTAB_4X = caixaNew.H_PARC_ESTAB_4X;
                caixa.H_PARC_ESTAB_5X = caixaNew.H_PARC_ESTAB_5X;
                caixa.H_PARC_ESTAB_6X = caixaNew.H_PARC_ESTAB_6X;

                caixa.H_PARC_ESTAB_7X = caixaNew.H_PARC_ESTAB_7X;
                caixa.H_PARC_ESTAB_8X = caixaNew.H_PARC_ESTAB_8X;
                caixa.H_PARC_ESTAB_9X = caixaNew.H_PARC_ESTAB_9X;
                caixa.H_PARC_ESTAB_10X = caixaNew.H_PARC_ESTAB_10X;

                caixa.HIPERCARD = caixaNew.HIPERCARD;
                caixa.M_CRED_VISTA = caixaNew.M_CRED_VISTA;
                caixa.M_DEB_VISTA = caixaNew.M_DEB_VISTA;
                caixa.M_PARC_ADMIN = caixaNew.M_PARC_ADMIN;
                caixa.M_PARC_ESTAB_2X = caixaNew.M_PARC_ESTAB_2X;
                caixa.M_PARC_ESTAB_3X = caixaNew.M_PARC_ESTAB_3X;
                caixa.M_PARC_ESTAB_4X = caixaNew.M_PARC_ESTAB_4X;
                caixa.M_PARC_ESTAB_5X = caixaNew.M_PARC_ESTAB_5X;
                caixa.M_PARC_ESTAB_6X = caixaNew.M_PARC_ESTAB_6X;

                caixa.M_PARC_ESTAB_7X = caixaNew.M_PARC_ESTAB_7X;
                caixa.M_PARC_ESTAB_8X = caixaNew.M_PARC_ESTAB_8X;
                caixa.M_PARC_ESTAB_9X = caixaNew.M_PARC_ESTAB_9X;
                caixa.M_PARC_ESTAB_10X = caixaNew.M_PARC_ESTAB_10X;

                caixa.MASTERCARD = caixaNew.MASTERCARD;
                caixa.OBS = caixaNew.OBS;
                caixa.OUTROS_CARTOES = caixaNew.OUTROS_CARTOES;
                caixa.RESPONSAVEL = caixaNew.RESPONSAVEL;
                caixa.V_CRED_VISTA = caixaNew.V_CRED_VISTA;
                caixa.V_DEB_VISTA = caixaNew.V_DEB_VISTA;
                caixa.V_PARC_ADMIN = caixaNew.V_PARC_ADMIN;
                caixa.V_PARC_ESTAB_2X = caixaNew.V_PARC_ESTAB_2X;
                caixa.V_PARC_ESTAB_3X = caixaNew.V_PARC_ESTAB_3X;
                caixa.V_PARC_ESTAB_4X = caixaNew.V_PARC_ESTAB_4X;
                caixa.V_PARC_ESTAB_5X = caixaNew.V_PARC_ESTAB_5X;
                caixa.V_PARC_ESTAB_6x = caixaNew.V_PARC_ESTAB_6x;

                caixa.V_PARC_ESTAB_7X = caixaNew.V_PARC_ESTAB_7X;
                caixa.V_PARC_ESTAB_8X = caixaNew.V_PARC_ESTAB_8X;
                caixa.V_PARC_ESTAB_9X = caixaNew.V_PARC_ESTAB_9X;
                caixa.V_PARC_ESTAB_10X = caixaNew.V_PARC_ESTAB_10X;

                caixa.VALOR_CARTAO_CREDITO = caixaNew.VALOR_CARTAO_CREDITO;
                caixa.VALOR_CARTAO_DEBITO = caixaNew.VALOR_CARTAO_DEBITO;
                caixa.VALOR_CHEQUE = caixaNew.VALOR_CHEQUE;
                caixa.VALOR_CHEQUE_PRE = caixaNew.VALOR_CHEQUE_PRE;
                caixa.VALOR_COBRANCA = caixaNew.VALOR_COBRANCA;
                caixa.VALOR_COMANDA = caixaNew.VALOR_COMANDA;
                caixa.VALOR_DESPESAS = caixaNew.VALOR_DESPESAS;
                caixa.VALOR_DEVOLUCAO = caixaNew.VALOR_DEVOLUCAO;
                caixa.VALOR_DEVOLUCAO_RETIRADA = caixaNew.VALOR_DEVOLUCAO_RETIRADA;
                caixa.VALOR_RETIRADA = caixaNew.VALOR_RETIRADA;
                caixa.VISA = caixaNew.VISA;
                caixa.CONFERIDO = caixaNew.CONFERIDO;
                caixa.OBS_MATRIZ = caixaNew.OBS_MATRIZ;
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

        public void ExcluiDocumento(int codigoDocumento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                DEPOSITO_DOCUMENTO depositoDocumento = BuscaDepositoDocumento(codigoDocumento);

                if (depositoDocumento != null)
                {
                    db.DEPOSITO_DOCUMENTOs.DeleteOnSubmit(depositoDocumento);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaDocumento(DEPOSITO_DOCUMENTO depositoNew)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            DEPOSITO_DOCUMENTO deposito = BuscaDepositoDocumento(depositoNew.CODIGO_DOCUMENTO);

            if (deposito != null)
            {
                deposito.USUARIO_CONF = depositoNew.USUARIO_CONF;
                deposito.NUMERO_LANCAMENTO = depositoNew.NUMERO_LANCAMENTO;
                deposito.DATA_CONF = depositoNew.DATA_CONF;
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

        public int Exclui(int codigo_usuario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                LojaController lojaController = new LojaController();

                List<USUARIOLOJA> usuarioLoja = lojaController.BuscaLojas(codigo_usuario);

                if (usuarioLoja.Count > 0)
                {
                    return 1; //Erro - Existe loja vinculada a este Staff
                }
                else
                {
                    USUARIO usuario = BuscaPorCodigoUsuario(codigo_usuario);

                    if (usuario != null)
                    {
                        db.USUARIOs.DeleteOnSubmit(usuario);
                        db.SubmitChanges();
                    }

                    return 0; // Ok
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluiChamada(int codigoChamada)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                TELEFONIA_CONTROLE_CHAMADA controleChamada = BuscaPorCodigoChamada(codigoChamada);

                if (controleChamada != null)
                {
                    db.TELEFONIA_CONTROLE_CHAMADAs.DeleteOnSubmit(controleChamada);
                    db.SubmitChanges();
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluiComprovante(int codigoComprovante)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                DEPOSITO_COMPROVANTE depositoComprovante = BuscaPorCodigoComprovante(codigoComprovante);

                if (depositoComprovante != null)
                {
                    db.DEPOSITO_COMPROVANTEs.DeleteOnSubmit(depositoComprovante);
                    db.SubmitChanges();
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluirNotaTransferencia(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                NOTA_TRANSFERENCIA notaTranferencia = BuscaNotaTransferencia(codigoNotaTransferencia);

                if (notaTranferencia != null)
                {
                    db.NOTA_TRANSFERENCIAs.DeleteOnSubmit(notaTranferencia);
                    db.SubmitChanges();
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluirNotaTransferenciaItem(int codigoNotaTransferencia)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                List<NOTA_TRANSFERENCIA_ITEM> listaNotaTranferenciaItem = BuscaNotaTransferenciaItem(codigoNotaTransferencia);

                if (listaNotaTranferenciaItem != null)
                {
                    foreach (NOTA_TRANSFERENCIA_ITEM item in listaNotaTranferenciaItem)
                    {
                        db.NOTA_TRANSFERENCIA_ITEMs.DeleteOnSubmit(item);
                        db.SubmitChanges();
                    }
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiHistoricoDespesa(int codigoDespesa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                FUNDO_FIXO_HISTORICO_DESPESA historicoDespesa = baseController.BuscaDespesaPorCodigo(codigoDespesa);

                if (historicoDespesa != null)
                {
                    db.FUNDO_FIXO_HISTORICO_DESPESAs.Attach(historicoDespesa);
                    db.FUNDO_FIXO_HISTORICO_DESPESAs.DeleteOnSubmit(historicoDespesa);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiColecao(IMPORTACAO_COLECAO colecaoOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_COLECAO colecao = BuscaPorCodigoColecao(colecaoOld.CODIGO_COLECAO);

                if (colecao != null)
                {
                    db.IMPORTACAO_COLECAOs.DeleteOnSubmit(colecao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiProforma(IMPORTACAO_PROFORMA proformaOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_PROFORMA proforma = BuscaPorCodigoProforma(proformaOld.CODIGO_PROFORMA);

                if (proforma != null)
                {
                    db.IMPORTACAO_PROFORMAs.DeleteOnSubmit(proforma);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiProformaProduto(IMPORTACAO_PROFORMA_PRODUTO proformaProdutoOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_PROFORMA_PRODUTO proformaProduto = baseController.BuscaProformaProduto(proformaProdutoOld.CODIGO_PROFORMA_PRODUTO);

                if (proformaProduto != null)
                {
                    db.IMPORTACAO_PROFORMA_PRODUTOs.Attach(proformaProduto);
                    db.IMPORTACAO_PROFORMA_PRODUTOs.DeleteOnSubmit(proformaProduto);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiNel(IMPORTACAO_NEL nelOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_NEL nel = BuscaPorCodigoNel(nelOld.CODIGO_NEL);

                if (nel != null)
                {
                    db.IMPORTACAO_NELs.DeleteOnSubmit(nel);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiContainer(IMPORTACAO_CONTAINER containerOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_CONTAINER container = BuscaPorCodigoContainer(containerOld.CODIGO_CONTAINER);

                if (container != null)
                {
                    db.IMPORTACAO_CONTAINERs.DeleteOnSubmit(container);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiPackGrade(IMPORTACAO_PACK_GRADE packGradeOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_PACK_GRADE packGrade = BuscaPorCodigoPackGrade(packGradeOld.CODIGO_PACK_GRADE);

                if (packGrade != null)
                {
                    db.IMPORTACAO_PACK_GRADEs.DeleteOnSubmit(packGrade);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcluiPackGroup(IMPORTACAO_PACK_GROUP packGroupOld)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                IMPORTACAO_PACK_GROUP packGroup = BuscaPorCodigoPackGroup(packGroupOld.CODIGO_PACK_GROUP);

                if (packGroup != null)
                {
                    db.IMPORTACAO_PACK_GROUPs.DeleteOnSubmit(packGroup);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluiNotaRetiradaItem(int codigoNotaRetiradaItem)
        {
            int erro = 1;

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                NOTA_RETIRADA_ITEM notaRetiradaItem = BuscaPorCodigoNotaRetiradaItem(codigoNotaRetiradaItem);

                if (notaRetiradaItem != null)
                {
                    db.NOTA_RETIRADA_ITEMs.DeleteOnSubmit(notaRetiradaItem);
                    db.SubmitChanges();

                    erro = 0;
                }

                return erro; // Ok
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluiNotaTransferenciaItem(int codigoNotaTransferenciaItem)
        {
            int erro = 1;

            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                NOTA_TRANSFERENCIA_ITEM notaTransferenciaItem = BuscaPorCodigoNotaTransferenciaItem(codigoNotaTransferenciaItem);

                if (notaTransferenciaItem != null)
                {
                    db.NOTA_TRANSFERENCIA_ITEMs.DeleteOnSubmit(notaTransferenciaItem);
                    db.SubmitChanges();

                    erro = 0;
                }

                return erro; // Ok
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExcluiCaixa(int codigoCaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                /*
                List<FECHAMENTO_ITEM_CAIXA> itensCaixa = baseController.BuscaItensCaixa(codigoCaixa);

                if (itensCaixa != null)
                {
                    foreach (FECHAMENTO_ITEM_CAIXA itemCaixa in itensCaixa)
                    {
                        db.FECHAMENTO_ITEM_CAIXAs.Attach(itemCaixa);
                        db.FECHAMENTO_ITEM_CAIXAs.DeleteOnSubmit(itemCaixa);
                        db.SubmitChanges();
                    }
                }
                */
                FECHAMENTO_CAIXA caixa = baseController.BuscaCaixa(codigoCaixa);

                if (caixa != null)
                {
                    db.FECHAMENTO_CAIXAs.Attach(caixa);
                    db.FECHAMENTO_CAIXAs.DeleteOnSubmit(caixa);
                    db.SubmitChanges();
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                return 1;

                throw ex;
            }
        }

        public int ExcluiHistoricoReceita(FECHAMENTO_CAIXA caixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                FUNDO_FIXO_HISTORICO_RECEITA receita = baseController.BuscaHistoricoReceita(caixa);

                if (receita != null)
                {
                    db.FUNDO_FIXO_HISTORICO_RECEITAs.Attach(receita);
                    db.FUNDO_FIXO_HISTORICO_RECEITAs.DeleteOnSubmit(receita);
                    db.SubmitChanges();
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                return 1;

                throw ex;
            }
        }
        public int ExcluiHistoricoDeposito(FECHAMENTO_CAIXA caixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            try
            {
                List<DEPOSITO_HISTORICO> listaDeposito = baseController.BuscaHistoricoDeposito(caixa);

                if (listaDeposito != null)
                {
                    foreach (DEPOSITO_HISTORICO item in listaDeposito)
                    {
                        db.DEPOSITO_HISTORICOs.Attach(item);
                        db.DEPOSITO_HISTORICOs.DeleteOnSubmit(item);
                        db.SubmitChanges();
                    }
                }

                return 0; // Ok
            }
            catch (Exception ex)
            {
                return 1;

                throw ex;
            }
        }

        public List<USUARIO> ObterEmailUsuarioTela(int codigoTela, int? codigoTelaModulo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from j in db.USUARIOs
                    join c in db.USUARIO_EMAIL_ENVIOs
                        on j.CODIGO_USUARIO equals c.USUARIO
                    where c.CODIGO_TELA == codigoTela
                    && (((codigoTelaModulo > 0) ? c.CODIGO_TELA_MODULO : null) == ((codigoTelaModulo > 0) ? codigoTelaModulo : null))
                    select j).ToList();
        }

        public List<SP_OBTER_REPRESENTANTES_ATIVOSResult> ObterRepresentantesAtivos()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return db.SP_OBTER_REPRESENTANTES_ATIVOS().ToList();
        }

    }
}
