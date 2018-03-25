using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    internal class Preference
    {
        [BsonElement("userid")]
        public int Id { get; set; }
        [BsonElement("vector")]
        public float[] Vector { get; set; }
        [BsonElement("nb")]
        public int NbArticles { get; set; }
        [BsonElement("history")]
        public List<string> History { get; set; }
    }
}