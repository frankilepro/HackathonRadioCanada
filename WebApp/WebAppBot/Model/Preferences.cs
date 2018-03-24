using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    internal class Preferences
    {
        [BsonElement("motsClefs")]
        public Dictionary<string, int> MotsClefs { get; set; }
        [BsonElement("categories")]
        public Dictionary<string, int> Categories { get; set; }
        [BsonElement("history")]
        public List<string> History { get; set; }
    }
}