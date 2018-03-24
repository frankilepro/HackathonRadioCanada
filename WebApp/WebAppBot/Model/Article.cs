using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    internal class Article
    {
        [BsonElement("id")]
        public string Id { get; set; }
        [BsonElement("vector")]
        public List<float> Vector { get; set; }
    }
}