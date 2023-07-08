using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2PriceSpecial
    {
        [JsonProperty("prices")]
        public List<ProductV2PriceSpecialSkus> Prices { get; set; }
    }

    public class ProductV2PriceSpecialSkus
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("store_id")]
        public int StoreId { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }
    }


}