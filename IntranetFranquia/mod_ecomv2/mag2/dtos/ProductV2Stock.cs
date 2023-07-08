using Newtonsoft.Json;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2Stock
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("stock_id")]
        public int StockId { get; set; }

        [JsonProperty("qty")]
        public int Qty { get; set; }

        [JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }

        [JsonProperty("manage_stock")]
        public bool ManageStock { get; set; }
    }
}