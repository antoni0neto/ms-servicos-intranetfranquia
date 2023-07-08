using Newtonsoft.Json;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2Category
    {
        [JsonProperty("productLink")]
        public ProductV2CategorySku ProductLink { get; set; }
    }

    public class ProductV2CategorySku
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }
    }
}