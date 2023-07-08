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
    public class Loggi
    {
        private EcomController _eController { get; set; }

        public Loggi()
        {
            _eController = new EcomController();
        }

        public List<string> PostarObjeto(List<SP_OBTER_ECOM_PEDIDO_FATURADOResult> pedidoFaturado, string trackNumber)
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

                var correiosPost = new ECOM_CORREIOS_POST();
                correiosPost.TRACK_NUMBER = trackNumber;
                correiosPost.PEDIDO = vPedido.PEDIDO;
                correiosPost.ECOM_EXPEDICAO_VOLUME = vPedido.ECOM_EXPEDICAO_VOLUME;
                correiosPost.VALOR_TARIFA = 0;
                correiosPost.ENTREGA_DOMICILIAR = "";
                correiosPost.PRAZO_DIAS_UTEIS = "";
                correiosPost.PESO_GRAMA = 0;
                correiosPost.DATA_ENVIO = DateTime.Now;
                correiosPost.SHIP_SERVICE_MANDAE = vPedido.FORMA_ENTREGA;
                correiosPost.ENTREGA_REALIZADA = false;

                _eController.InserirCorreiosPost(correiosPost);

                etiquetaLista.Add(trackNumber);

                pedidoAux += 1;
            }

            if (erroLista.Count() > 0)
                return erroLista;

            return etiquetaLista;

        }

    }
}