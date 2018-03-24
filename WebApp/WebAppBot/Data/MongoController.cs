using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebAppBot.Model;

namespace WebAppBot.Data
{
    internal static class MongoController
    {
        private static MongoClient Client { get; set; }
        private static IMongoDatabase Db { get; set; }
        public static void Initialize()
        {
            const string uri = "mongodb://hackrcdb:SPjTg21eZutZh3LhHE5hE1lMKB3oIrHT7aLOyqnihq30rqAifwy8Kbd5s2AYpkuk3gzzq3xevzZYF42uExh0SA==@hackrcdb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            const string db = "hackrcdb";
            Client = new MongoClient(uri);
            Db = Client.GetDatabase(db);
        }

        public static async Task PostDocument<T>(T doc, string collName)
        {
            var collection = Db.GetCollection<T>(collName);
            await collection.InsertOneAsync(doc);
        }

        public static void UpdatePreferences(int userId, string articleId, bool isPositive)
        {
            var userCollection = Db.GetCollection<Preference>("user");
            var user = userCollection.Find(x => x.Id == userId).First();

            var articleCollection = Db.GetCollection<Article>("articles");
            var article = articleCollection.Find(x => x.Id == articleId).First();

            List<float> newVector = new List<float>();
            for (int i = 0; i < user.Vector.Count; i++)
            {
                if (isPositive)
                {
                    newVector[i] = (user.NbArticles * user.Vector[i] + article.Vector[i]) / (user.NbArticles + 1);
                }
                else
                {
                    newVector[i] = ((user.NbArticles + 1) * user.Vector[i] - article.Vector[i]) / user.NbArticles;
                }
            }

            userCollection.UpdateOne(x => x.Id == userId, Builders<Preference>.Update.Set("vector", newVector));
        }

        public static List<Article> GetArticlesByCategory(string cat)
        {
            var articlesCollection = Db.GetCollection<Article>("articles");
            var articles = articlesCollection.Find(x => x.themeTag.name.Contains(cat)).ToList().Take(3).ToList();

            return articles;
        }
    }
}