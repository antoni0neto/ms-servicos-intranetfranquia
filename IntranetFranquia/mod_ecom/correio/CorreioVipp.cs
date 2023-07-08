using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Relatorios.mod_ecom.correio
{
    public class CorreioVipp
    {
        private CorreiosVipp.PostagemVippSoapClient _correioVipp { get; set; }
        private EcomController _eController { get; set; }

        public CorreioVipp()
        {
            _correioVipp = new CorreiosVipp.PostagemVippSoapClient("PostagemVippSoap");
            _eController = new EcomController();
        }

        public List<string> PostarObjeto(List<SP_OBTER_ECOM_PEDIDO_FATURADOResult> pedidoFaturado)
        {

            List<string> etiquetaLista = new List<string>();
            List<string> erroLista = new List<string>();
            int posicaoVolume = 1;

            var loginVipp = _eController.ObterLoginCorreios();
            if (loginVipp == null)
            {
                erroLista.Add("Erro ao encontrar credencias dos Correios. Entre em contato com TI.");
                return erroLista;
            }

            // var volumesPedido = _eController.ObterMagentoExpedicaoVolumePorPedido(pedidoFaturado[0].PEDIDO);
            foreach (var vPedido in pedidoFaturado)
            {

                if (vPedido.TRACK_NUMBER != "")
                {
                    etiquetaLista.Add(vPedido.TRACK_NUMBER);
                    continue;
                }

                CorreiosVipp.Postagem pacoteObjeto = new CorreiosVipp.Postagem();

                // Login
                CorreiosVipp.PerfilVipp perfilVipp = new CorreiosVipp.PerfilVipp();
                perfilVipp.Usuario = loginVipp.VIPP_USUARIO;
                perfilVipp.Token = loginVipp.VIPP_SENHA;
                perfilVipp.IdPerfil = loginVipp.VIPP_PERFIL;
                pacoteObjeto.PerfilVipp = perfilVipp;

                CorreiosVipp.ContratoEct contratoECT = new CorreiosVipp.ContratoEct();
                contratoECT.CodigoAdministrativo = loginVipp.ADMIN_CODIGO;
                contratoECT.NrCartao = loginVipp.CARTAO_POSTAGEM;
                contratoECT.NrContrato = loginVipp.CONTRATO;
                pacoteObjeto.ContratoEct = contratoECT;

                // Destinatario
                CorreiosVipp.Destinatario dest = new CorreiosVipp.Destinatario();
                dest.Nome = vPedido.NOME_CLIENTE;
                dest.CnpjCpf = vPedido.CGC_CPF;
                dest.Endereco = vPedido.ENDERECO;
                dest.Numero = vPedido.NUMERO;
                dest.Complemento = vPedido.COMPLEMENTO;
                dest.Bairro = vPedido.BAIRRO;
                dest.Cidade = vPedido.CIDADE;
                dest.UF = vPedido.UF;
                dest.Cep = vPedido.CEP;
                dest.Email = vPedido.EMAIL;
                pacoteObjeto.Destinatario = dest;


                //VALIDAR
                CorreiosVipp.Servico servico = new CorreiosVipp.Servico();

                if (vPedido.FORMA_ENTREGA.ToLower() == "pac")
                    servico.ServicoECT = "04669"; // PAC COM CONTRATO
                else if (vPedido.FORMA_ENTREGA.ToLower() == "sedex")
                    servico.ServicoECT = "04162"; // SEDEX COM CONTRATO
                else
                    servico.ServicoECT = "04162";

                pacoteObjeto.Servico = servico;

                //Nota Fiscal
                List<CorreiosVipp.NotaFiscal> notasFiscais = new List<CorreiosVipp.NotaFiscal>();
                CorreiosVipp.NotaFiscal notasFiscal = new CorreiosVipp.NotaFiscal();
                notasFiscal.DtNotaFiscal = Convert.ToDateTime(vPedido.EMISSAO).ToString("dd/MM/yyyy"); //DD/MM/YYYY
                notasFiscal.SerieNotaFiscal = vPedido.SERIE_NF;
                notasFiscal.NrNotaFiscal = vPedido.NF_SAIDA;
                notasFiscal.VlrTotalNota = vPedido.VALOR_TOTAL.ToString();

                notasFiscais.Add(notasFiscal);
                pacoteObjeto.NotasFiscais = notasFiscais.ToArray();

                // Volume Objeto
                List<CorreiosVipp.VolumeObjeto> volumes = new List<CorreiosVipp.VolumeObjeto>();

                var vol = new CorreiosVipp.VolumeObjeto();
                string pesoGrama = Convert.ToInt32(vPedido.PESO_GRAMA).ToString();
                vol.Peso = pesoGrama;
                vol.Altura = Convert.ToInt32(vPedido.ALTURA_CM).ToString();
                vol.Comprimento = Convert.ToInt32(vPedido.COMPRIMENTO_CM).ToString();
                vol.Largura = Convert.ToInt32(vPedido.LARGURA_CM).ToString();
                vol.PosicaoVolume = posicaoVolume.ToString();
                volumes.Add(vol);

                pacoteObjeto.Volumes = volumes.ToArray();


                //POSTAR OBJETO NO CORREIO
                var resp = _correioVipp.PostarObjeto(pacoteObjeto);

                //Obter status postagem
                var statusPostagem = (from stPost in resp.Elements("StatusPostagem") select stPost).First().Value;

                //valida postagem
                if (statusPostagem != null && statusPostagem.ToLower() == "valida")
                {
                    var volumesTag = from voltag in resp.Elements("Volumes") select voltag;
                    if (volumesTag != null && volumesTag.Count() > 0)
                    {
                        var volumeObjeto = from voltobj in volumesTag.Elements("VolumeObjeto") select voltobj;
                        if (volumeObjeto != null)
                        {
                            var objeto = volumeObjeto.First();
                            if (objeto != null)
                            {
                                var etiqueta = objeto.Element("Etiqueta").Value;
                                var peso = objeto.Element("Peso").Value;
                                var valorTarifa = volumeObjeto.First().Element("ValorTarifa").Value;
                                var stEntregaDomiciliar = volumeObjeto.First().Element("StEntregaDomiciliar").Value;
                                var diasUteisPrazo = volumeObjeto.First().Element("DiasUteisPrazo").Value;

                                var correiosPost = new ECOM_CORREIOS_POST();
                                correiosPost.TRACK_NUMBER = etiqueta;
                                correiosPost.PEDIDO = vPedido.PEDIDO;
                                correiosPost.ECOM_EXPEDICAO_VOLUME = vPedido.ECOM_EXPEDICAO_VOLUME;
                                correiosPost.VALOR_TARIFA = Convert.ToDecimal(valorTarifa.Replace(".", ","));
                                correiosPost.ENTREGA_DOMICILIAR = stEntregaDomiciliar;
                                correiosPost.PRAZO_DIAS_UTEIS = diasUteisPrazo;
                                correiosPost.PESO_GRAMA = Convert.ToDecimal(peso);
                                correiosPost.DATA_ENVIO = DateTime.Now;

                                _eController.InserirCorreiosPost(correiosPost);

                                if (etiqueta == "")
                                    erroLista.Add("Não foi possível obter a Etiqueta dos Correios. Posição Volume: " + posicaoVolume.ToString() + " Por favor, entrar em contato com Suporte TI.");
                                else
                                    etiquetaLista.Add(etiqueta);

                            }
                        }
                    }
                }
                else
                {
                    var listaErros = from lerros in resp.Elements("ListaErros") select lerros;
                    if (listaErros != null)
                    {
                        var erroTag = (from erroobj in listaErros.Elements("Erro") select erroobj);

                        string errosDescricao = "";
                        foreach (var erro in erroTag)
                        {
                            //var tipoErro = erro.Element("TipoErro").Value;
                            //var atributo = erro.Element("Atributo").Value;
                            var descricao = erro.Element("Descricao").Value;
                            errosDescricao = errosDescricao + descricao + ", ";
                            //var mensagem = erro.Element("Mensagem").Value;
                            //var stackTrace = erro.Element("StackTrace").Value;
                            //var stackTraceOut = erro.Element("StackTraceOut").Value;
                            //var origem = erro.Element("Origem").Value;
                        }

                        errosDescricao = errosDescricao.Trim() + ",";
                        errosDescricao = errosDescricao.Replace(",,", "");

                        erroLista.Add("Erro ao realizar a Postagem nos Correios (Vipp): " + errosDescricao);
                    }
                }

                posicaoVolume += 1;
            }

            if (erroLista.Count() > 0)
                return erroLista;

            return etiquetaLista;
        }


    }
}