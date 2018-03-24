using MongoDB.Bson.Serialization.Attributes;

namespace WebAppBot.Model
{
    internal class TestMongoModel
    {
        [BsonElement("nb")]
        public int NombreBidon { get; set; }
        [BsonElement("title")]
        public string Titre { get; set; }

        public TestMongoModel(int nb, string titre)
        {
            NombreBidon = nb;
            Titre = titre;
        }
    }
}