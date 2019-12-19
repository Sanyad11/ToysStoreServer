using System;
using ToysStore.Attributes;

namespace ToysStore
{
    public class ErrorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ErrorResponse(Errors error)
        {
            Id = (int)error;
            Name = error.ToString();
            Type t = error.GetType();
            var field = t.GetField(Name);
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            Description = attr.Description;
        }
    }
}