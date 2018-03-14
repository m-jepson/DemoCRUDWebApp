using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DependencyInjectionCRUDWebApp.Models
{
    public class Customer
    {
        [BsonId]
        public int? Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
