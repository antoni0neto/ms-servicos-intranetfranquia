using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq.SqlClient;

namespace DAL
{
    public class ProducaoController
    {
        DCDataContext db;
        INTRADataContext dbIntra;
        LINXDataContext dbLinx;

        //PROD_HB_MOLDE
        public List<PROD_HB_MOLDE> ObterMolde()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_MOLDEs orderby m.CODIGO descending select m).ToList();
        }
        public PROD_HB_MOLDE ObterMolde(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_MOLDEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_HB_MOLDE ObterMolde(string molde)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_MOLDEs where m.MOLDE == molde select m).SingleOrDefault();
        }
        public string ObterMoldeUltimo()
        {
            string moldeCodigo = "";
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);

            var molde = (from i in dbIntra.PROD_HB_MOLDEs
                         select i).ToList();
            if (molde != null && molde.Count() > 0)
                moldeCodigo = molde.Max(p => p.MOLDE);

            int moldeInt = 0;
            if (int.TryParse(moldeCodigo, out moldeInt))
                moldeCodigo = (moldeInt += 1).ToString();
            else
                moldeCodigo = "";

            return moldeCodigo;
        }
        public void InserirMolde(PROD_HB_MOLDE molde)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.PROD_HB_MOLDEs.InsertOnSubmit(molde);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMolde(PROD_HB_MOLDE _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            PROD_HB_MOLDE molde = ObterMolde(_novo.CODIGO);

            if (_novo != null)
            {
                molde.CODIGO = _novo.CODIGO;
                molde.MOLDE = _novo.MOLDE;
                molde.MODELAGEM = _novo.MODELAGEM;
                molde.PRODUTO = _novo.PRODUTO;
                molde.USUARIO_CAD = _novo.USUARIO_CAD;
                molde.USUARIO_CAD_BAIXA = _novo.USUARIO_CAD_BAIXA;
                molde.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                molde.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

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
        public void ExcluirMolde(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_MOLDE molde = ObterMolde(codigo);
                if (molde != null)
                {
                    dbIntra.PROD_HB_MOLDEs.DeleteOnSubmit(molde);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SP_OBTER_MOLDE_PRODUTOResult> ObterMoldeProduto()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_MOLDE_PRODUTO() select m).ToList();
        }
        public List<SP_OBTER_HB_MOLDEResult> ObterMoldeHB(string colecao, int? hb, string produto, string nome, string molde, string modelagem, char? mostruario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_HB_MOLDE(colecao, hb, produto, nome, molde, modelagem, mostruario) select m).ToList();
        }
        //FIM PROD_HB_MOLDE


        //UNIDADE_MEDIDA
        public List<UNIDADE_MEDIDA> ObterUnidadeMedida()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.UNIDADE_MEDIDAs orderby m.DESCRICAO select m).ToList();
        }
        public UNIDADE_MEDIDA ObterUnidadeMedida(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.UNIDADE_MEDIDAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        //FIM UNIDADE_MEDIDA

        //AVIAMENTO
        public List<PROD_AVIAMENTO> ObterAviamento()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_AVIAMENTOs orderby m.DESCRICAO select m).ToList();
        }
        public PROD_AVIAMENTO ObterAviamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_AVIAMENTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public bool ObterAviamentoProdHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_PROD_AVIAMENTO _aviamento_hb = (from m in db.PROD_HB_PROD_AVIAMENTOs where m.PROD_AVIAMENTO == codigo select m).Take(1).SingleOrDefault();
            if (_aviamento_hb == null)
                return false;

            return true;
        }
        public void InserirAviamento(PROD_AVIAMENTO _aviamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_AVIAMENTOs.InsertOnSubmit(_aviamento);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAviamento(PROD_AVIAMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_AVIAMENTO _aviamento = ObterAviamento(_novo.CODIGO);

            if (_novo != null)
            {
                _aviamento.CODIGO = _novo.CODIGO;
                _aviamento.DESCRICAO = _novo.DESCRICAO;
                _aviamento.UNIDADE_MEDIDA = _novo.UNIDADE_MEDIDA;
                _aviamento.STATUS = _novo.STATUS;
                _aviamento.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _aviamento.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

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
        public void ExcluirAviamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_AVIAMENTO _aviamento = ObterAviamento(codigo);
                if (_aviamento != null)
                {
                    db.PROD_AVIAMENTOs.DeleteOnSubmit(_aviamento);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM AVIAMENTO

        //PROD_HB
        public int CriarHB(PROD_HB _hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HBs.InsertOnSubmit(_hb);
                db.SubmitChanges();

                return _hb.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AtualizarHB(PROD_HB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB _hb = ObterHB(_novo.CODIGO);

            if (_hb != null)
            {
                _hb.CODIGO = _novo.CODIGO;
                _hb.HB = _novo.HB;
                _hb.COLECAO = _novo.COLECAO;
                _hb.GRUPO = _novo.GRUPO;
                _hb.NOME = _novo.NOME;
                _hb.TECIDO = _novo.TECIDO;
                _hb.NUMERO_PEDIDO = _novo.NUMERO_PEDIDO;
                _hb.COR = _novo.COR;
                _hb.FORNECEDOR = _novo.FORNECEDOR;
                _hb.LARGURA = _novo.LARGURA;
                _hb.MODELAGEM = _novo.MODELAGEM;
                _hb.LIQUIDAR = _novo.LIQUIDAR;
                _hb.LIQUIDAR_PQNO = _novo.LIQUIDAR_PQNO;
                _hb.ATACADO = _novo.ATACADO;
                _hb.CUSTO_TECIDO = _novo.CUSTO_TECIDO;
                _hb.FOTO_TECIDO = _novo.FOTO_TECIDO;
                _hb.TIPO = _novo.TIPO;
                _hb.FOTO_PECA = _novo.FOTO_PECA;
                _hb.OBSERVACAO = _novo.OBSERVACAO;
                _hb.STATUS = _novo.STATUS;
                _hb.CODIGO_PRODUTO_LINX = _novo.CODIGO_PRODUTO_LINX;
                _hb.HB_COPIADO = _novo.HB_COPIADO;
                _hb.MOSTRUARIO = _novo.MOSTRUARIO;
                _hb.MOLDE = _novo.MOLDE;
                _hb.AMPLIACAO_OUTRO = _novo.AMPLIACAO_OUTRO;
                _hb.COR_FORNECEDOR = _novo.COR_FORNECEDOR;
                _hb.GRUPO_TECIDO = _novo.GRUPO_TECIDO;
                _hb.PROD_GRADE = _novo.PROD_GRADE;

                _hb.DATA_BAIXA_ETIQUETA = _novo.DATA_BAIXA_ETIQUETA;
                _hb.USUARIO_BAIXA_ETIQUETA = _novo.USUARIO_BAIXA_ETIQUETA;

                _hb.ORDEM_PRODUCAO = _novo.ORDEM_PRODUCAO;
                _hb.GASTO_FOLHA_SIMULACAO = _novo.GASTO_FOLHA_SIMULACAO;

                _hb.DATA_ENVIO_ETI_COMP = _novo.DATA_ENVIO_ETI_COMP;
                _hb.DATA_BAIXA_ETI_COMP = _novo.DATA_BAIXA_ETI_COMP;

                _hb.DATA_GRADE_ATACADO = _novo.DATA_GRADE_ATACADO;
                _hb.USUARIO_GRADE_ATACADO = _novo.USUARIO_GRADE_ATACADO;

                if (_novo.VOLUME != null)
                    _hb.VOLUME = _novo.VOLUME;

                _hb.ETIQUETA_COMPOSICAO = _novo.ETIQUETA_COMPOSICAO;

                if (_novo.GABARITO != null)
                    _hb.GABARITO = _novo.GABARITO;
                if (_novo.ESTAMPARIA != null)
                    _hb.ESTAMPARIA = _novo.ESTAMPARIA;

                if (_novo.DATA_IMP_FIC_LOGISTICA != null)
                    _hb.DATA_IMP_FIC_LOGISTICA = _novo.DATA_IMP_FIC_LOGISTICA;

                if (_novo.RENDIMENTO_1MT != null)
                    _hb.RENDIMENTO_1MT = _novo.RENDIMENTO_1MT;

                if (_novo.PRECO_FACC_MOSTRUARIO != null)
                    _hb.PRECO_FACC_MOSTRUARIO = _novo.PRECO_FACC_MOSTRUARIO;

                if (_novo.PROD_HB_FACCAO_PROBLEMA != null)
                    _hb.PROD_HB_FACCAO_PROBLEMA = _novo.PROD_HB_FACCAO_PROBLEMA;

                if (_novo.DESENV_PRODUTO_ORIGEM != null)
                    _hb.DESENV_PRODUTO_ORIGEM = _novo.DESENV_PRODUTO_ORIGEM;
                if (_novo.CORTE_SIMULACAO != null)
                    _hb.CORTE_SIMULACAO = _novo.CORTE_SIMULACAO;

                _hb.OBS_REL_FALTANTE = _novo.OBS_REL_FALTANTE;

                _hb.KIDS = _novo.KIDS;

                _hb.NOME_AMPLIACAO = _novo.NOME_AMPLIACAO;
                _hb.NOME_RISCO = _novo.NOME_RISCO;
                _hb.NOME_MODELAGEM = _novo.NOME_MODELAGEM;

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _hb.CODIGO;
        }
        public void AtualizarStatusHB(int codigo, char status)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB _hb = ObterHB(codigo);
                if (_hb != null)
                {
                    _hb.STATUS = status;

                    db.SubmitChanges();

                    List<PROD_HB> _Det = ObterDetalhesHB(codigo);
                    foreach (PROD_HB d in _Det)
                    {
                        d.STATUS = status;
                        db.SubmitChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AtualizarBaixaHB(PROD_HB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB _hb = ObterHB(_novo.CODIGO);

            if (_hb != null)
            {
                _hb.CODIGO = _novo.CODIGO;
                _hb.UNIDADE_MEDIDA = _novo.UNIDADE_MEDIDA;
                _hb.GASTO_FOLHA = _novo.GASTO_FOLHA;
                _hb.RETALHOS = _novo.RETALHOS;
                _hb.GASTO_PECA_CUSTO = _novo.GASTO_PECA_CUSTO;
                _hb.STATUS = _novo.STATUS;

                if (_novo.DATA_ENVIO_ETI_COMP != null)
                    _hb.DATA_ENVIO_ETI_COMP = _novo.DATA_ENVIO_ETI_COMP;
                if (_novo.VOLUME != null)
                    _hb.VOLUME = _novo.VOLUME;

                if (_novo.GABARITO != null)
                    _hb.GABARITO = _novo.GABARITO;
                if (_novo.ESTAMPARIA != null)
                    _hb.ESTAMPARIA = _novo.ESTAMPARIA;

                if (_novo.DATA_IMP_FIC_LOGISTICA != null)
                    _hb.DATA_IMP_FIC_LOGISTICA = _novo.DATA_IMP_FIC_LOGISTICA;

                if (_novo.RENDIMENTO_1MT != null)
                    _hb.RENDIMENTO_1MT = _novo.RENDIMENTO_1MT;

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
        public void AtualizarPedidoCompra(PROD_HB _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB _hb = ObterHB(_novo.CODIGO);

            if (_hb != null)
            {
                _hb.CODIGO = _novo.CODIGO;
                _hb.PC_ATACADO = _novo.PC_ATACADO;
                _hb.PC_VAREJO = _novo.PC_VAREJO;

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
        public int ObterUltimoHB(string colecao, char mostruario)
        {
            int? HB = null;
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            HB = (from i in db.PROD_HBs
                  where i.CODIGO_PAI == null
                  && i.STATUS != 'X'
                  && i.MOSTRUARIO == mostruario
                  && i.COLECAO == colecao
                  select i).Max(p => p.HB);

            if (HB.HasValue)
                return Convert.ToInt32(HB += 1);

            return 1;
        }
        public PROD_HB ObterHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<PROD_HB> ObterHB()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs where m.CODIGO_PAI == null && m.STATUS != 'E' && m.STATUS != 'X' select m).ToList();
        }
        public List<PROD_HB> ObterHB(string codigoProdutoLinx)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs
                    where m.CODIGO_PAI == null &&
                    m.STATUS != 'E' &&
                    m.STATUS != 'X' &&
                    m.CODIGO_PRODUTO_LINX.Trim() == codigoProdutoLinx.ToUpper().Trim()
                    select m).ToList();
        }
        public PROD_HB ObterHB(string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs
                    where m.CODIGO_PRODUTO_LINX.Trim() == produto.Trim()
                     && m.COR == cor.Trim()
                     && m.STATUS == 'B'
                     && m.CODIGO_PAI == null
                    orderby m.DATA_INCLUSAO descending
                    select m).FirstOrDefault();
        }
        public PROD_HB ObterHB(string colecao, string produto, string cor, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs
                    where m.COLECAO.Trim() == colecao.Trim()
                     && m.CODIGO_PRODUTO_LINX.Trim() == produto.Trim()
                     && m.COR == cor.Trim()
                     && m.STATUS != 'E'
                     && m.CODIGO_PAI == null
                     && m.MOSTRUARIO == mostruario
                    select m).FirstOrDefault();
        }

        public List<PROD_HB> ObterHBPorDesenvProduto(string produto, string cor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs
                    where m.CODIGO_PRODUTO_LINX.Trim() == produto.Trim()
                     && m.COR == cor.Trim()
                    select m).ToList();
        }

        public List<PROD_HB> ObterNumeroHB(string colecao, int hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs
                    where
                    m.CODIGO_PAI == null && m.COLECAO == colecao && m.HB == hb
                    select m).ToList();
        }
        public PROD_HB ObterNumeroHBDetalhe(string colecao, int hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs
                    where
                    m.CODIGO_PAI != null && m.COLECAO == colecao && m.HB == hb
                    select m).Take(1).SingleOrDefault();
        }
        public List<SP_OBTER_GRUPOResult> ObterGrupoProduto(string codCategoria)
        {
            //01 - PEÇAS
            //02 - ACESSORIOS

            db = new DCDataContext(Constante.ConnectionString);
            return (from pg in db.SP_OBTER_GRUPO(codCategoria) select pg).ToList();
        }
        public List<SP_OBTER_SUBGRUPO_PRODUTOResult> ObterSubGrupoProduto(string subGrupoProduto, string grupoProduto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from pg in dbLinx.SP_OBTER_SUBGRUPO_PRODUTO(subGrupoProduto, grupoProduto) select pg).ToList();
        }
        public List<CORES_BASICA> ObterCoresBasicas()
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CORES_BASICAs
                    where
                    !m.DESC_COR.Substring(0, 1).Equals("0") &&
                    !m.DESC_COR.Substring(0, 1).Equals("1") &&
                    !m.DESC_COR.Substring(0, 1).Equals("2") &&
                    !m.DESC_COR.Substring(0, 1).Equals("3") &&
                    !m.DESC_COR.Substring(0, 1).Equals("4") &&
                    !m.DESC_COR.Substring(0, 1).Equals("5") &&
                    !m.DESC_COR.Substring(0, 1).Equals("6") &&
                    !m.DESC_COR.Substring(0, 1).Equals("7") &&
                    !m.DESC_COR.Substring(0, 1).Equals("8") &&
                    !m.DESC_COR.Substring(0, 1).Equals("9")
                    orderby m.DESC_COR
                    select m).ToList();
        }
        public List<CORES_BASICA> ObterCoresBasicasDesc(string desc_cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CORES_BASICAs
                    where m.DESC_COR.Trim().Contains(desc_cor.Trim().ToUpper())
                    select m).ToList();
        }
        public CORES_BASICA ObterCoresBasicasDescricao(string desc_cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CORES_BASICAs
                    where m.DESC_COR.Trim() == desc_cor.Trim()
                    select m).SingleOrDefault();
        }
        public CORES_BASICA ObterCoresBasicas(string cor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.CORES_BASICAs where m.COR == cor select m).SingleOrDefault();
        }
        public List<SP_OBTER_FORNECEDORESResult> ObterFornecedores(string subtipo)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.SP_OBTER_FORNECEDORES(subtipo) select i).ToList();
        }
        public List<PROD_HB> ValidarNumeroHB(int _HB, string _colecao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.PROD_HBs
                    where i.HB == _HB
                    && i.COLECAO == _colecao
                    && i.CODIGO_PAI == null
                    && i.STATUS != 'X'
                    select i).ToList();
        }
        public List<PROD_HB> ObterHBFila(int processo, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where i.STATUS == 'P'
                    && p.PROD_PROCESSO == processo
                    && p.DATA_FIM == null
                    && i.PROD_DETALHE == null
                    && i.MOSTRUARIO == mostruario
                    orderby i.COLECAO ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }
        public List<PROD_HB> ObterHBFilaDetalhe(int processo, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where i.STATUS == 'P'
                    && p.PROD_PROCESSO == processo
                    && p.DATA_FIM == null
                    && i.PROD_DETALHE != null
                    //&& i.PROD_DETALHE != 5
                    && i.MOSTRUARIO == mostruario
                    orderby i.COLECAO ascending, i.HB ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();

        }

        public List<SP_OBTER_HB_FILAResult> ObterHBFilaPrincipalDetalhe(int processo, char mostruario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_HB_FILA(processo, mostruario) select m).ToList();
        }

        /*public List<PROD_HB> ObterHBFilaCorte()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where i.STATUS == 'P' && p.PROD_PROCESSO == 3 && p.DATA_FIM == null && i.PROD_DETALHE == null
                    orderby i.COLECAO ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }*/
        /*public List<PROD_HB> ObterHBFilaCorteDetalhe()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where i.STATUS == 'P' && p.PROD_PROCESSO == 3 && p.DATA_FIM == null && i.PROD_DETALHE != null
                    orderby i.COLECAO ascending, i.HB ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }*/
        public List<PROD_HB> ObterHBFilaCortado(DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where i.STATUS == 'B' && p.PROD_PROCESSO == 3 && Convert.ToDateTime(p.DATA_FIM).Date == dataFim.Date && (i.PROD_DETALHE == null)
                    orderby i.COLECAO ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }
        public List<PROD_HB> ObterHBFilaCortadoDetalhe(DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where i.STATUS == 'B' && p.PROD_PROCESSO == 3 && Convert.ToDateTime(p.DATA_FIM).Date == dataFim.Date && i.PROD_DETALHE != null
                    orderby i.COLECAO ascending, i.HB ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }
        public List<PROD_HB> ObterHBFilaRiscado(DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where p.PROD_PROCESSO == 2 && p.DATA_FIM != null && Convert.ToDateTime(p.DATA_FIM).Date == dataFim.Date && i.PROD_DETALHE == null
                    orderby i.COLECAO ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }
        public List<PROD_HB> ObterHBFilaRiscadoDetalhe(DateTime dataFim)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join p in db.PROD_HB_PROD_PROCESSOs
                    on i.CODIGO equals p.PROD_HB
                    where p.PROD_PROCESSO == 2 && p.DATA_FIM != null && Convert.ToDateTime(p.DATA_FIM).Date == dataFim.Date && i.PROD_DETALHE != null
                    orderby i.COLECAO ascending, i.HB ascending, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }
        public List<PROD_HB> ObterHBPedidoNumero(int pedidoNumero)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from i in db.PROD_HBs
                    join j in db.DESENV_PRODUTO_HBs
                    on i.CODIGO equals j.PROD_HB into hbLeft
                    from j in hbLeft.DefaultIfEmpty()
                    where j.PROD_HB == null && i.NUMERO_PEDIDO == pedidoNumero && i.STATUS != 'E' && i.STATUS != 'X' && i.MOSTRUARIO == 'N' && i.CODIGO_PAI == null
                    orderby i.COLECAO ascending, i.HB ascending, i.GRUPO, i.DATA_INCLUSAO ascending
                    select i).ToList();
        }
        public int ObterVolumeHB(int prod_hb)
        {
            int? volume = null;
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            volume = (from i in db.PROD_HBs
                      where i.CODIGO == prod_hb
                      select i).SingleOrDefault().VOLUME;

            var det = (from i in db.PROD_HBs
                       where i.CODIGO_PAI == prod_hb
                       select i).Sum(p => p.VOLUME);

            if (det.HasValue)
                volume += det.Value;

            if (volume.HasValue)
                return Convert.ToInt32(volume);

            return 0;
        }

        public string ObterMolde(string colecao, string grupo, string cor, string codigoLinx)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            string molde = "&nbsp;";
            try
            {
                var moldeProd = (from i in db.PROD_HBs
                                 where i.COLECAO == colecao && i.MOSTRUARIO == 'N' && i.GRUPO == grupo && i.COR == cor && i.CODIGO_PRODUTO_LINX == codigoLinx
                                 select i).FirstOrDefault();

                if (moldeProd == null)
                {
                    var moldeMostru = (from i in db.PROD_HBs
                                       where i.COLECAO == colecao && i.MOSTRUARIO == 'S' && i.GRUPO == grupo && i.COR == cor && i.CODIGO_PRODUTO_LINX == codigoLinx
                                       select i).FirstOrDefault();

                    if (moldeMostru != null)
                        molde = "MOL MOSTR: " + moldeMostru.MOLDE;
                }
                else
                {
                    molde = "MOL PROD: " + moldeProd.MOLDE;
                }
            }
            catch (Exception ex)
            {
                molde = "&nbsp;";
            }

            return molde;
        }

        public bool ValidarHBCortado(int prod_hb)
        {
            bool cortado = false;
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            var pri = (from i in db.PROD_HBs
                       where i.CODIGO == prod_hb
                       select i).SingleOrDefault();

            if (pri != null && pri.STATUS == 'B')
                cortado = true;

            var det = (from i in db.PROD_HBs
                       where i.CODIGO_PAI == prod_hb
                       select i).ToList();

            foreach (var d in det)
                if (d != null && d.STATUS != 'B')
                {
                    cortado = false;
                    break;
                }

            return cortado;
        }
        //FIM //PROD_HB

        //PROD_HB_COMPOSICAO
        public SP_OBTER_COMPOSICAO_PRODUTO_LINXResult ObterComposicaoPorProdutoLinx(string produto)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_OBTER_COMPOSICAO_PRODUTO_LINX(produto) select m).SingleOrDefault();
        }
        public List<PROD_HB_COMPOSICAO> ObterComposicaoHB(int _prodHB)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_COMPOSICAOs where m.PROD_HB == _prodHB select m).ToList();
        }
        public PROD_HB_COMPOSICAO ObterComposicao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_COMPOSICAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public void InserirComposicao(PROD_HB_COMPOSICAO _composicao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_COMPOSICAOs.InsertOnSubmit(_composicao);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirComposicao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_COMPOSICAO _composicao = ObterComposicao(codigo);
                if (_composicao != null)
                {
                    db.PROD_HB_COMPOSICAOs.DeleteOnSubmit(_composicao);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_HB_COMPOSICAO

        //PROD_DETALHES
        public List<PROD_DETALHE> ObterDetalhes()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_DETALHEs where m.STATUS == 'A' orderby m.CODIGO select m).ToList();
        }
        public PROD_DETALHE ObterDetalhes(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_DETALHEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<PROD_HB> ObterDetalhesHB(int codigo_pai)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs where m.CODIGO_PAI == codigo_pai select m).ToList();
        }
        public PROD_HB ObterDetalhesHB(int codigo_pai, int detalhe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HBs where m.CODIGO_PAI == codigo_pai && m.PROD_DETALHE == detalhe select m).SingleOrDefault();
        }
        public int InserirDetalheHB(PROD_HB _detalhe)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HBs.InsertOnSubmit(_detalhe);
                db.SubmitChanges();

                return _detalhe.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirDetalheHB(int _codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB _detalhe = ObterHB(_codigo);
                if (_detalhe != null)
                {
                    db.PROD_HBs.DeleteOnSubmit(_detalhe);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_DETALHES

        //PROD_HB_PROD_AVIAMENTO
        public List<PROD_HB_PROD_AVIAMENTO> ObterAviamentoHB(int _prodHB)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_PROD_AVIAMENTOs where m.PROD_HB == _prodHB orderby m.DESCRICAO select m).ToList();
        }
        public PROD_HB_PROD_AVIAMENTO ObterAviamentoHBCodigo(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_PROD_AVIAMENTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<PROD_HB_PROD_AVIAMENTO> ObterAviamentoHB()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_PROD_AVIAMENTOs
                    where m.PROD_HB1.STATUS != 'X' && m.PROD_HB1.STATUS != 'E'
                    orderby m.PROD_HB1.COLECAO ascending, m.PROD_HB1.HB ascending, m.DATA_INCLUSAO
                    select m).ToList();
        }
        public int InserirAviamentoHB(PROD_HB_PROD_AVIAMENTO _aviamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_PROD_AVIAMENTOs.InsertOnSubmit(_aviamento);
                db.SubmitChanges();

                return _aviamento.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAviamentoHB(PROD_HB_PROD_AVIAMENTO _aviamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_PROD_AVIAMENTO _aviamento_hb = ObterAviamentoHBCodigo(_aviamento.CODIGO);
                if (_aviamento_hb != null)
                {
                    if (_aviamento.QTDE_EXTRA != null)
                        _aviamento_hb.QTDE_EXTRA = _aviamento.QTDE_EXTRA;
                    if (_aviamento.QTDE != null && _aviamento.QTDE > 0)
                        _aviamento_hb.QTDE = _aviamento.QTDE;
                    if (_aviamento.NUMERO_PEDIDO != null)
                        _aviamento_hb.NUMERO_PEDIDO = _aviamento.NUMERO_PEDIDO;
                    if (_aviamento.AVIAMENTO_LIBERADO != null)
                        _aviamento_hb.AVIAMENTO_LIBERADO = _aviamento.AVIAMENTO_LIBERADO;

                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void ExcluirAviamentoHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_PROD_AVIAMENTO _aviamento = ObterAviamentoHBCodigo(codigo);
                if (_aviamento != null)
                {
                    db.PROD_HB_PROD_AVIAMENTOs.DeleteOnSubmit(_aviamento);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAviamentoCusto(PROD_HB_PROD_AVIAMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_PROD_AVIAMENTO _aviamento = ObterAviamentoHBCodigo(_novo.CODIGO);
            if (_aviamento != null)
            {
                _aviamento.CONSUMO = _novo.CONSUMO;
                _aviamento.CUSTO_UNITARIO = _novo.CUSTO_UNITARIO;

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
        //FIM -PROD_HB_PROD_AVIAMENTO
        //GRADE
        public void InserirGrade(PROD_HB_GRADE _grade)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_GRADEs.InsertOnSubmit(_grade);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PROD_HB_GRADE ObterGrade(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_GRADEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_HB_GRADE ObterGradeHB(int prod_hb, int processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_GRADEs where m.PROD_HB == prod_hb && m.PROD_PROCESSO == processo select m).FirstOrDefault();
            /*
                1	Ampliação
                2	Risco
                3	Corte
             * 99   Atacado
             * */
        }
        public int ObterQtdeGradeHB(int prod_hb, int processo)
        {
            int total = 0;
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            var query = (from m in db.PROD_HB_GRADEs where m.PROD_HB == prod_hb && m.PROD_PROCESSO == processo select m).FirstOrDefault();

            if (query != null)
                total += Convert.ToInt32(query.GRADE_EXP) + Convert.ToInt32(query.GRADE_XP) + Convert.ToInt32(query.GRADE_PP) + Convert.ToInt32(query.GRADE_P) + Convert.ToInt32(query.GRADE_M) + Convert.ToInt32(query.GRADE_G) + Convert.ToInt32(query.GRADE_GG);

            return total;
        }
        public void AtualizarGrade(PROD_HB_GRADE _novo, int processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_GRADE _grade = ObterGradeHB(_novo.PROD_HB, processo);

            if (_grade != null)
            {
                _grade.PROD_HB = _novo.PROD_HB;
                _grade.PROD_PROCESSO = _novo.PROD_PROCESSO;
                _grade.GRADE_EXP = _novo.GRADE_EXP;
                _grade.GRADE_XP = _novo.GRADE_XP;
                _grade.GRADE_PP = _novo.GRADE_PP;
                _grade.GRADE_P = _novo.GRADE_P;
                _grade.GRADE_M = _novo.GRADE_M;
                _grade.GRADE_G = _novo.GRADE_G;
                _grade.GRADE_GG = _novo.GRADE_GG;
                _grade.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _grade.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO; //((USUARIO)Session["USUARIO"]).CODIGO_USUARIO;

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
        // FIM GRADE

        //PROCESSO
        public List<PROD_HB_PROD_PROCESSO> ObterProcesso(string colecao, int hb, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_PROD_PROCESSOs
                    where m.PROD_HB1.COLECAO == colecao
                    && m.PROD_HB1.HB == hb
                    && m.PROD_HB1.MOSTRUARIO == mostruario
                    && (m.PROD_HB1.STATUS == 'B' || m.PROD_HB1.STATUS == 'P')
                    select m).ToList();
        }
        public void AtualizarProcessoHB(PROD_HB_PROD_PROCESSO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_PROD_PROCESSO processoHB = ObterProcessoHB(_novo.CODIGO);
                if (processoHB != null)
                {
                    if (_novo.DATA_INICIO != null)
                        processoHB.DATA_INICIO = _novo.DATA_INICIO;

                    if (_novo.DATA_FIM != null)
                        processoHB.DATA_FIM = Convert.ToDateTime(Convert.ToDateTime(_novo.DATA_FIM).ToString("yyyy-MM-dd") + " 23:59:59");

                    db.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PROD_PROCESSO ObterProcesso(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_PROCESSOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_PROCESSO ObterProcessoOrdem(int ordem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_PROCESSOs where m.ORDEM == ordem select m).SingleOrDefault();
        }
        public void InserirProcesso(PROD_HB_PROD_PROCESSO _processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_PROD_PROCESSOs.InsertOnSubmit(_processo);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PROD_HB_PROD_PROCESSO ObterProcessoHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_PROD_PROCESSOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_HB_PROD_PROCESSO ObterProcessoHB(int prod_hb, int _processo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_PROD_PROCESSOs where m.PROD_HB == prod_hb && m.PROD_PROCESSO == _processo select m).Take(1).SingleOrDefault();
        }
        public void AtualizarProcessoDataFim(int _prod_hb, int _processo, DateTime _dataBaixa)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_PROD_PROCESSO _processo_hb = ObterProcessoHB(_prod_hb, _processo);

            if (_processo_hb != null)
            {
                string horaAtual = DateTime.Now.ToString("HH:mm:ss");
                string dataBaixa = _dataBaixa.ToString("yyyy-MM-dd ");

                _processo_hb.DATA_FIM = Convert.ToDateTime(dataBaixa + horaAtual);
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

        public void VoltarProcessoRisco(int _prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            //ATUALIZAR DATA FIM RISCO = NULL
            PROD_HB_PROD_PROCESSO phbRisco = ObterProcessoHB(_prod_hb, 2);
            if (phbRisco != null)
            {
                phbRisco.DATA_FIM = null;
                try
                {
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            //EXCLUIR REGISTRO DE CORTE
            PROD_HB_PROD_PROCESSO phbCorte = ObterProcessoHB(_prod_hb, 3);
            if (phbCorte != null)
            {
                db.PROD_HB_PROD_PROCESSOs.DeleteOnSubmit(phbCorte);
                db.SubmitChanges();
            }

        }
        //FIM PROCESSO

        //GRADE SUB
        public List<PROD_HB_GRADE_SUB> ObterGradeSubHB(int _prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PROD_HB_GRADE_SUBs
                    join c in db.PROD_HB_CORTE_GRADE_SUBs
                    on g.CODIGO equals c.PROD_HB_GRADE_SUB into gradeLeft
                    from c in gradeLeft.DefaultIfEmpty()
                    where c.CODIGO == null && g.PROD_HB == _prod_hb
                    select g).ToList();
        }
        public PROD_HB_GRADE_SUB ObterGradeSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from g in db.PROD_HB_GRADE_SUBs where g.CODIGO == codigo select g).SingleOrDefault();
        }
        public void InserirGradeSub(PROD_HB_GRADE_SUB _grade_sub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_GRADE_SUBs.InsertOnSubmit(_grade_sub);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirGradeSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_GRADE_SUB _grade_sub = ObterGradeSub(codigo);
                if (_grade_sub != null)
                {
                    db.PROD_HB_GRADE_SUBs.DeleteOnSubmit(_grade_sub);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //GRADE SUB

        //GRADE CORTE
        public PROD_HB_CORTE ObterGradeCorte(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.PROD_HB_CORTEs where c.CODIGO == codigo select c).SingleOrDefault();
        }
        public int InserirGradeCorte(PROD_HB_CORTE _corte)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_CORTEs.InsertOnSubmit(_corte);
                db.SubmitChanges();

                return _corte.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirGradeCorte(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_CORTE _corte = ObterGradeCorte(codigo);
                if (_corte != null)
                {
                    db.PROD_HB_CORTEs.DeleteOnSubmit(_corte);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarGradeCorte(PROD_HB_CORTE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_CORTE _corte = ObterGradeCorte(_novo.CODIGO);

            if (_corte != null)
            {
                _corte.PROD_HB = _novo.PROD_HB;
                _corte.FOLHA = _novo.FOLHA;
                _corte.ENFESTO = _novo.ENFESTO;
                _corte.CORTE = _novo.CORTE;
                _corte.AMARRADOR = _novo.AMARRADOR;
                if (_novo.RISCO_NOME != null && _novo.RISCO_NOME.Trim() != "")
                    _corte.RISCO_NOME = _novo.RISCO_NOME;

                try
                {
                    db.SubmitChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        //FIM GRADE CORTE

        //GRADE CORTE SUB
        public List<SP_OBTER_GRADE_CORTEResult> ObterGradeCorteHB(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.SP_OBTER_GRADE_CORTE(Convert.ToInt32(prod_hb)) select c).ToList();
        }
        public void ExcluirGradeCorteHB(int prod_hb_grade)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.SP_EXCLUIR_GRADE_CORTE(Convert.ToInt32(prod_hb_grade));
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PROD_HB_CORTE_GRADE_SUB ObterGradeCorteSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.PROD_HB_CORTE_GRADE_SUBs where c.CODIGO == codigo select c).SingleOrDefault();
        }
        public List<PROD_HB_CORTE_GRADE_SUB> ObterGradeCorteSubPorCorte(int prod_hb_corte)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from c in db.PROD_HB_CORTE_GRADE_SUBs where c.PROD_HB_CORTE == prod_hb_corte select c).ToList();
        }
        public void InserirGradeCorteSub(PROD_HB_CORTE_GRADE_SUB _corteSub)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_CORTE_GRADE_SUBs.InsertOnSubmit(_corteSub);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirGradeCorteSub(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_CORTE_GRADE_SUB _corte = ObterGradeCorteSub(codigo);
                if (_corte != null)
                {
                    db.PROD_HB_CORTE_GRADE_SUBs.DeleteOnSubmit(_corte);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM GRADE CORTE SUB
        public void InserirDadosGalao(int prod_hb_pai, int prod_detalhe)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.SP_MONTAR_GALAO(Convert.ToInt32(prod_hb_pai), prod_detalhe);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void baixarGalaoProprio(int prod_hb_pai)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.SP_BAIXAR_GALAO_PROPRIO(Convert.ToInt32(prod_hb_pai));
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //HB EM PROCESSAMENTO
        public List<SP_OBTER_HB_PROCESSOResult> ObterHBProcesso(string dataInicial, string dataFinal, string grupo)
        {
            BaseController _base = new BaseController();
            string dataIni = _base.AjustaData(dataInicial);
            string dataFim = _base.AjustaData(dataFinal);
            if (grupo == "Selecione" || grupo == "0")
                grupo = null;

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_OBTER_HB_PROCESSO(dataIni, dataFim, grupo, null, null, null) select h).ToList();
        }
        public List<SP_OBTER_HB_PROCESSOResult> ObterHBProcesso(int? hb, string colecao, string nome)
        {
            string sHB = "";
            if (hb != null)
                sHB = hb.ToString();
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from h in db.SP_OBTER_HB_PROCESSO(null, null, null, sHB, colecao, nome) select h).ToList();
        }
        //FIM //HB EM PROCESSAMENTO

        //PROD_CORTE_FUNCIONARIO
        public List<PROD_CORTE_FUNCIONARIO> ObterCorteFuncionario()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_CORTE_FUNCIONARIOs orderby m.NOME select m).ToList();
        }
        public PROD_CORTE_FUNCIONARIO ObterCorteFuncionario(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_CORTE_FUNCIONARIOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public void InserirCorteFuncionario(PROD_CORTE_FUNCIONARIO _corteFunc)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_CORTE_FUNCIONARIOs.InsertOnSubmit(_corteFunc);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCorteFuncionario(PROD_CORTE_FUNCIONARIO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_CORTE_FUNCIONARIO _CorteFunc = ObterCorteFuncionario(_novo.CODIGO);

            if (_CorteFunc != null)
            {
                _CorteFunc.CODIGO = _novo.CODIGO;
                _CorteFunc.NOME = _novo.NOME;
                _CorteFunc.STATUS = _novo.STATUS;
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
        public void ExcluirCorteFuncionario(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_CORTE_FUNCIONARIO _corteFunc = ObterCorteFuncionario(codigo);
                if (_corteFunc != null)
                {
                    db.PROD_CORTE_FUNCIONARIOs.DeleteOnSubmit(_corteFunc);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM PROD_CORTE_FUNCIONARIO

        //PROD_FORNECEDOR
        public List<PROD_FORNECEDOR> ObterFornecedor()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_FORNECEDORs
                    orderby m.FORNECEDOR
                    select m).ToList();
        }
        public PROD_FORNECEDOR ObterFornecedor(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_FORNECEDORs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_FORNECEDOR ObterFornecedor(string fornecedor, char tipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_FORNECEDORs
                    where m.FORNECEDOR.Trim() == fornecedor.Trim() &&
                            m.TIPO == tipo
                    select m).SingleOrDefault();
        }
        public int InserirFornecedor(PROD_FORNECEDOR _fornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_FORNECEDORs.InsertOnSubmit(_fornecedor);
                db.SubmitChanges();

                return _fornecedor.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarFornecedor(PROD_FORNECEDOR _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_FORNECEDOR _fornecedor = ObterFornecedor(_novo.CODIGO);

            if (_fornecedor != null)
            {
                _fornecedor.CODIGO = _novo.CODIGO;
                _fornecedor.FORNECEDOR = _novo.FORNECEDOR;
                _fornecedor.PORC_ICMS = _novo.PORC_ICMS;
                _fornecedor.TIPO = _novo.TIPO;
                _fornecedor.STATUS = _novo.STATUS;
                _fornecedor.EMAIL = _novo.EMAIL;

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
        public void ExcluirFornecedor(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_FORNECEDOR _fornecedor = ObterFornecedor(codigo);

            if (_fornecedor != null)
            {
                try
                {
                    db.PROD_FORNECEDORs.DeleteOnSubmit(_fornecedor);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - PROD_FORNECEDOR

        //PROD_HB_GRADE_ATACADO
        public List<PROD_HB_GRADE_ATACADO> ObterGradeAtacado()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_GRADE_ATACADOs
                    orderby m.COLECAO, m.GRUPO, m.GRIFFE
                    select m).ToList();
        }
        public PROD_HB_GRADE_ATACADO ObterGradeAtacado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_GRADE_ATACADOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirGradeAtacado(PROD_HB_GRADE_ATACADO _gradeAtacado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_GRADE_ATACADOs.InsertOnSubmit(_gradeAtacado);
                db.SubmitChanges();

                return _gradeAtacado.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarGradeAtacado(PROD_HB_GRADE_ATACADO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_GRADE_ATACADO _gradeAtacado = ObterGradeAtacado(_novo.CODIGO);

            if (_gradeAtacado != null)
            {
                _gradeAtacado.CODIGO = _novo.CODIGO;
                _gradeAtacado.COLECAO = _novo.COLECAO;
                _gradeAtacado.GRUPO = _novo.GRUPO;
                _gradeAtacado.GRIFFE = _novo.GRIFFE;
                _gradeAtacado.GRADE_EXP = _novo.GRADE_EXP;
                _gradeAtacado.GRADE_XP = _novo.GRADE_XP;
                _gradeAtacado.GRADE_PP = _novo.GRADE_PP;
                _gradeAtacado.GRADE_P = _novo.GRADE_P;
                _gradeAtacado.GRADE_M = _novo.GRADE_M;
                _gradeAtacado.GRADE_G = _novo.GRADE_G;
                _gradeAtacado.GRADE_GG = _novo.GRADE_GG;
                _gradeAtacado.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _gradeAtacado.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirGradeAtacado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_GRADE_ATACADO _gradeAtacado = ObterGradeAtacado(codigo);

            if (_gradeAtacado != null)
            {
                try
                {
                    db.PROD_HB_GRADE_ATACADOs.DeleteOnSubmit(_gradeAtacado);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - PROD_HB_GRADE_ATACADO


        //SP_GERAR_PEDIDO_COMPRAResult
        public SP_GERAR_PEDIDO_COMPRAResult GerarPedidoCompra(int prod_hb, string produto, string corProduto, char atacado, string aprovadoPor)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from i in db.SP_GERAR_PEDIDO_COMPRA(prod_hb, produto, corProduto, atacado, aprovadoPor) select i).SingleOrDefault();
        }
        //FIM SP_GERAR_PEDIDO_COMPRAResult

        //PROD_HB_AVIAMENTO_ORCAMENTO
        public List<PROD_HB_AVIAMENTO_ORCAMENTO> ObterAviamentoOrcamento()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AVIAMENTO_ORCAMENTOs orderby m.FORNECEDOR select m).ToList();
        }
        public PROD_HB_AVIAMENTO_ORCAMENTO ObterAviamentoOrcamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AVIAMENTO_ORCAMENTOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirAviamentoOrcamento(PROD_HB_AVIAMENTO_ORCAMENTO _aviamentoOrcamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_AVIAMENTO_ORCAMENTOs.InsertOnSubmit(_aviamentoOrcamento);
                db.SubmitChanges();

                return _aviamentoOrcamento.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CancelarAviamentoOrcamento(PROD_HB_AVIAMENTO_ORCAMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AVIAMENTO_ORCAMENTO _aviamentoOrcamento = ObterAviamentoOrcamento(_novo.CODIGO);

            if (_aviamentoOrcamento != null)
            {
                _aviamentoOrcamento.CODIGO = _novo.CODIGO;
                _aviamentoOrcamento.USUARIO_CANCELAMENTO = _novo.USUARIO_CANCELAMENTO;
                _aviamentoOrcamento.DATA_CANCELAMENTO = _novo.DATA_CANCELAMENTO;

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
        public void ComprarAviamentoOrcamento(PROD_HB_AVIAMENTO_ORCAMENTO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AVIAMENTO_ORCAMENTO _aviamentoOrcamento = ObterAviamentoOrcamento(_novo.CODIGO);

            if (_aviamentoOrcamento != null)
            {
                _aviamentoOrcamento.CODIGO = _novo.CODIGO;
                _aviamentoOrcamento.PRECO_REAL = _novo.PRECO_REAL;
                _aviamentoOrcamento.DESCONTO_REAL = _novo.DESCONTO_REAL;
                _aviamentoOrcamento.USUARIO_COMPRA = _novo.USUARIO_COMPRA;
                _aviamentoOrcamento.DATA_COMPRA = _novo.DATA_COMPRA;

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
        public void ExcluirAviamentoOrcamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AVIAMENTO_ORCAMENTO _aviamentoOrcamento = ObterAviamentoOrcamento(codigo);

            if (_aviamentoOrcamento != null)
            {
                try
                {
                    db.PROD_HB_AVIAMENTO_ORCAMENTOs.DeleteOnSubmit(_aviamentoOrcamento);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM PROD_HB_AVIAMENTO_ORCAMENTO

        //PROD_HB_AVIAMENTO_ORCAMENTO_AVI
        public List<PROD_HB_AVIAMENTO_ORCAMENTO_AVI> ObterRelacaoAviamentoOrcamento()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AVIAMENTO_ORCAMENTO_AVIs select m).ToList();
        }
        public PROD_HB_AVIAMENTO_ORCAMENTO_AVI ObterRelacaoAviamentoOrcamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AVIAMENTO_ORCAMENTO_AVIs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public List<PROD_HB_AVIAMENTO_ORCAMENTO_AVI> ObterAviamentoComprado()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AVIAMENTO_ORCAMENTO_AVIs
                    join j in db.PROD_HB_AVIAMENTO_ORCAMENTOs
                    on m.PROD_HB_AVIAMENTO_ORCAMENTO equals j.CODIGO
                    where j.DATA_COMPRA != null
                    select m).Distinct().ToList();
        }
        public int RelacionarAviamentoOrcamento(PROD_HB_AVIAMENTO_ORCAMENTO_AVI _aviamentoOrcamento)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_AVIAMENTO_ORCAMENTO_AVIs.InsertOnSubmit(_aviamentoOrcamento);
                db.SubmitChanges();

                return _aviamentoOrcamento.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirRelacaoAviamentoOrcamento(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AVIAMENTO_ORCAMENTO_AVI _aviamentoOrcamento = ObterRelacaoAviamentoOrcamento(codigo);

            if (_aviamentoOrcamento != null)
            {
                try
                {
                    db.PROD_HB_AVIAMENTO_ORCAMENTO_AVIs.DeleteOnSubmit(_aviamentoOrcamento);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM PROD_HB_AVIAMENTO_ORCAMENTO_AVI

        //PROD_HB_CUSTO_SERVICO
        public List<PROD_HB_CUSTO_SERVICO> ObterCustoServicoConexao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (from m in db.PROD_HB_CUSTO_SERVICOs
                    orderby m.PRODUTO
                    select m).ToList();
        }
        public List<PROD_HB_CUSTO_SERVICO> ObterCustoServico()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_CUSTO_SERVICOs
                    orderby m.PRODUTO
                    select m).ToList();
        }

        public List<PROD_HB_CUSTO_SERVICO> ObterCustoServico(string produto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_CUSTO_SERVICOs
                    where m.PRODUTO.Trim() == produto.Trim()
                    && m.MOSTRUARIO == mostruario
                    select m).ToList();
        }
        public PROD_HB_CUSTO_SERVICO ObterCustoServico(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_CUSTO_SERVICOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirCustoServico(PROD_HB_CUSTO_SERVICO _custoServico)
        {

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_CUSTO_SERVICOs.InsertOnSubmit(_custoServico);
                db.SubmitChanges();

                return _custoServico.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCustoServico(PROD_HB_CUSTO_SERVICO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            PROD_HB_CUSTO_SERVICO _custoServico = ObterCustoServico(_novo.CODIGO);

            if (_custoServico != null)
            {
                _custoServico.CODIGO = _novo.CODIGO;
                _custoServico.PRODUTO = _novo.PRODUTO;
                _custoServico.FORNECEDOR = _novo.FORNECEDOR;
                _custoServico.SERVICO = _novo.SERVICO;
                _custoServico.CUSTO = _novo.CUSTO;
                _custoServico.CUSTO_PECA = _novo.CUSTO_PECA;
                _custoServico.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _custoServico.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirCustoServico(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_CUSTO_SERVICO _custoServico = ObterCustoServico(codigo);

            if (_custoServico != null)
            {
                try
                {
                    db.PROD_HB_CUSTO_SERVICOs.DeleteOnSubmit(_custoServico);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM PROD_HB_CUSTO_SERVICO

        //SP_OBTER_AVIAMENTO_ORCADOResult
        public List<SP_OBTER_AVIAMENTO_ORCADOResult> ObterAviamentoOrcado(string codigoOrcamento, string prod_aviamento, string fornecedor, string numero_hb, string colecao)
        {
            int? aviamento = null;
            int? hb = null;
            int? codigo = null;

            if (codigoOrcamento.Trim() != "")
                codigo = Convert.ToInt32(codigoOrcamento);

            if (prod_aviamento != "0")
                aviamento = Convert.ToInt32(prod_aviamento);

            if (numero_hb.Trim() != "")
                hb = Convert.ToInt32(numero_hb);

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_AVIAMENTO_ORCADO(codigo, aviamento, fornecedor, hb, colecao) select m).ToList();
        }
        // FIM SP_OBTER_AVIAMENTO_ORCADOResult

        //SP_OBTER_AVIAMENTO_ESTOQUEResult
        public List<SP_OBTER_AVIAMENTO_ESTOQUEResult> ObterAviamentoEstoque(string prod_aviamento)
        {
            int? aviamento = null;

            if (prod_aviamento != "0")
                aviamento = Convert.ToInt32(prod_aviamento);

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_AVIAMENTO_ESTOQUE(aviamento) select m).ToList();
        }
        // FIM SP_OBTER_AVIAMENTO_ESTOQUEResult

        //SP_OBTER_HB_AVIAMENTO_CUSTOResult
        public List<SP_OBTER_HB_AVIAMENTO_CUSTOResult> ObterAviamentoCusto(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_HB_AVIAMENTO_CUSTO(prod_hb) select m).ToList();
        }
        // FIM SP_OBTER_HB_AVIAMENTO_CUSTOResult

        //PRODUTO_CUSTO_SIMULACAO
        public List<PRODUTO_CUSTO_SIMULACAO> ObterCustoSimulacao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    orderby m.PRODUTO, m.MARKUP
                    select m).ToList();
        }
        public List<PRODUTO_CUSTO_SIMULACAO> ObterCustoSimulacao(string produto, char mostruario, char finalizado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    where m.PRODUTO == produto && m.MOSTRUARIO == mostruario && m.FINALIZADO == finalizado
                    select m).ToList();
        }
        public List<PRODUTO_CUSTO_SIMULACAO> ObterCustoSimulacaoPreco(string produto, char simulacao, char tipoCusto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    where m.PRODUTO == produto && m.SIMULACAO == simulacao && m.CUSTO_MOSTRUARIO == tipoCusto
                    select m).ToList();
        }
        public List<PRODUTO_CUSTO_SIMULACAO> ObterCustoSimulacao(string produto, char mostruario, char simulacao, char tipoCusto, char finalizado)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    where m.PRODUTO == produto && m.MOSTRUARIO == mostruario && m.SIMULACAO == simulacao && m.CUSTO_MOSTRUARIO == tipoCusto && m.FINALIZADO == finalizado
                    select m).ToList();
        }
        public List<PRODUTO_CUSTO_SIMULACAO> ObterCustoSimulacao(string produto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    where m.PRODUTO.Trim() == produto.Trim() && m.MOSTRUARIO == mostruario
                    orderby m.PRODUTO, m.MARKUP
                    select m).ToList();
        }

        public List<PRODUTO_CUSTO_SIMULACAO> ObterCustoSimulacaoPrecoAprovado(string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    where m.PRODUTO == produto
                        && m.SIMULACAO == 'N' && m.CUSTO_MOSTRUARIO == 'N'
                    orderby m.DATA_INCLUSAO
                    select m).ToList();
        }

        public PRODUTO_CUSTO_SIMULACAO ObterCustoSimulacaoPrecoAprovado(string produto, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs
                    where m.PRODUTO == produto
                        && m.SIMULACAO == 'N' && m.CUSTO_MOSTRUARIO == 'N'
                        && m.MOSTRUARIO == mostruario
                    select m).SingleOrDefault();
        }


        public PRODUTO_CUSTO_SIMULACAO ObterCustoSimulacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public SP_OBTER_CUSTO_SIMULACAOResult ObterCustoSimulacao(string codigoProduto, char mostruario, decimal markup, char tipoCusto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_CUSTO_SIMULACAO(codigoProduto, mostruario, markup, tipoCusto) select m).FirstOrDefault();
        }
        public int InserirCustoSimulacao(PRODUTO_CUSTO_SIMULACAO _custoSimulacao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PRODUTO_CUSTO_SIMULACAOs.InsertOnSubmit(_custoSimulacao);
                db.SubmitChanges();

                return _custoSimulacao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCustoSimulacao(PRODUTO_CUSTO_SIMULACAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PRODUTO_CUSTO_SIMULACAO _custoSimulacao = ObterCustoSimulacao(_novo.CODIGO);

            if (_custoSimulacao != null)
            {
                _custoSimulacao.CODIGO = _novo.CODIGO;
                _custoSimulacao.PRODUTO = _novo.PRODUTO;
                _custoSimulacao.PRECO_PRODUTO = _novo.PRECO_PRODUTO;
                _custoSimulacao.MARKUP = _novo.MARKUP;
                _custoSimulacao.TT_PORC = _novo.TT_PORC;
                _custoSimulacao.TT_IMPOSTO = _novo.TT_IMPOSTO;
                _custoSimulacao.ICMS = _novo.ICMS;
                _custoSimulacao.CUSTO_COM_IMPOSTO = _novo.CUSTO_COM_IMPOSTO;
                _custoSimulacao.LUCRO_PORC = _novo.LUCRO_PORC;
                _custoSimulacao.SIMULACAO = _novo.SIMULACAO;
                _custoSimulacao.MOSTRUARIO = _novo.MOSTRUARIO;
                _custoSimulacao.CUSTO_MOSTRUARIO = _novo.CUSTO_MOSTRUARIO;
                _custoSimulacao.FINALIZADO = _novo.FINALIZADO;
                _custoSimulacao.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _custoSimulacao.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirCustoSimulacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PRODUTO_CUSTO_SIMULACAO _custoSimulacao = ObterCustoSimulacao(codigo);

            if (_custoSimulacao != null)
            {
                try
                {
                    db.PRODUTO_CUSTO_SIMULACAOs.DeleteOnSubmit(_custoSimulacao);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        // FIM PRODUTO_CUSTO_SIMULACAO

        //PRODUTO_CUSTO_SIMULACAO_PORC
        public PRODUTO_CUSTO_SIMULACAO_PORC ObterCustoSimulacaoValoresCalculo()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_SIMULACAO_PORCs where m.CODIGO == 1 select m).SingleOrDefault();
        }
        //FIM PRODUTO_CUSTO_SIMULACAO_PORC

        //PRODUTO_CUSTO_COD_ORIGEM
        public List<PRODUTO_CUSTO_COD_ORIGEM> ObterCustoCodigoOrigem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_COD_ORIGEMs
                    orderby m.COD_CUSTO_ORIGEM
                    select m).ToList();
        }
        public PRODUTO_CUSTO_COD_ORIGEM ObterCustoCodigoOrigem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_COD_ORIGEMs
                    where m.COD_CUSTO_ORIGEM == codigo
                    orderby m.COD_CUSTO_ORIGEM
                    select m).SingleOrDefault();
        }
        //FIM PRODUTO_CUSTO_COD_ORIGEM

        //PRODUTO_CUSTO_ORIGEM
        public List<PRODUTO_CUSTO_ORIGEM> ObterCustoOrigem()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_ORIGEMs
                    orderby m.PRODUTO, m.COD_CUSTO_ORIGEM
                    select m).ToList();
        }
        public List<PRODUTO_CUSTO_ORIGEM> ObterCustoOrigem(string produto, char mostruario, char tipoCusto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_ORIGEMs
                    where m.PRODUTO.Trim() == produto
                    && m.MOSTRUARIO == mostruario
                    && m.CUSTO_MOSTRUARIO == tipoCusto
                    select m).ToList();
        }
        public PRODUTO_CUSTO_ORIGEM ObterCustoOrigem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PRODUTO_CUSTO_ORIGEMs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirCustoOrigem(PRODUTO_CUSTO_ORIGEM _custoOrigem)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PRODUTO_CUSTO_ORIGEMs.InsertOnSubmit(_custoOrigem);
                db.SubmitChanges();

                return _custoOrigem.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCustoOrigem(PRODUTO_CUSTO_ORIGEM _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PRODUTO_CUSTO_ORIGEM _custoOrigem = ObterCustoOrigem(_novo.CODIGO);

            if (_custoOrigem != null)
            {
                _custoOrigem.CODIGO = _novo.CODIGO;
                _custoOrigem.CUSTO_TOTAL = _novo.CUSTO_TOTAL;

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
        public void ExcluirCustoOrigem(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PRODUTO_CUSTO_ORIGEM _custoOrigem = ObterCustoOrigem(codigo);

            if (_custoOrigem != null)
            {
                try
                {
                    db.PRODUTO_CUSTO_ORIGEMs.DeleteOnSubmit(_custoOrigem);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        //SP_OBTER_CUSTO_ORIGEMResult
        public SP_OBTER_CUSTO_ORIGEMResult ObterCustoOrigem(string codigoProduto, string colecao, int codOrigem, char mostruario, char tipoCusto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_CUSTO_ORIGEM(codigoProduto, colecao, codOrigem, mostruario, tipoCusto) select m).FirstOrDefault();
        }
        //FIM PRODUTO_CUSTO_ORIGEM

        //PROD_HB_AMPLIACAO
        public List<PROD_HB_AMPLIACAO> ObterAmpliacao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AMPLIACAOs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public PROD_HB_AMPLIACAO ObterAmpliacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AMPLIACAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirAmpliacao(PROD_HB_AMPLIACAO _ampliacao)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_AMPLIACAOs.InsertOnSubmit(_ampliacao);
                db.SubmitChanges();

                return _ampliacao.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAmpliacao(PROD_HB_AMPLIACAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AMPLIACAO _ampliacao = ObterAmpliacao(_novo.CODIGO);

            if (_ampliacao != null)
            {
                _ampliacao.CODIGO = _novo.CODIGO;
                _ampliacao.DESCRICAO = _novo.DESCRICAO;
                _ampliacao.STATUS = _novo.STATUS;

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
        public void ExcluirAmpliacao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AMPLIACAO _ampliacao = ObterAmpliacao(codigo);

            if (_ampliacao != null)
            {
                try
                {
                    db.PROD_HB_AMPLIACAOs.DeleteOnSubmit(_ampliacao);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM PROD_HB_AMPLIACAO

        //PROD_HB_AMPLIACAO_MEDIDA
        public List<PROD_HB_AMPLIACAO_MEDIDA> ObterAmpliacaoMedidasHB()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AMPLIACAO_MEDIDAs
                    orderby m.PROD_HB_AMPLIACAO1.DESCRICAO
                    select m).ToList();
        }
        public PROD_HB_AMPLIACAO_MEDIDA ObterAmpliacaoMedidasHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_AMPLIACAO_MEDIDAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirAmpliacaoMedidasHB(PROD_HB_AMPLIACAO_MEDIDA _ampliacaoMedidas)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_AMPLIACAO_MEDIDAs.InsertOnSubmit(_ampliacaoMedidas);
                db.SubmitChanges();

                return _ampliacaoMedidas.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarAmpliacaoMedidasHB(PROD_HB_AMPLIACAO_MEDIDA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AMPLIACAO_MEDIDA _ampliacaoMedidas = ObterAmpliacaoMedidasHB(_novo.CODIGO);

            if (_ampliacaoMedidas != null)
            {
                _ampliacaoMedidas.CODIGO = _novo.CODIGO;

                _ampliacaoMedidas.PROD_HB = _novo.PROD_HB;
                _ampliacaoMedidas.PROD_HB_AMPLIACAO = _novo.PROD_HB_AMPLIACAO;
                _ampliacaoMedidas.LOCAL = _novo.LOCAL;
                _ampliacaoMedidas.QTDE = _novo.QTDE;
                _ampliacaoMedidas.COMPRIMENTO = _novo.COMPRIMENTO;
                _ampliacaoMedidas.LARGURA = _novo.LARGURA;
                _ampliacaoMedidas.DESCRICAO = _novo.DESCRICAO;
                _ampliacaoMedidas.GRADE_EXP = _novo.GRADE_EXP;
                _ampliacaoMedidas.GRADE_XP = _novo.GRADE_XP;
                _ampliacaoMedidas.GRADE_PP = _novo.GRADE_PP;
                _ampliacaoMedidas.GRADE_P = _novo.GRADE_P;
                _ampliacaoMedidas.GRADE_M = _novo.GRADE_M;
                _ampliacaoMedidas.GRADE_G = _novo.GRADE_G;
                _ampliacaoMedidas.GRADE_GG = _novo.GRADE_GG;
                _ampliacaoMedidas.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;
                _ampliacaoMedidas.DATA_INCLUSAO = _novo.DATA_INCLUSAO;

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
        public void ExcluirAmpliacaoMedidasHB(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_AMPLIACAO_MEDIDA _ampliacaoMedidas = ObterAmpliacaoMedidasHB(codigo);

            if (_ampliacaoMedidas != null)
            {
                try
                {
                    db.PROD_HB_AMPLIACAO_MEDIDAs.DeleteOnSubmit(_ampliacaoMedidas);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM PROD_HB_AMPLIACAO_MEDIDA

        //SP_OBTER_PRODUTO_ACORTARResult
        public List<SP_OBTER_PRODUTO_ACORTARResult> ObterProdutoACortar(string colecao, int? origem, string grupo, string modelo, string nome)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_PRODUTO_ACORTAR(colecao, origem, grupo, modelo, nome) select m).ToList();
        }
        // FIM SP_OBTER_PRODUTO_ACORTARResult

        //PROD_SERVICO
        public List<PROD_SERVICO> ObterServicoProducao()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_SERVICOs
                    orderby m.DESCRICAO
                    select m).ToList();
        }
        public PROD_SERVICO ObterServicoProducao(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_SERVICOs
                    where m.CODIGO == codigo
                    orderby m.DESCRICAO
                    select m).SingleOrDefault();
        }
        //FIM PROD_SERVICO

        //PROD_GRADE
        public List<PROD_GRADE> ObterGradeNome()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_GRADEs
                    orderby m.GRADE
                    select m).ToList();
        }
        public PROD_GRADE ObterGradeNome(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_GRADEs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirGradeNome(PROD_GRADE _grade)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_GRADEs.InsertOnSubmit(_grade);
                db.SubmitChanges();

                return _grade.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarGradeNome(PROD_GRADE _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_GRADE _grade = ObterGradeNome(_novo.CODIGO);

            if (_grade != null)
            {
                _grade.CODIGO = _novo.CODIGO;
                _grade.GRADE = _novo.GRADE;
                _grade.GRADE_EXP = _novo.GRADE_EXP;
                _grade.GRADE_XP = _novo.GRADE_XP;
                _grade.GRADE_PP = _novo.GRADE_PP;
                _grade.GRADE_P = _novo.GRADE_P;
                _grade.GRADE_M = _novo.GRADE_M;
                _grade.GRADE_G = _novo.GRADE_G;
                _grade.GRADE_GG = _novo.GRADE_GG;
                _grade.STATUS = _grade.STATUS;

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
        public void ExcluirGradeNome(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_GRADE _grade = ObterGradeNome(codigo);

            if (_grade != null)
            {
                try
                {
                    db.PROD_GRADEs.DeleteOnSubmit(_grade);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - PROD_GRADE

        //SP_OBTER_PRODUTO_SEM_CUSTOResult
        public List<SP_OBTER_PRODUTO_SEM_CUSTOResult> ObterProdutoSemCusto(string colecao, string produto, string nome, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_PRODUTO_SEM_CUSTO(colecao, produto, nome, mostruario) select m).ToList();
        }
        // FIM SP_OBTER_PRODUTO_SEM_CUSTOResult

        //SP_OBTER_COMPOSICAO_HBResult
        public List<SP_OBTER_COMPOSICAO_HBResult> ObterListaComposicaoHB(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_COMPOSICAO_HB(prod_hb) select m).ToList();
        }
        // FIM SP_OBTER_COMPOSICAO_HBResult

        public List<SP_OBTER_COMPOSICAO_HB_AVIAMENTOResult> ObterListaComposicaoHBAviamento(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_COMPOSICAO_HB_AVIAMENTO(prod_hb) select m).ToList();
        }
        // FIM SP_OBTER_COMPOSICAO_HB_AVIAMENTOResult

        //SP_OBTER_PRODUTO_SIMULADOResult
        public List<SP_OBTER_PRODUTO_SIMULADOResult> ObterProdutoCustoSimulado(string colecao, string produto, string nome, char mostruario, char finalizado, string tipoCusto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;
            return (from m in db.SP_OBTER_PRODUTO_SIMULADO(colecao, produto, nome, mostruario, finalizado, tipoCusto) select m).ToList();
        }
        // FIM SP_OBTER_PRODUTO_SIMULADOResult

        //PROD_HB_MATERIAL_EXTRA
        public List<PROD_HB_MATERIAL_EXTRA> ObterMaterialExtra()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_MATERIAL_EXTRAs
                    orderby m.FORNECEDOR, m.MATERIAL
                    select m).ToList();
        }
        public List<PROD_HB_MATERIAL_EXTRA> ObterMaterialExtra(int prod_hb, char tipoMaterial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_MATERIAL_EXTRAs
                    where m.PROD_HB == prod_hb && m.TIPO_MATERIAL == tipoMaterial
                    orderby m.FORNECEDOR, m.MATERIAL
                    select m).ToList();
        }
        public PROD_HB_MATERIAL_EXTRA ObterMaterialExtra(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_MATERIAL_EXTRAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirMaterialExtra(PROD_HB_MATERIAL_EXTRA _matExtra)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_MATERIAL_EXTRAs.InsertOnSubmit(_matExtra);
                db.SubmitChanges();

                return _matExtra.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialExtra(PROD_HB_MATERIAL_EXTRA _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_MATERIAL_EXTRA _matExtra = ObterMaterialExtra(_novo.CODIGO);

            if (_matExtra != null)
            {
                _matExtra.CODIGO = _novo.CODIGO;
                _matExtra.PROD_HB = _novo.PROD_HB;
                _matExtra.FORNECEDOR = _novo.FORNECEDOR;
                _matExtra.MATERIAL = _novo.MATERIAL;
                _matExtra.ICMS = _novo.ICMS;
                _matExtra.PRECO = _novo.PRECO;
                _matExtra.CONSUMO_CUSTO = _novo.CONSUMO_CUSTO;
                _matExtra.CONSUMO_PRECO = _novo.CONSUMO_PRECO;
                _matExtra.TIPO_MATERIAL = _novo.TIPO_MATERIAL;
                _matExtra.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _matExtra.USUARIO_INCLUSAO = _novo.USUARIO_INCLUSAO;

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
        public void ExcluirMaterialExtra(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_MATERIAL_EXTRA _matExtra = ObterMaterialExtra(codigo);

            if (_matExtra != null)
            {
                try
                {
                    db.PROD_HB_MATERIAL_EXTRAs.DeleteOnSubmit(_matExtra);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public List<PRODUTO_MATERIAL_EXTRA> ObterMaterialExtraTerceiro()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_MATERIAL_EXTRAs
                    orderby m.MATERIAL
                    select m).ToList();
        }
        public List<PRODUTO_MATERIAL_EXTRA> ObterMaterialExtraTerceiro(string produto, char mostruario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_MATERIAL_EXTRAs
                    where m.PRODUTO == produto && m.MOSTRUARIO == mostruario
                    orderby m.MATERIAL
                    select m).ToList();
        }
        public PRODUTO_MATERIAL_EXTRA ObterMaterialExtraTerceiro(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_MATERIAL_EXTRAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirMaterialExtraTerceiro(PRODUTO_MATERIAL_EXTRA _matExtra)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.PRODUTO_MATERIAL_EXTRAs.InsertOnSubmit(_matExtra);
                dbIntra.SubmitChanges();

                return _matExtra.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarMaterialExtraTerceiro(PRODUTO_MATERIAL_EXTRA _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            PRODUTO_MATERIAL_EXTRA _matExtra = ObterMaterialExtraTerceiro(_novo.CODIGO);

            if (_matExtra != null)
            {
                _matExtra.CODIGO = _novo.CODIGO;
                _matExtra.PRODUTO = _novo.PRODUTO;
                _matExtra.MOSTRUARIO = _novo.MOSTRUARIO;
                _matExtra.MATERIAL = _novo.MATERIAL;
                _matExtra.PRECO = _novo.PRECO;
                _matExtra.CONSUMO = _novo.CONSUMO;


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
        public void ExcluirMaterialExtraTerceiro(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            PRODUTO_MATERIAL_EXTRA _matExtra = ObterMaterialExtraTerceiro(codigo);

            if (_matExtra != null)
            {
                try
                {
                    dbIntra.PRODUTO_MATERIAL_EXTRAs.DeleteOnSubmit(_matExtra);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - PROD_HB_MATERIAL_EXTRA

        //SP_CALCULAR_GRADE_REALResult
        public SP_CALCULAR_GRADE_REALResult CalcularGradeReal(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_CALCULAR_GRADE_REAL(prod_hb) select m).SingleOrDefault();
        }
        // FIM SP_CALCULAR_GRADE_REALResult

        //SP_OBTER_PRODUTO_PRECO_APROVADOResult
        public List<SP_OBTER_PRODUTO_PRECO_APROVADOResult> ObterProdutoPrecoAprovado(string colecao, string produto, string grupo, string griffe, string grupoTecido, string subGrupoTecido, char? tipo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_PRODUTO_PRECO_APROVADO(colecao, produto, grupo, griffe, grupoTecido, subGrupoTecido, tipo) select m).ToList();
        }
        // FIM SP_OBTER_PRODUTO_PRECO_APROVADOResult

        //SP_OBTER_PRODUTO_PRECO_APROVADOResult
        public List<SP_OBTER_HB_SEM_ETIQUETAResult> ObterHBSemEtiquetaBaixada()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_HB_SEM_ETIQUETA() select m).ToList();
        }
        // FIM SP_OBTER_PRODUTO_PRECO_APROVADOResult


        public List<SP_OBTER_PRODUTO_SEMTAGAVIResult> ObterObterProdutoSemTAGAVI()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_PRODUTO_SEMTAGAVI() select m).ToList();
        }


        //SP_OBTER_GRADE_REAL_PRODUTOResult
        public SP_OBTER_GRADE_REAL_PRODUTOResult ObterGradeRealProduto(string colecao, string produto, string cor, char mostruario)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_GRADE_REAL_PRODUTO(colecao, produto, cor, mostruario) select m).SingleOrDefault();
        }
        // FIM SP_OBTER_GRADE_REAL_PRODUTOResult

        //PROD_HB_TECIDO_SIMULACAO
        public List<PROD_HB_TECIDO_SIMULACAO> ObterTecidoEstoqueSimulado()
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_TECIDO_SIMULACAOs
                    select m).ToList();
        }
        public List<PROD_HB_TECIDO_SIMULACAO> ObterTecidoEstoqueSimuladoPedido(int numeroPedido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_TECIDO_SIMULACAOs
                    where m.NUMERO_PEDIDO == numeroPedido
                    select m).ToList();
        }
        public PROD_HB_TECIDO_SIMULACAO ObterTecidoEstoqueSimulado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_TECIDO_SIMULACAOs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirTecidoEstoqueSimulado(PROD_HB_TECIDO_SIMULACAO _estTecido)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_TECIDO_SIMULACAOs.InsertOnSubmit(_estTecido);
                db.SubmitChanges();

                return _estTecido.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarTecidoEstoqueSimulado(PROD_HB_TECIDO_SIMULACAO _novo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_TECIDO_SIMULACAO _estTecido = ObterTecidoEstoqueSimulado(_novo.CODIGO);

            if (_estTecido != null)
            {
                _estTecido.CODIGO = _novo.CODIGO;
                _estTecido.NUMERO_PEDIDO = _novo.NUMERO_PEDIDO;
                _estTecido.PRODUTO = _novo.PRODUTO;
                _estTecido.GRADE_TOTAL = _novo.GRADE_TOTAL;
                _estTecido.GASTO_FOLHA = _novo.GASTO_FOLHA;
                _estTecido.DATA_INCLUSAO = _novo.DATA_INCLUSAO;
                _estTecido.DATA_BAIXA = _novo.DATA_BAIXA;


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
        public void ExcluirTecidoEstoqueSimulado(int codigo)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            PROD_HB_TECIDO_SIMULACAO _estTecido = ObterTecidoEstoqueSimulado(codigo);

            if (_estTecido != null)
            {
                try
                {
                    db.PROD_HB_TECIDO_SIMULACAOs.DeleteOnSubmit(_estTecido);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - PROD_HB_TECIDO_SIMULACAO

        //SP_OBTER_HB_PEDIDOResult
        public List<SP_OBTER_HB_PEDIDOResult> ObterEstoqueCartelinha(int numeroPedido, string grupo, string subGrupo, string cor, string corFornecedor)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_HB_PEDIDO(numeroPedido, grupo, subGrupo, cor, corFornecedor) select m).ToList();
        }
        // FIM SP_OBTER_HB_PEDIDOResult

        //SP_OBTER_PRODUTO_PRECO_IMPORTADOResult
        public List<SP_OBTER_PRODUTO_PRECO_IMPORTADOResult> ObterProdutoImportadoPreco(string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_PRODUTO_PRECO_IMPORTADO(produto) select m).ToList();
        }
        // FIM SP_OBTER_PRODUTO_PRECO_IMPORTADOResult

        //PROD_HB_ETIQUETA_COMP
        public List<PROD_HB_ETIQUETA_COMP> ObterEtiquetaComposicao(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.PROD_HB_ETIQUETA_COMPs
                    where m.PROD_HB == prod_hb
                    orderby m.CODIGO
                    select m).ToList();
        }
        public int InserirEtiquetaComposicao(PROD_HB_ETIQUETA_COMP _etiquetaComp)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            try
            {
                db.PROD_HB_ETIQUETA_COMPs.InsertOnSubmit(_etiquetaComp);
                db.SubmitChanges();

                return _etiquetaComp.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ExcluirEtiquetaComposicao(int prod_hb)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            List<PROD_HB_ETIQUETA_COMP> _etiquetaComp = ObterEtiquetaComposicao(prod_hb);

            if (_etiquetaComp != null)
            {
                try
                {
                    foreach (var e in _etiquetaComp)
                        db.PROD_HB_ETIQUETA_COMPs.DeleteOnSubmit(e);
                    db.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //FIM - PROD_HB_ETIQUETA_COMP

        public List<SP_OBTER_DEFINICAO_GRADE_ATACADOResult> ObterDefinicaoGradeAtacado(string colecao, int? hb, string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_DEFINICAO_GRADE_ATACADO(colecao, hb, produto) select m).ToList();
        }

        public List<SP_OBTER_DEFINICAO_GRADE_ATACADO_IMPResult> ObterDefinicaoGradeAtacadoImpressao(string colecao, int? hb, string produto)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);
            return (from m in db.SP_OBTER_DEFINICAO_GRADE_ATACADO_IMP(colecao, hb, produto) select m).ToList();
        }

        //Rotas
        public PROD_HB_ROTA ObterRota(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_ROTAs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public PROD_HB_ROTA ObterRotaHB(int prod_hb)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_ROTAs where m.PROD_HB == prod_hb select m).SingleOrDefault();
        }
        public PROD_HB_ROTA ObterRotaOP(String ordemProducao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PROD_HB_ROTAs where m.ORDEM_PRODUCAO == ordemProducao select m).SingleOrDefault();
        }
        public void InserirRota(PROD_HB_ROTA _Rota)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.PROD_HB_ROTAs.InsertOnSubmit(_Rota);
                dbIntra.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarRota(PROD_HB_ROTA rota)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            PROD_HB_ROTA _rota = ObterRota(rota.CODIGO);

            if (_rota != null)
            {
                _rota.ORDEM_PRODUCAO = rota.ORDEM_PRODUCAO;
                _rota.PROD_HB = rota.PROD_HB;
                _rota.USUARIO = rota.USUARIO;
                _rota.DATA_INCLUSAO = rota.DATA_INCLUSAO;
                _rota.FACCAO = rota.FACCAO;
                _rota.ESTAMPARIA = rota.ESTAMPARIA;
                _rota.LAVANDERIA = rota.LAVANDERIA;
                _rota.ACABAMENTO = rota.ACABAMENTO;

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
        public void ExcluirRota(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                PROD_HB_ROTA _Rota = ObterRota(codigo);
                if (_Rota != null)
                {
                    dbIntra.PROD_HB_ROTAs.DeleteOnSubmit(_Rota);
                    dbIntra.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PRODUTO_OPERACOES_ROTA> ObterRotaFases(string tabelaOperacoes)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.PRODUTO_OPERACOES_ROTAs where m.TABELA_OPERACOES == tabelaOperacoes select m).ToList();
        }
        public PRODUCAO_FASE ObterFase(string faseProducao)
        {
            db = new DCDataContext(Constante.ConnectionString);
            return (from m in db.PRODUCAO_FASEs where m.FASE_PRODUCAO == faseProducao select m).SingleOrDefault();
        }
        //FIM ROTAS

        public SP_OBTER_PRODUTO_ACABADO_PRECOResult ObterProdutoAcabadoTerceiroPreco(string produto, string nomeProduto, char mostruario)
        {
            dbLinx = new LINXDataContext(Constante.ConnectionString);
            return (from m in dbLinx.SP_OBTER_PRODUTO_ACABADO_PRECO(produto, nomeProduto, mostruario) select m).SingleOrDefault();
        }

        //PRODUTO_CUSTO_TERCEIRO
        public List<PRODUTO_CUSTO_TERCEIRO> ObterCustoTerceiro()
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_CUSTO_TERCEIROs
                    orderby m.PRODUTO, m.MOSTRUARIO
                    select m).ToList();
        }
        public PRODUTO_CUSTO_TERCEIRO ObterCustoTerceiro(string produto, char mostruario)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_CUSTO_TERCEIROs
                    where m.PRODUTO == produto && m.MOSTRUARIO == mostruario
                    select m).SingleOrDefault();
        }
        public PRODUTO_CUSTO_TERCEIRO ObterCustoTerceiro(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_CUSTO_TERCEIROs where m.CODIGO == codigo select m).SingleOrDefault();
        }
        public int InserirCustoTerceiro(PRODUTO_CUSTO_TERCEIRO custoTerceiro)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                dbIntra.PRODUTO_CUSTO_TERCEIROs.InsertOnSubmit(custoTerceiro);
                dbIntra.SubmitChanges();

                return custoTerceiro.CODIGO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarCustoTerceiro(PRODUTO_CUSTO_TERCEIRO _novo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            PRODUTO_CUSTO_TERCEIRO _custoTerceiro = ObterCustoTerceiro(_novo.CODIGO);

            if (_custoTerceiro != null)
            {
                _custoTerceiro.CODIGO = _novo.CODIGO;
                _custoTerceiro.PRODUTO = _novo.PRODUTO;
                _custoTerceiro.MOSTRUARIO = _novo.MOSTRUARIO;
                _custoTerceiro.CUSTO = _novo.CUSTO;
                _custoTerceiro.PRECO = _novo.PRECO;



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
        public void ExcluirCustoTerceiro(int codigo)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            PRODUTO_CUSTO_TERCEIRO _custoTerceiro = ObterCustoTerceiro(codigo);

            if (_custoTerceiro != null)
            {
                try
                {
                    dbIntra.PRODUTO_CUSTO_TERCEIROs.DeleteOnSubmit(_custoTerceiro);
                    dbIntra.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        //Fim - PRODUTO_CUSTO_TERCEIRO

        public SP_OBTER_HB_GASTO_MOSTRUARIOResult ObterHBGastoMostruario(string produto)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_HB_GASTO_MOSTRUARIO(produto) select m).SingleOrDefault();
        }

        public SP_OBTER_RENDIMENTO_TECIDO_KGResult ObterRendimentoTecidoKG(string numeroPedido)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.SP_OBTER_RENDIMENTO_TECIDO_KG(numeroPedido) select m).SingleOrDefault();
        }


        public PRODUTO_CUSTO_COLECAO_VAR ObterCustoVariavelPorColecao(string colecao)
        {
            dbIntra = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in dbIntra.PRODUTO_CUSTO_COLECAO_VARs where m.COLECAO == colecao select m).SingleOrDefault();
        }


    }
}

