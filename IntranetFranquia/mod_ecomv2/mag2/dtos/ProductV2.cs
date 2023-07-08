using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{
    public class ProductV2
    {
        [JsonProperty("product")]
        public ProductV2Detail Product { get; set; }
    }

    public class ProductV2Detail
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("attribute_set_id")]
        public int AttributeSetId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("type_id")]
        public string TypeId { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("extension_attributes")]
        public ProductV2ExtAttr ExtensionAttributes { get; set; }

        [JsonProperty("custom_attributes")]
        public List<ProductV2CustomAttr> CustomAttributes { get; set; }
    }

    public class ProductV2DetailSimple : ProductV2Detail
    {
        [JsonProperty("price")]
        public double Price { get; set; }
    }

    public class ProductV2ExtAttr
    {
        [JsonProperty("category_links")]
        public List<ProductV2ExtAttrCategoryLinks> CategoryLinks { get; set; }

        [JsonProperty("stock_item")]
        public ProductV2ExtAttrStockItem StockItem { get; set; }
    }

    public class ProductV2ExtAttrCategoryLinks
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }
    }

    public class ProductV2ExtAttrStockItem
    {
        [JsonProperty("qty")]
        public string Qty { get; set; }

        [JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }
    }

    public class ProductV2CustomAttr
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}