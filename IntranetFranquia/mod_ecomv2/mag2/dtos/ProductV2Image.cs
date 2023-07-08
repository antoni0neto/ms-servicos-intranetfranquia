using Newtonsoft.Json;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2Image
    {
        [JsonProperty("entry")]
        public ProductV2ImageEntry Entry { get; set; }
    }

    public class ProductV2ImageEntry
    {
        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }

        [JsonProperty("content")]
        public ProductV2ImageContent Content { get; set; }
    }

    public class ProductV2ImageContent
    {
        [JsonProperty("base64_encoded_data")]
        public string Base64EncodedData { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}