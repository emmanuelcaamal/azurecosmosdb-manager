using AzureCosmosDbManager.Attributes;
using AzureCosmosDbManager.Context;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureCosmosDbManager.ApiTest.Models
{
    [ContainerName("Products")]
    [PartitionKeyPath("/id")]
    public class Product : Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("presentations")]
        public List<ProductPresentation> Presentations { get; set; }
    }

    public class ProductPresentation
    {
        [JsonProperty("clave")]
        public string Clave { get; set; }
        [JsonProperty("description")]
        public string Descripcion { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
