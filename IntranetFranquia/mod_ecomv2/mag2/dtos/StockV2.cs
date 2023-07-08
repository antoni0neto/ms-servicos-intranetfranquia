using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class StockV2
    {
        [JsonProperty("sourceItems")]
        public List<StockV2Items> SourceItems { get; set; }
    }

    public class StockV2Items
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("source_code")]
        public string SourceCode { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}