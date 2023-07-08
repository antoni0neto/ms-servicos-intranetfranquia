using Newtonsoft.Json;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2WebSite
    {
        [JsonProperty("productWebsiteLink")]
        public ProductV2WebSiteLink ProductWebsiteLink { get; set; }
    }

    public class ProductV2WebSiteLink
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("website_id")]
        public int WebsiteId { get; set; }
    }
}