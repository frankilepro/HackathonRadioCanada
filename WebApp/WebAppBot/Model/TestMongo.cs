using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    [BsonIgnoreExtraElements]
    internal class TestMongo
    {
        [BsonElement("_id")]
        public ObjectId _Id { get; set; }
        
        [BsonElement("nb")]
        public int NombreBidon { get; set; }
    }
}