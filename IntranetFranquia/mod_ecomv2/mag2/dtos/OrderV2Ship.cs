using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class OrderV2Ship
    {
        [JsonProperty("items")]
        public List<OrderV2ShipItems> Items { get; set; }

        [JsonProperty("notify")]
        public bool Notify { get; set; }

        [JsonProperty("appendComment")]
        public bool AppendComment { get; set; }

        [JsonProperty("comment")]
        public OrderV2ShipComment Comment { get; set; }

        [JsonProperty("tracks")]
        public List<OrderV2ShipTrack> Tracks { get; set; }
    }

    public class OrderV2ShipItems
    {
        [JsonProperty("order_item_id")]
        public int OrderItemId { get; set; }

        [JsonProperty("qty")]
        public int Qty { get; set; }
    }

    public class OrderV2ShipComment
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("is_visible_on_front")]
        public int IsVisibleOnFront { get; set; }
    }

    public class OrderV2ShipTrack
    {
        [JsonProperty("track_number")]
        public string TrackNumber { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("carrier_code")]
        public string CarrierCode { get; set; }
    }
}