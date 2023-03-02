using AzureCosmosDbManager.Interfaces;
using Newtonsoft.Json;
using System;

namespace AzureCosmosDbManager.Context
{
    public class Item : IItem
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
