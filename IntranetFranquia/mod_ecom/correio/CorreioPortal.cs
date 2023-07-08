using DAL;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;

namespace Relatorios.mod_ecom.correio
{
    public class CorreioPortal
    {
        private CorreioPP.PrePostagemWSPortTypeClient _correioPortal { get; set; }
        private string _login { get; set; }
        private string _senha { get; set; }
        private int _codAgencia { get; set; }
        private string _numeroCartao { get; set; }
        private string _urlEtiqueta { get; set; }
        private string _urlEtiquetaFormato { get; set; }
        private EcomController _eController { get; set; }

        public CorreioPortal()
        {
            _correioPortal = new CorreioPP.PrePostagemWSPortTypeClient("PrePostagemWSSOAP12port_http");
            //_correioPortal = new PrePostagemWSPortTypeClient("PrePostagemWSSOAP12port_http");
            _eController = new EcomController();

            var loginCorreio = _eController.ObterLoginCorreios();

            if (loginCorreio == null)
                throw new Exception("ECOM_CORREIOS_LOGIN não existe...");

            _login = loginCorreio.PP_USUARIO;
            _senha = loginCorreio.PP_SENHA;
            _codAgencia = loginCorreio.COD_AGENCIA;
            _numeroCartao = loginCorreio.CARTAO_POSTAGEM;
            _urlEtiqueta = loginCorreio.URL_ETIQUETA;
            _urlEtiquetaFormato = loginCorreio.URL_ETIQUETA_FORMATO;
        }

        public List<string> PostarObjeto(List<SP_OBTER_ECOM_PEDIDO_FATURADOResult> pedidoFaturado)
        {
            List<string> etiquetaLista = new List<string>();
            List<string> erroLista = new List<string>();

            int codigoDoServico = 0;
            int pedidoAux = 1;
            foreach (var vPedido in pedidoFaturado)
            {
                //Se track_number ja foi gerado, não gera novamente;
                if (vPedido.TRACK_NUMBER != "")
                {
                    etiquetaLista.Add(vPedido.TRACK_NUMBER);
                    pedidoAux += 1;
                    continue;
                }

                if (vPedido.FORMA_ENTREGA.ToLower() == "pac")
                    codigoDoServico = 04669; // PAC COM CONTRATO
                else if (vPedido.FORMA_ENTREGA.ToLower() == "sedex")
                    codigoDoServico = 04162; // SEDEX COM CONTRATO
                else
                    codigoDoServico = 04162;

                var resp = PrePostarCorreio(vPedido.ECOM_EXPEDICAO_VOLUME, _numeroCartao, vPedido.NOME_CLIENTE,
                                        vPedido.ENDERECO, vPedido.NUMERO, vPedido.COMPLEMENTO, vPedido.BAIRRO, vPedido.UF, vPedido.CIDADE, vPedido.CEP,
                                        vPedido.NF_SAIDA, vPedido.PEDIDO + "-" + pedidoAux.ToString(), codigoDoServico,
                                        Convert.ToInt32(vPedido.PESO_GRAMA), Convert.ToDecimal(vPedido.VALOR_TOTAL), vPedido.ALTURA_CM, vPedido.LARGURA_CM, vPedido.COMPRIMENTO_CM,
                                        false, false, vPedido.EMAIL, "", "", _codAgencia, _login, _senha, "", vPedido.CGC_CPF, vPedido.FORMA_ENTREGA
                                        );

                //sucesso
                if (resp.Length == 13)
                    etiquetaLista.Add(resp);
                else
                    erroLista.Add(resp);

                pedidoAux += 1;

            }

            if (erroLista.Count() > 0)
                return erroLista;

            return etiquetaLista;
        }

