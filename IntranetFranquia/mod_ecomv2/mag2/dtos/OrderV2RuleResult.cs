using Newtonsoft.Json;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class OrderV2RuleResult
    {
        [JsonProperty("rule_id")]
        public int RuleId { get; set; }
    }
}