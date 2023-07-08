using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class AttrOptionsV2
    {
        [JsonProperty("option")]
        public OptionV2 Option { get; set; }
    }

    public class OptionV2
    {
        [JsonProperty("attribute_id")]
        public string AttributeId { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("is_use_default")]
        public bool IsUseDefault { get; set; }

        [JsonProperty("values")]
        public List<OptionValueIndexV2> Values { get; set; }
    }

    public class OptionValueIndexV2
    {
        [JsonProperty("value_index")]
        public int ValueIndex { get; set; }
    }
}