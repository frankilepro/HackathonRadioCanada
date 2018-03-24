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
        [BsonElement("canonicalWebLink")]
        public CanonicalWebLink canonicalWebLink { get; set; }

        [BsonElement("summary")]
        public string summary { get; set; }
        
        [BsonElement("title")]
        public string title { get; set; }

        [BsonElement("themeTag")]
        public ThemeTag themeTag { get; set; }
        public class ThemeTag
        {
            [BsonElement("name")]
            public string name { get; set; }
        }

        public class CanonicalWebLink
        {
            [BsonElement("href")]
            public string href { get; set; }
        }
    }
}