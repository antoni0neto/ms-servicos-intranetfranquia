using Newtonsoft.Json;
using System;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class OrderV2Comment
    {
        [JsonProperty("statusHistory")]
        public OrderV2CommentStatus StatusHistory { get; set; }
    }

    public class OrderV2CommentStatus
    {
        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("is_customer_notified")]
        public int IsCustomerNotified { get; set; }

        [JsonProperty("is_visible_on_front")]
        public int IsVisibleOnFront { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}