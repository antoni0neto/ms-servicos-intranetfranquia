using Newtonsoft.Json;
using Relatorios.mod_ecomv2.mag2.utils;
using System;
using System.Collections.Generic;

namespace Relatorios.mod_ecomv2.mag2.dtos
{

    public class ProductV2DetailResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

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

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("product_links")]
        public List<ProductV2ProductLinksResult> ProductLinks { get; set; }

        [JsonProperty("media_gallery_entries")]
        public List<ProductV2MediaGalleryEntriesResult> MediaGalleryEntries { get; set; }

        [JsonProperty("extension_attributes")]
        public ProductV2ExtAttrResult ExtensionAttributes { get; set; }

        [JsonProperty("custom_attributes")]
        public List<ProductV2CustomAttrResult> CustomAttributes { get; set; }
    }

    public class ProductV2DetailSimpleResult : ProductV2DetailResult
    {
        [JsonProperty("price")]
        public double Price { get; set; }
    }

    public class ProductV2ExtAttrResult
    {
        [JsonProperty("category_links")]
        public List<ProductV2ExtAttrCategoryLinksResult> CategoryLinks { get; set; }

        [JsonProperty("stock_item")]
        public ProductV2ExtAttrStockItemResult StockItem { get; set; }
    }

    public class ProductV2ExtAttrCategoryLinksResult
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }
    }

    public class ProductV2ExtAttrStockItemResult
    {
        [JsonProperty("item_id")]
        public int ItemId { get; set; }

        [JsonProperty("product_id")]
        public int ProductId { get; set; }

        [JsonProperty("stock_id")]
        public int StockId { get; set; }

        [JsonProperty("qty")]
        public string Qty { get; set; }

        [JsonProperty("is_in_stock")]
        public bool IsInStock { get; set; }
    }

    public class ProductV2MediaGalleryEntriesResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("file")]
        public string File { get; set; }
    }

    public class ProductV2ProductLinksResult
    {
        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("link_type")]
        public string LinkType { get; set; }

        [JsonProperty("linked_product_sku")]
        public string LinkedProductSku { get; set; }

        [JsonProperty("linked_product_type")]
        public string LinkedProductType { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }
    }

    public class ProductV2CustomAttrResult
    {
        [JsonProperty("attribute_code")]
        public string AttributeCode { get; set; }

        [JsonProperty("value")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> Value { get; set; }
    }
}