using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class OrderV2Rule
    {
        [JsonProperty("rule")]
        public OrderV2RuleRule Rule { get; set; }
    }

    public class OrderV2RuleRule
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("website_ids")]
        public List<int> WebsiteIds { get; set; }

        [JsonProperty("customer_group_ids")]
        public List<int> CustomerGroupIds { get; set; }

        [JsonProperty("from_date")]
        public string FromDate { get; set; }

        [JsonProperty("uses_per_customer")]
        public int UsesPerCustomer { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("is_advanced")]
        public bool IsAdvanced { get; set; }

        [JsonProperty("simple_action")]
        public string SimpleAction { get; set; }

        [JsonProperty("discount_amount")]
        public double DiscountAmount { get; set; }

        [JsonProperty("discount_step")]
        public double DiscountStep { get; set; }

        [JsonProperty("apply_to_shipping")]
        public bool ApplyToShipping { get; set; }

        [JsonProperty("coupon_type")]
        public string CouponType { get; set; }

        [JsonProperty("use_auto_generation")]
        public bool UseAutoGeneration { get; set; }

        [JsonProperty("uses_per_coupon")]
        public int UsesPerCoupon { get; set; }

        [JsonProperty("simple_free_shipping")]
        public string SimpleFreeShipping { get; set; }
    }
}