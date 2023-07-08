using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DesempenhoController
    {
        BaseController baseController = new BaseController();

        DCDataContext db;
        LINXDataContext LINXdb;
        INTRADataContext Intradb;

        public List<Sp_Desempenho_VendaResult> BuscaDesempenhoVenda(string dataInicio, string dataFim, string filial)
        {
            List<Sp_Desempenho_VendaResult> desempenhoVenda = new List<Sp_Desempenho_VendaResult>();

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            List<Sp_Desempenho_VendaResult> listDesempenhoVenda = db.Sp_Desempenho_Venda(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), filial).ToList();

            foreach (Sp_Desempenho_VendaResult item in listDesempenhoVenda)
            {
                Sp_Desempenho_VendaResult desempenhoVendaItem = new Sp_Desempenho_VendaResult();

                //Só para tirar linhas com zero venda

                if (item != null && (item.Vendas_0 + item.Vendas_1 + item.Vendas_2 + item.Vendas_3 + item.Vendas_4 + item.Vendas_5) > 0)
                {
                    desempenhoVendaItem.Nome_Vendedor = item.Nome_Vendedor;
                    desempenhoVendaItem.Data_Ativacao = item.Data_Ativacao;
                    desempenhoVendaItem.Data_Desativacao = item.Data_Desativacao;
                    desempenhoVendaItem.Vendas_0 = item.Vendas_0;
                    desempenhoVendaItem.Vendas_1 = item.Vendas_1;
                    desempenhoVendaItem.Vendas_2 = item.Vendas_2;
                    desempenhoVendaItem.Vendas_3 = item.Vendas_3;
                    desempenhoVendaItem.Vendas_4 = item.Vendas_4;
                    desempenhoVendaItem.Vendas_5 = item.Vendas_5;
                    desempenhoVendaItem.Vendas_qt_total = item.Vendas_qt_total;
                    desempenhoVendaItem.Vendas_Consumidor = item.Vendas_Consumidor;
                    desempenhoVendaItem.Valor_Pago = item.Valor_Pago;
                    desempenhoVendaItem.Numero_Dias = item.Numero_Dias;

                    desempenhoVenda.Add(desempenhoVendaItem);
                }
            }

            return desempenhoVenda;
        }

        public List<Sp_Desempenho_Venda_LojaResult> BuscaDesempenhoFilial(string dataInicio, string dataFim, USUARIO usuario)
        {
            List<Sp_Desempenho_Venda_LojaResult> _list = new List<Sp_Desempenho_Venda_LojaResult>();

            List<FILIAI> _filiais = baseController.BuscaFiliais(usuario);
            var _dePara = baseController.BuscaFilialDePara();
            var _FilialAtiva = _filiais.Where(i => !_dePara.Any(j => j.DE == i.COD_FILIAL));
            _FilialAtiva = _FilialAtiva.Where(e => e.TIPO_FILIAL.ToUpper().Trim() == "LOJA").ToList();

            foreach (FILIAI f in _FilialAtiva)
            {
                Sp_Desempenho_Venda_LojaResult _desempenhoLoja = BuscaDesempenhoVendaLoja(dataInicio, dataFim, f.COD_FILIAL);
                if (_desempenhoLoja != null)
                {
                    _list.Add(_desempenhoLoja);
                }
            }

            return _list;
        }

        public Sp_Desempenho_Venda_LojaResult BuscaDesempenhoVendaLoja(string dataInicio, string dataFim, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            return (from dvl in db.Sp_Desempenho_Venda_Loja(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), filial) select dvl).SingleOrDefault();
        }

        public List<Vendas> BuscaVendasAtualAnterior(string dataInicioAtual, string dataFimAtual, string dataInicioAnterior, string dataFimAnterior, bool todasFiliais)
        {
            List<Vendas> listaVendas = new List<Vendas>();
            List<FILIAI> filiais;

            db = new DCDataContext(Constante.ConnectionString);
            db.CommandTimeout = 0;

            if (todasFiliais == true)
            {
                filiais = baseController.BuscaFiliaisAtivaInativa();
            }
            else
            {
                filiais = baseController.BuscaFiliaisAtivaInativa_Agora();
            }

            if (filiais != null)
            {
                //NAO MOSTRAR FILIAIS ANTIGAS
                List<FILIAIS_DE_PARA> dePara = new List<FILIAIS_DE_PARA>();
                dePara = baseController.BuscaFilialDePara();

                string filialAnterior = "0";

                foreach (FILIAI filial in filiais)
                {
                    Vendas venda = new Vendas();

                    venda.CodigoFilial = filial.COD_FILIAL.Trim();
                    venda.Filial = filial.FILIAL;

                    FILIAIS_DE_PARA filialDePara = baseController.BuscaFilialDePara(filial.COD_FILIAL);

                    if (filialDePara != null)
                        filialAnterior = filialDePara.DE;
                    else
                        filialAnterior = filial.COD_FILIAL;

                    SP_BUSCA_VENDAS_ATUAL_ANTERIORResult vendaFilial = (from bvaa in db.SP_BUSCA_VENDAS_ATUAL_ANTERIOR(filial.COD_FILIAL, filialAnterior, dataInicioAtual, dataFimAtual, baseController.AjustaData(dataInicioAnterior), baseController.AjustaData(dataFimAnterior)) select bvaa).SingleOrDefault();

                    if (filialDePara != null)
                    {
                        venda.ValorVendaAtual = Convert.ToDecimal(vendaFilial.venda_atual_filial_atual + vendaFilial.venda_atual_filial_anterior);
                        venda.ValorVendaAnterior = Convert.ToDecimal(vendaFilial.venda_anterior_filial_atual + vendaFilial.venda_anterior_filial_anterior);
                    }
                    else
                    {
                        venda.ValorVendaAtual = Convert.ToDecimal(vendaFilial.venda_atual_filial_atual);
                        venda.ValorVendaAnterior = Convert.ToDecimal(vendaFilial.venda_anterior_filial_atual);
                    }

                    //NAO MOSTRAR FILIAIS ANTIGAS
                    if (dePara.Where(p => p.DE.Trim() == filial.COD_FILIAL.Trim()).Count() <= 0)
                    {
                        //if (venda.ValorVendaAtual > 0)
                        listaVendas.Add(venda);
                    }
                }
            }

            return listaVendas;
        }


        public List<SP_OBTER_VENDA_COMPARAANTResult> ObterVendasComparaGrafico(string codigoFilial, string dataInicioAtual, string dataFimAtual, string dataInicioAnterior, string dataFimAnterior)
        {

            LINXdb = new LINXDataContext(Constante.ConnectionString);

            var dataIni1 = baseController.AjustaData(dataInicioAtual);
            var dataFim1 = baseController.AjustaData(dataFimAtual);
            var dataIni2 = baseController.AjustaData(dataInicioAnterior);
            var dataFim2 = baseController.AjustaData(dataFimAnterior);

            return (from dvg in LINXdb.SP_OBTER_VENDA_COMPARAANT(codigoFilial, "", dataIni1, dataFim1, dataIni2, dataFim2) select dvg).ToList();
        }



        public List<Sp_Desempenho_Venda_GraficoResult> BuscaDesempenhoVendaGrafico(string dataInicio, string dataFim, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from dvg in db.Sp_Desempenho_Venda_Grafico(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), filial) select dvg).ToList();
        }

        public List<Sp_Desempenho_Venda_LojaResult> BuscaDesempenhoVendaGrupo(string dataInicio, string dataFim, string filial)
        {
            List<Sp_Desempenho_Venda_LojaResult> desempenhoVendaLoja = new List<Sp_Desempenho_Venda_LojaResult>();

            db = new DCDataContext(Constante.ConnectionStringIntranet);
            db.CommandTimeout = 0;

            Sp_Busca_Supervisor_LojaResult supervisor = BuscaSupervisorLoja(filial);

            if (supervisor != null)
            {
                USUARIO usuario = baseController.BuscaUsuario(supervisor.codigo_usuario);

                if (usuario != null)
                {
                    List<FILIAI> filiais = baseController.BuscaFiliais(usuario);

                    if (filiais != null)
                    {
                        decimal valorPago = 0;
                        int vendasZero = 0;
                        int vendasUm = 0;
                        int vendasDois = 0;
                        int vendasTres = 0;
                        int vendasQuatro = 0;
                        int vendasCinco = 0;
                        int vendasConsumidor = 0;
                        int vendasTotal = 0;
                        int numeroDias = 0;
                        decimal qtMediaAtendido = 0;
                        int qtOcorrencias = 0;

                        Sp_Desempenho_Venda_LojaResult desempenhoVenda = null;

                        foreach (FILIAI xfilial in filiais)
                        {
                            db = new DCDataContext(Constante.ConnectionString);
                            db.CommandTimeout = 0;

                            desempenhoVenda = db.Sp_Desempenho_Venda_Loja(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), xfilial.COD_FILIAL).SingleOrDefault();

                            valorPago += desempenhoVenda.Valor_Pago;
                            vendasZero += desempenhoVenda.Vendas_0;
                            vendasUm += desempenhoVenda.Vendas_1;
                            vendasDois += desempenhoVenda.Vendas_2;
                            vendasTres += desempenhoVenda.Vendas_3;
                            vendasQuatro += desempenhoVenda.Vendas_4;
                            vendasCinco += desempenhoVenda.Vendas_5;
                            vendasConsumidor += desempenhoVenda.Vendas_Consumidor;
                            vendasTotal += desempenhoVenda.Vendas_qt_total;
                            numeroDias = desempenhoVenda.Numero_Dias;

                            if (numeroDias > 0)
                            {
                                qtMediaAtendido += (Convert.ToDecimal(desempenhoVenda.Vendas_qt_total) / Convert.ToDecimal(desempenhoVenda.Numero_Dias)) / Convert.ToDecimal(desempenhoVenda.Numero_Vendedor);
                                qtOcorrencias++;
                            }
                        }

                        desempenhoVenda.Valor_Pago = valorPago;
                        desempenhoVenda.Vendas_0 = vendasZero;
                        desempenhoVenda.Vendas_1 = vendasUm;
                        desempenhoVenda.Vendas_2 = vendasDois;
                        desempenhoVenda.Vendas_3 = vendasTres;
                        desempenhoVenda.Vendas_4 = vendasQuatro;
                        desempenhoVenda.Vendas_5 = vendasCinco;
                        desempenhoVenda.Vendas_Consumidor = vendasConsumidor;
                        desempenhoVenda.Vendas_qt_total = vendasTotal;
                        desempenhoVenda.Numero_Dias = numeroDias;
                        desempenhoVenda.Numero_Vendedor = Convert.ToInt32((qtMediaAtendido / Convert.ToDecimal(qtOcorrencias)) * 100);

                        desempenhoVendaLoja.Add(desempenhoVenda);

                        return desempenhoVendaLoja;
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        public List<Sp_Desempenho_Venda_LojaResult> BuscaDesempenhoVendaRede(string dataInicio, string dataFim)
        {
            List<Sp_Desempenho_Venda_LojaResult> desempenhoVendaLoja = new List<Sp_Desempenho_Venda_LojaResult>();

            List<FILIAI> filiais = baseController.BuscaFiliais();

            var _dePara = baseController.BuscaFilialDePara();
            var _FilialAtiva = filiais.Where(i => !_dePara.Any(j => j.DE == i.COD_FILIAL));
            _FilialAtiva = _FilialAtiva.Where(e => e.TIPO_FILIAL.ToUpper().Trim() == "LOJA").ToList();


            decimal valorPago = 0;
            int vendasZero = 0;
            int vendasUm = 0;
            int vendasDois = 0;
            int vendasTres = 0;
            int vendasQuatro = 0;
            int vendasCinco = 0;
            int vendasConsumidor = 0;
            int vendasTotal = 0;
            int numeroDias = 0;
            decimal qtMediaAtendido = 0;
            int qtOcorrencias = 0;

            Sp_Desempenho_Venda_LojaResult desempenhoVenda = null;

            foreach (FILIAI xfilial in _FilialAtiva)
            {
                db = new DCDataContext(Constante.ConnectionString);
                db.CommandTimeout = 0;

                desempenhoVenda = db.Sp_Desempenho_Venda_Loja(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim), xfilial.COD_FILIAL).SingleOrDefault();

                valorPago += desempenhoVenda.Valor_Pago;
                vendasZero += desempenhoVenda.Vendas_0;
                vendasUm += desempenhoVenda.Vendas_1;
                vendasDois += desempenhoVenda.Vendas_2;
                vendasTres += desempenhoVenda.Vendas_3;
                vendasQuatro += desempenhoVenda.Vendas_4;
                vendasCinco += desempenhoVenda.Vendas_5;
                vendasConsumidor += desempenhoVenda.Vendas_Consumidor;
                vendasTotal += desempenhoVenda.Vendas_qt_total;
                numeroDias = desempenhoVenda.Numero_Dias;

                if (numeroDias > 0)
                {
                    qtMediaAtendido += (Convert.ToDecimal(desempenhoVenda.Vendas_qt_total) / Convert.ToDecimal(desempenhoVenda.Numero_Dias)) / Convert.ToDecimal(desempenhoVenda.Numero_Vendedor);
                    qtOcorrencias++;
                }
            }

            desempenhoVenda.Valor_Pago = valorPago;
            desempenhoVenda.Vendas_0 = vendasZero;
            desempenhoVenda.Vendas_1 = vendasUm;
            desempenhoVenda.Vendas_2 = vendasDois;
            desempenhoVenda.Vendas_3 = vendasTres;
            desempenhoVenda.Vendas_4 = vendasQuatro;
            desempenhoVenda.Vendas_5 = vendasCinco;
            desempenhoVenda.Vendas_Consumidor = vendasConsumidor;
            desempenhoVenda.Vendas_qt_total = vendasTotal;
            desempenhoVenda.Numero_Dias = numeroDias;
            desempenhoVenda.Numero_Vendedor = Convert.ToInt32((qtMediaAtendido / Convert.ToDecimal(qtOcorrencias)) * 100);

            desempenhoVendaLoja.Add(desempenhoVenda);

            return desempenhoVendaLoja;
        }

        public Sp_Busca_Supervisor_LojaResult BuscaSupervisorLoja(string filial)
        {
            db = new DCDataContext(Constante.ConnectionStringIntranet);

            return (db.Sp_Busca_Supervisor_Loja(filial)).Take(1).SingleOrDefault();
        }

        public List<Sp_Ranking_VendaResult> BuscaDesempenhoVendedor(string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (db.Sp_Ranking_Venda(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim))).ToList();
        }

        public List<Sp_Ranking_Venda_LojaResult> BuscaDesempenhoFilial(string dataInicio, string dataFim)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (db.Sp_Ranking_Venda_Loja(baseController.AjustaData(dataInicio), baseController.AjustaData(dataFim))).ToList();
        }

        public List<LOJA_VENDEDORE> BuscaTotalVendedoresAtivo()
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (from v in db.LOJA_VENDEDOREs where v.DATA_DESATIVACAO == null select v).ToList();
        }

        public List<SP_OBTER_FLASH_DIARIO_SUPERResult> ObterFlashVarejo(int ano, int anoComparativo, int? mes, int? diaIni, int? diaFim, string supervisor, string tipoLoja, string filial)
        {
            db = new DCDataContext(Constante.ConnectionString);

            return (db.SP_OBTER_FLASH_DIARIO_SUPER(ano, anoComparativo, mes, diaIni, diaFim, supervisor, tipoLoja, filial)).ToList();
        }

        public List<SP_OBTER_FLASH_HANDCLUBResult> ObterFlashHandclub(DateTime dataIni, DateTime dataFim, string filial)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);

            return (LINXdb.SP_OBTER_FLASH_HANDCLUB(dataIni, dataFim, filial)).ToList();
        }

        public List<SP_OBTER_DESEMPENHO_VENDEDORResult> ObterDesempenhoVendedores(string codigoFilial, DateTime dataIni, DateTime dataFim, string vendedor)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);

            return (LINXdb.SP_OBTER_DESEMPENHO_VENDEDOR(codigoFilial, dataIni, dataFim, vendedor)).ToList();
        }

        public List<SP_OBTER_DESEMPENHO_VENDEDOR_REDEResult> ObterDesempenhoFiliais(string codigoFilial, DateTime dataIni, DateTime dataFim)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);

            return (LINXdb.SP_OBTER_DESEMPENHO_VENDEDOR_REDE(dataIni, dataFim, codigoFilial)).ToList();
        }

        public List<SP_OBTER_VENDA_CLIENTENVResult> ObterDesempenhoVendedorNV(string ano, string codigoFilial, string vendedor)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);

            return (LINXdb.SP_OBTER_VENDA_CLIENTENV(ano, codigoFilial, vendedor)).ToList();
        }

        public List<SP_OBTER_VENDA_CLIENTENV_TICKETResult> ObterDesempenhoVendedorNVTicket(string vendedor, DateTime dataIni, DateTime dataFim)
        {
            LINXdb = new LINXDataContext(Constante.ConnectionString);

            return (LINXdb.SP_OBTER_VENDA_CLIENTENV_TICKET(vendedor, dataIni, dataFim)).ToList();
        }

        public List<SP_OBTER_GINCANA_NAT2019_PERIODOResult> ObterDesempenhoGincana2019(DateTime dataIni, DateTime dataFim, string codigoFilial)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);

            return (Intradb.SP_OBTER_GINCANA_NAT2019_PERIODO(dataIni, dataFim, codigoFilial)).ToList();
        }

        /*QUARENTINE */
        public List<QUAR> ObterQUARUsuario(string nomeUsuario)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.QUARs
                    where m.USUARIO.Trim().ToLower() == nomeUsuario.Trim().ToLower()
                    orderby m.VALOR_PAGO descending
                    select m).ToList();
        }
        public List<QUAR> ObterQUARUsuario(string nomeUsuario, int mes, int dia)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.QUARs
                    where m.USUARIO == nomeUsuario
                    && m.MES == mes
                    && m.DIA == dia
                    orderby m.VALOR_PAGO descending
                    select m).ToList();
        }
        public QUAR ObterQUAR(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            return (from m in Intradb.QUARs
                    where
                        m.CODIGO == codigo
                    select m).SingleOrDefault();
        }
        public void InserirQUAR(QUAR quar)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                Intradb.QUARs.InsertOnSubmit(quar);
                Intradb.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AtualizarQUAR(QUAR novo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            QUAR quar = ObterQUAR(novo.CODIGO);

            if (quar != null)
            {
                quar.CODIGO = novo.CODIGO;
                quar.DATA_BAIXA = novo.DATA_BAIXA;
                quar.OBS = novo.OBS;

                try
                {
                    Intradb.SubmitChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void ExcluirQUAR(int codigo)
        {
            Intradb = new INTRADataContext(Constante.ConnectionStringIntranet);
            try
            {
                QUAR quar = ObterQUAR(codigo);
                if (quar != null)
                {
                    Intradb.QUARs.DeleteOnSubmit(quar);
                    Intradb.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //FIM DESENV_MATERIAL_CARRINHO


    }
}
