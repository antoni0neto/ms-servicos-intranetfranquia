using DAL;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Relatorios.mod_ecom.correio
{
    public class Mandae
    {
        private EcomController _eController { get; set; }
        private string _customerId { get; set; }
        private string _url { get; set; }
        private string _urlFrete { get; set; }
        private string _urlTracking { get; set; }
        private string _authorization { get; set; }

        public Mandae()
        {
            _eController = new EcomController();

            var loginCorreio = _eController.ObterLoginCorreios();

            if (loginCorreio == null)
                throw new Exception("ECOM_CORREIOS_LOGIN não existe...");

            _customerId = loginCorreio.MANDAE_CUSTOMERID;
            _url = loginCorreio.MANDAE_URL_API;
            _urlFrete = loginCorreio.MANDAE_URLFRETE_API;
            _urlTracking = loginCorreio.MANDAE_URLTRACKING;
            _authorization = loginCorreio.MANDAE_AUTHORIZATION;
        }

        public List<string> PostarObjeto(List<SP_OBTER_ECOM_PEDIDO_FATURADOResult> pedidoFaturado)
        {
            var etiquetaLista = new List<string>();
            var erroLista = new List<string>();

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

                var dataFrete = MontarJsonFrete(vPedido);
                var valorFrete = CalcularFrete(dataFrete, vPedido.FORMA_ENTREGA, vPedido.CEP.Trim());
                if (valorFrete <= 0)
                {
                    erroLista.Add("Erro ao postar pacote para Mandaê. Frete não foi encontrado.");
                    continue;
                }

                // posta
                var trackingId = GerarTrackingId();
                var data = MontarJson(vPedido, trackingId, valorFrete);
                var resp = CriarPedido(data);

                if (resp == HttpStatusCode.Created || resp.ToString() == "422")
                {
                    var correiosPost = new ECOM_CORREIOS_POST();
                    correiosPost.TRACK_NUMBER = trackingId;
                    correiosPost.PEDIDO = vPedido.PEDIDO;
                    correiosPost.ECOM_EXPEDICAO_VOLUME = vPedido.ECOM_EXPEDICAO_VOLUME;
                    correiosPost.VALOR_TARIFA = valorFrete;
                    correiosPost.ENTREGA_DOMICILIAR = "";
                    correiosPost.PRAZO_DIAS_UTEIS = "";
                    correiosPost.PESO_GRAMA = 0;
                    correiosPost.DATA_ENVIO = DateTime.Now;
                    correiosPost.SHIP_SERVICE_MANDAE = vPedido.FORMA_ENTREGA;
                    correiosPost.ENTREGA_REALIZADA = false;

                    _eController.InserirCorreiosPost(correiosPost);

                    etiquetaLista.Add(trackingId);
                }
                else
                {
                    erroLista.Add("Erro ao postar pacote para Mandaê. Erro na criação da Encomenda (API).");
                }

                pedidoAux += 1;
            }

            if (erroLista.Count() > 0)
                return erroLista;

            return etiquetaLista;

        }

        private decimal CalcularFrete(string data, string formaEntrega, string cep)
        {
            var valorFrete = 0M;
            var url = _urlFrete.Replace("@@@@@@@@", cep.Replace("-", "").Trim());

            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", _authorization);
            request.AddHeader("content-type", "application/json");

            request.AddParameter(
                "application/json",
                data,
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var shippingServices = (JArray)JObject.Parse(response.Content)["shippingServices"];

                var formaEntregaAux = "";

                if (formaEntrega.ToLower() == "rapido" || formaEntrega.ToLower() == "rápido")
                {
                    formaEntrega = "Rápido";
                    formaEntregaAux = "Econômico";
                }
                else if (formaEntrega.ToLower() == "economico" || formaEntrega.ToLower() == "econômico")
                {
                    formaEntrega = "Econômico";
                    formaEntregaAux = "Rápido";
                }

                var price = ObterValorArray(shippingServices, formaEntrega);
                if (price <= 0)
                    price = ObterValorArray(shippingServices, formaEntregaAux);

                valorFrete = price;
            }

            return valorFrete;
        }
        private HttpStatusCode CriarPedido(string data)
        {
            var client = new RestClient(_url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("authorization", _authorization);
            request.AddHeader("content-type", "application/json");

            request.AddParameter(
                "application/json",
                data,
                ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.StatusCode;
        }
        private string MontarJsonFrete(SP_OBTER_ECOM_PEDIDO_FATURADOResult pedido)
        {
            var data = "{ " +
                            " \"declaredValue\": " + pedido.VALOR_TOTAL.ToString().Replace(",", ".") + "," +
                            " \"weight\" : " + pedido.PESO_KG.ToString().Replace(",", ".") + "," +
                            " \"height\" : " + Convert.ToInt32(pedido.ALTURA_CM).ToString() + "," +
                            " \"width\" : " + Convert.ToInt32(pedido.LARGURA_CM).ToString() + "," +
                            " \"length\": " + Convert.ToInt32(pedido.COMPRIMENTO_CM).ToString() + "" +
                        "}";

            return data;

        }
        private string MontarJson(SP_OBTER_ECOM_PEDIDO_FATURADOResult pedido, string trackingId, decimal valorFrete)
        {

            var d = DateTime.Now.AddDays(3).ToString("o");

            var data = "";
            data = data + "{\r\n       " +
                "    \"customerId\":\"" + _customerId + "\",\r\n       " +
                //"    \"partnerOrderId\":\"OI-123456\",\r\n       " +
                "    \"scheduling\":\"" + d + "\",\r\n       " +
                "    \"items\":[\r\n          " +
                "               {\r\n             " +
                "                   \"dimensions\":{\r\n                " +
                "                                       \"height\":" + Convert.ToInt32(pedido.ALTURA_CM).ToString() + ",\r\n                " +
                "                                       \"width\":" + Convert.ToInt32(pedido.LARGURA_CM).ToString() + ",\r\n                " +
                "                                       \"length\":" + Convert.ToInt32(pedido.COMPRIMENTO_CM).ToString() + ",\r\n                " +
                "                                       \"weight\":" + pedido.PESO_KG.ToString().Replace(",", ".") + "\r\n             " +
                "                                   },\r\n             " +
                "               \"skus\":[\r\n                ";


            var itens = _eController.ObterProdutoMandaeItens(pedido.PEDIDO);
            int i = itens.Count();
            int iAux = 0;
            foreach (var item in itens)
            {


                // loops nos skus
                data = data + "             {\r\n                   " +
                "                               \"skuId\":\"" + item.SKU + "\",\r\n                   " +
                "                               \"description\":\"" + item.NOME + "\",\r\n                   " +
                //"                               \"ean\":\"SKUVR1\",\r\n                   " +
                "                               \"price\":" + item.PRECO1.ToString().Replace(",", ".") + ",\r\n                   " +
                "                               \"freight\":0,\r\n                   " +
                "                               \"quantity\":" + Convert.ToInt32(item.QTDE_ORIGINAL).ToString() + "\r\n                " +
                "                           }\r\n             ";

                iAux = iAux + 1;

                if (iAux < i)
                    data = data + ",";
            }
            // fim loop
            data = data + "                        ],\r\n\t\t\t " +
                "               \"partnerItemId\" : \"" + ((pedido.PEDIDO_EXTERNO != null) ? pedido.PEDIDO_EXTERNO.Trim() : ("TR" + pedido.PEDIDO.Trim())) + "\",\r\n             " +
             "               \"invoice\": {\r\n                " +
             "                               \"id\":\"" + pedido.NF_SAIDA + "\",\r\n                " +
             "                               \"key\":\"" + pedido.CHAVE_NFE + "\"\r\n              " +
             "                            },\r\n             \r\n             " +
             "               \"trackingId\" : \"" + trackingId + "\" ,\r\n                    " +
             //"               \"observation\":\"\",\r\n             " +
             "               \"recipient\":{\r\n                " +
             "                               \"fullName\":\"" + pedido.NOME_COMPLETO + "\",\r\n                " +
             "                               \"phone\":\"(" + pedido.DDD + ") " + pedido.TELEFONE + "\",\r\n                " +
             "                               \"document\":\"" + pedido.CGC_CPF.Trim() + "\",\r\n                " +
             "                               \"email\":\"" + pedido.EMAIL + "\",\r\n                " +
             "                               \"address\":{\r\n                   " +
             "                               \"postalCode\":\"" + pedido.CEP + "\",\r\n                   " +
             "                               \"street\":\"" + pedido.ENDERECO + "\",\r\n                   " +
             "                               \"number\":\"" + pedido.NUMERO + "\",\r\n                   " +
             "                               \"addressLine2\":\"" + pedido.COMPLEMENTO + "\",\r\n                   " +
             "                               \"neighborhood\":\"" + pedido.BAIRRO + "\",\r\n                   " +
             "                               \"city\":\"" + pedido.CIDADE + "\",\r\n                   " +
             "                               \"state\":\"" + pedido.UF + "\",\r\n                   " +
             "                               \"country\":\"BR\"\r\n                " +
             "                             }\r\n                                 " +
             "               },\r\n             " +

             "               \"shippingService\":\"" + pedido.FORMA_ENTREGA + "\",\r\n\r\n             " +

             "               \"valueAddedServices\":[\r\n                " +
             "                                       {\r\n                   " +
             "                                           \"name\":\"ValorDeclarado\",\r\n                   " +
             "                                           \"value\":" + pedido.VALOR_TOTAL.ToString().Replace(",", ".") + "\r\n                " +
             "                                       }\r\n             " +
             "                                       ]\r\n          " +
             "              }\r\n       " +
             "             ],\r\n       " +
             "    \"sender\":{\r\n          " +
             "                \"fullName\":\"Handbook Online\",\r\n          " +
             "                \"address\":{\r\n             " +
             "                            \"postalCode\":\"04713030\",\r\n             " +
             "                            \"street\":\"Rua Bento de Matos\",\r\n             " +
             "                            \"number\":\"122\",\r\n             " +
             "                            \"neighborhood\":\"Chácara Santo Antônio\",\r\n             " +
             "                            \"city\":\"São Paulo\",\r\n             " +
             "                            \"state\":\"SP\",\r\n             " +
             "                            \"country\":\"BR\"\r\n          " +
             "                            }\r\n       " +
             "                },\r\n       " +
             "    \"observation\":null,\r\n       " +
             "    \"channel\":\"ecommerce\",\r\n       " +
             "    \"store\":\"Handbook Online\",\r\n       " +
             "    \"totalValue\":" + pedido.VALOR_TOTAL.ToString().Replace(",", ".") + ",\r\n       " +
             "    \"totalFreight\":" + valorFrete.ToString().Replace(",", ".") + ",\r\n       " +
             "    \"vehicle\":\"Car\"\r\n    " +
             "}";

            return data;

        }
        private string GerarTrackingId()
        {
            Random randNum = new Random();
            var res = randNum.Next(1000000, 9999999);
            var mes = DateTime.Now.Month;
            var trackMonth = (mes < 10) ? ("0" + mes.ToString()) : mes.ToString();

            return "HAND" + res.ToString() + trackMonth;
        }
        private decimal ObterValorArray(JArray yourJArray, string key)
        {
            foreach (var keyValuePair in yourJArray)
            {
                if (keyValuePair.SelectToken("name").ToString() == key || keyValuePair.SelectToken("name").ToString() == "Frete Mandaê")
                    return Convert.ToDecimal(keyValuePair.SelectToken("price"));
            }

            return 0;
        }


        public List<RASTREIO_MANDAE> ObterRastreioPedido(string trackingId)
        {
            var url = _urlTracking + trackingId;

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", _authorization);
            request.AddHeader("content-type", "application/json");

            IRestResponse response = client.Execute(request);

            List<RASTREIO_MANDAE> lstRastreioMandae = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                lstRastreioMandae = new List<RASTREIO_MANDAE>();

                var jsonArray = (JArray)JObject.Parse(response.Content)["events"];
                foreach (var j in jsonArray)
                {
                    var data = j.SelectToken("date").ToString();
                    var nome = j.SelectToken("name").ToString();
                    var descricao = j.SelectToken("description").ToString();

                    lstRastreioMandae.Add(new RASTREIO_MANDAE
                    {
                        DATA = (data == "") ? DateTime.Now : Convert.ToDateTime(data),
                        NOME = nome,
                        DESCRICAO = descricao
                    });

                }
            }

            return lstRastreioMandae;
        }
        public bool VerificarEntregaRealizada(string trackingId)
        {
            var url = _urlTracking + trackingId;

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("authorization", _authorization);
            request.AddHeader("content-type", "application/json");

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonArray = (JArray)JObject.Parse(response.Content)["events"];
                if (jsonArray.Count > 0)
                {
                    var evento = jsonArray.First["name"].ToString().ToLower().Trim();

                    if (evento == "entrega realizada" || evento == "pedido entregue")
                        return true;
                }
            }

            return false;
        }

    }
}

public class RASTREIO_MANDAE
{
    public DateTime DATA { get; set; }
    public string NOME { get; set; }
    public string DESCRICAO { get; set; }
}