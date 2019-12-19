using System;

namespace ToysStore.Attributes
{
    public class CacheAttribute : Attribute
    {
        public string Key { get; set; }
    }
}