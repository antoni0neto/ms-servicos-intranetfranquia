using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;

namespace Relatorios
{
    public class MaterialEntity
    {
        public string GRUPO { get; set; }
        public string SUBGRUPO { get; set; }
        public string COR { get; set; }
        public string COR_FORNECEDOR { get; set; }
        public decimal QTDE { get; set; }
        public decimal QTDE_ENTREGUE { get; set; }
        public decimal QTDE_PEDIDO { get; set; }
        public decimal QTDE_PRODUTO { get; set; }
        public decimal QTDE_NECESSIDADE { get; set; }
        public decimal QTDE_SOBRA { get; set; }
    }
}