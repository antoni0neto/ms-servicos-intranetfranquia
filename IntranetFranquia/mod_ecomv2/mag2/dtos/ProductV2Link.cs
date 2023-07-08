using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2Link
    {
        [JsonProperty("items")]
        public List<ProductV2LinkItems> Items { get; set; }
    }

    public class ProductV2LinkItems
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("link_type")]
        public string LinkType { get; set; }

        [JsonProperty("linked_product_sku")]
        public string LinkedProductSku { get; set; }

        [JsonProperty("linked_product_type")]
        public string LinkedProductType { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }
    }
}