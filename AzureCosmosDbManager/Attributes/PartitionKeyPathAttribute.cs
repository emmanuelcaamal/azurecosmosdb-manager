using System;

namespace AzureCosmosDbManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PartitionKeyPathAttribute : Attribute
    {
        public PartitionKeyPathAttribute(string path) 
            => Path = path ?? throw new ArgumentNullException(nameof(path), "A name is required");

        public string Path { get; } = "/id";
    }
}
