using Newtonsoft.Json;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class OrderV2RuleCoupon
    {
        [JsonProperty("coupon")]
        public OrderV2RuleCouponCoupon Coupon { get; set; }
    }

    public class OrderV2RuleCouponCoupon
    {
        [JsonProperty("rule_id")]
        public int RuleId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("usage_limit")]
        public int UsageLimit { get; set; }

        [JsonProperty("usage_per_customer")]
        public int UsagePerCustomer { get; set; }

        [JsonProperty("is_primary")]
        public bool IsPrimary { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}