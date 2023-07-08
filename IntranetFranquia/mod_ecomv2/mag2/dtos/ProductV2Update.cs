using Newtonsoft.Json;
using Relatorios.mod_ecomv2.mag2.utils;
using System;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2Update
    {
        [JsonProperty("product")]
        public ProductV2UpdateDetail Product { get; set; }
    }

    public class ProductV2UpdateDetail
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("type_id")]
        public string TypeId { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("extension_attributes")]
        public ProductV2UpdateExtAttr ExtensionAttributes { get; set; }

        [JsonProperty("custom_attributes")]
        public List<ProductV2UpdateCustomAttr> CustomAttributes { get; set; }
    }

    public class ProductV2UpdateDetailSimple : ProductV2UpdateDetail
    {
        [JsonProperty("price")]
        public double Price { get; set; }
    }

    public class ProductV2UpdateExtAttr
    {
        [JsonProperty("category_links")]
        public List<ProductV2UpdateExtAttrCategoryLinks> CategoryLinks { get; set; }

    }

    public class ProductV2UpdateExtAttrCategoryLinks
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }
    }

    public class ProductV2UpdateCustomAttr
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        //[JsonProperty("value")]
        //public string Value { get; set; }

        [JsonProperty("value")]
        public List<string> Value { get; set; }
    }
}