using System;

namespace AzureCosmosDbManager.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ContainerNameAttribute : Attribute
    {
        public ContainerNameAttribute(string name) 
            =>  Name = name ?? throw new ArgumentNullException(nameof(name), "A name is required");    

        public string Name { get; }
    }
}
