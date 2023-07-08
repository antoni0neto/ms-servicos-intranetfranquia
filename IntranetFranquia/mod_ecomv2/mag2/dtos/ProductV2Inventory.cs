using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2Inventory
    {
        [JsonProperty("items")]
        public List<ProductV2InventoryItems> Items { get; set; }
    }

    public class ProductV2InventoryItems
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