        private string PrePostarCorreio(int codigoExpedicaoVolume, string numeroCartao, string nome,
                                string endereco, string numero, string complemento, string bairro, string uf, string cidade, string cepString,
                                string notaFiscal, string pedido, int codigoDoServico,
                                int pesoReal, decimal valorDeclarado, decimal altura, decimal largura, decimal comprimento,
                                bool avisoRecebimento, bool numeroDeRegistro, string email, string conteudo, string celular, int codAgencia, string login, string senha, string lote, string cpf,
                                string formaEntrega)
        {

            //pedido = pedido.Trim() + "X";


            //<?xml version="1.0" encoding="UTF-8"?>
            //<portalpostal>
            //   <pre_postagem>
            //      <chave>123456</chave>
            //      <nome>DEYSE NOGUEIRA</nome>
            //      <aos_cuidados>JOÃO</aos_cuidados>
            //      <cep>89900000</cep>
            //      <endereco>AVENIDA SALGADO FILHO</endereco>
            //      <numero>766</numero>
            //      <complemento>AP 208</complemento>
            //      <bairro>CENTRO</bairro>
            //      <cidade>SAO MIGUEL DO OESTE</cidade>
            //      <estado>SC</estado>
            //      <nota_fiscal>2150283</nota_fiscal>
            //      <centro_custo>1</centro_custo>
            //      <servico>PAC</servico>
            //      <servico_adicional>VD</servico_adicional>
            //      <valor_declarado>97.76</valor_declarado>
            //      <celular>49991496857</celular>
            //      <cpf_cnpj>04332424900</cpf_cnpj>
            //      <email>dmpotrich@yahoo.com.br</email>
            //      <observacoes></observacoes>
            //      <conteudo>
            //        <item>
            //          <descricao>Item 1</descricao>
            //          <quantidade>1</quantidade>
            //          <valor>235,30</valor>
            //        </item>
            //        <item>
            //          <descricao>Item 2</descricao>
            //          <quantidade>1</quantidade>
            //          <valor>50,23</valor>
            //        </item>
            //      </conteudo>
            //   </pre_postagem>
            //</portalpostal>










            //Posta o objeto no correio
            var post = _correioPortal.PrePostarObjeto(numeroCartao, nome, endereco, numero, complemento, bairro, uf, cidade, cepString, notaFiscal, pedido, codigoDoServico,
                        pesoReal, (float)valorDeclarado, (float)altura, (float)largura, (float)comprimento, avisoRecebimento, numeroDeRegistro, email, conteudo, celular,
                        codAgencia, login, senha, lote);


            //Obter Retorno
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(post);
            XmlNodeList nodelist = doc.SelectNodes("/portalpostal")[0].SelectNodes("postagem");
            var chave = "";
            var codigo_rastreio = "";
            var sigla_triagem = "";
            var detalhes = "";
            foreach (XmlNode node in nodelist)
            {
                chave = node.SelectSingleNode("chave").InnerText;
                codigo_rastreio = node.SelectSingleNode("codigo_rastreio").InnerText;
                sigla_triagem = node.SelectSingleNode("sigla_triagem").InnerText;
                detalhes = node.SelectSingleNode("detalhes").InnerText;
            }
            /*******************************************************************************************/

            if (codigo_rastreio == "erro")
            {
                return "Erro ao postar no Correio PP - " + detalhes;
            }

            var correiosPost = new ECOM_CORREIOS_POST();
            correiosPost.TRACK_NUMBER = codigo_rastreio;
            correiosPost.PEDIDO = pedido;
            correiosPost.ECOM_EXPEDICAO_VOLUME = codigoExpedicaoVolume;
            correiosPost.VALOR_TARIFA = 0;
            correiosPost.ENTREGA_DOMICILIAR = "";
            correiosPost.PRAZO_DIAS_UTEIS = "";
            correiosPost.PESO_GRAMA = 0;
            correiosPost.DATA_ENVIO = DateTime.Now;
            correiosPost.SHIP_SERVICE_MANDAE = formaEntrega;
            correiosPost.ENTREGA_REALIZADA = false;

            _eController.InserirCorreiosPost(correiosPost);

            return codigo_rastreio;
        }

        public string GerarURLEtiqueta(string codigoRastreio)
        {
            string urlEtiqueta = "";


            urlEtiqueta = _urlEtiqueta;

            urlEtiqueta = urlEtiqueta.Replace("##codAgencia##", _codAgencia.ToString());
            urlEtiqueta = urlEtiqueta.Replace("##login##", _login);
            urlEtiqueta = urlEtiqueta.Replace("##senha##", _senha);
            urlEtiqueta = urlEtiqueta.Replace("##sro##", codigoRastreio);
            urlEtiqueta = urlEtiqueta.Replace("##formato##", _urlEtiquetaFormato);

            return urlEtiqueta;
        }

        public List<RASTREIO_MANDAE> ObterRastreioPedido(string trackingId)
        {

            var urlTracking = "http://www.portalpostal.com.br/Cliente/Servicos/ws_consulta_postagem.jsp?codigoAGF=152&usuario=reworkedservice&senha=10203040&tipoArquivo=RAW&formatoArquivo=CSV&separadorArquivo=;&cabecalhoArquivo=N&camposArquivo=DATA_SITUACAO|SITUACAO&tamanhoArquivo=30|250&data_inicio=&data_fim=&sro=";

            var url = urlTracking + trackingId;

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");

            IRestResponse response = client.Execute(request);

            List<RASTREIO_MANDAE> lstRastreioMandae = null;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                lstRastreioMandae = new List<RASTREIO_MANDAE>();

                var resp = response.Content.ToString();

                if (resp != "" && !resp.Contains("null"))
                {
                    var campos = resp.Split(';');

                    lstRastreioMandae.Add(new RASTREIO_MANDAE
                    {
                        DATA = Convert.ToDateTime(campos[0].ToString()),
                        NOME = "Correio",
                        DESCRICAO = campos[1].ToString()
                    });
                }

            }

            return lstRastreioMandae;
        }
        public bool VerificarEntregaRealizada(string trackingId)
        {
            var urlTracking = "http://www.portalpostal.com.br/Cliente/Servicos/ws_consulta_postagem.jsp?codigoAGF=152&usuario=reworkedservice&senha=10203040&tipoArquivo=RAW&formatoArquivo=CSV&separadorArquivo=;&cabecalhoArquivo=N&camposArquivo=DATA_SITUACAO|SITUACAO&tamanhoArquivo=30|250&data_inicio=&data_fim=&sro=";
            var url = urlTracking + trackingId;

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var resp = response.Content.ToString();
                if (resp != "")
                {
                    var campos = resp.Split(';');

                    var statusObj = campos[1].ToString().ToLower();

                    if (statusObj.Contains("objeto entregue"))
                        return true;
                }

            }

            return false;
        }

    }
}