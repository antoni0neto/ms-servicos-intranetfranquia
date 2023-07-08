using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class OrderV2
    {
        [JsonProperty("items")]
        public List<OrderV2Items> Items { get; set; }
    }

    public class OrderV2Items
    {
        [JsonProperty("entity_id")]
        public int ParentId { get; set; }

        [JsonProperty("increment_id")]
        public string OrderId { get; set; }

        [JsonProperty("items")]
        public List<OrderV2ItemsItems> Items { get; set; }
    }

    public class OrderV2ItemsItems
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("qty_ordered")]
        public int Qty { get; set; }
    }
}