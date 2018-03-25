using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebAppBot.Controllers;
using WebAppBot.Model;

namespace WebAppBot.Data
{
    internal static class MongoController
    {
        public const string uri = "mongodb://hackrcdb:SPjTg21eZutZh3LhHE5hE1lMKB3oIrHT7aLOyqnihq30rqAifwy8Kbd5s2AYpkuk3gzzq3xevzZYF42uExh0SA==@hackrcdb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
        public const string db = "hackrcdb";

        private static MongoClient Client { get; set; }
        private static IMongoDatabase Db { get; set; }
        public static void Initialize()
        {
            Client = new MongoClient(uri);
            Db = Client.GetDatabase(db);
        }

        public static async Task PostDocument<T>(T doc, string collName)
        {
            var collection = Db.GetCollection<T>(collName);
            await collection.InsertOneAsync(doc);
        }

        public static void UpdatePreferences(int userId, string articleID, bool isPositive)
        {
            var userCollection = Db.GetCollection<Preference>("user");
            var user = MessageController.User;

            var article = MessageController.Articles.First(x => x.Id == articleID);

            var newVector = Enumerable.Repeat(0.0f, 300).ToArray();
            for (var i = 0; i < user.Vector.Length; i++)
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

            var res = userCollection.UpdateOne(x => x.Id == userId,
                Builders<Preference>.Update.Set("vector", newVector)
                                           .Set("nb", ++user.NbArticles)
                                           .Set("history", new List<string>(user.History) { article.Id }));
            if (res.MatchedCount == 0)
            {
                userCollection.InsertOne(new Preference
                {
                    Id = 1,
                    NbArticles = 1,
                    History = new List<string>() { article.Id },
                    Vector = newVector
                });
            }
        }

        public static List<Article> GetArticlesByEntities(List<string> catLs, List<DateTime> dateLs)
        {
            Random ran = new Random();
            var articlesCollection = Db.GetCollection<Article>("articles");
            var articles = new List<Article>();
            FilterDefinition<Article> filter;
            if (catLs.Count == 0)
            {
                if (dateLs.Count == 0)
                {
                    filter = Builders<Article>.Filter.Empty;
                    var req = articlesCollection.Find(filter);
                    var count = req.Count();
                    articles = req.Skip(ran.Next((int)count)).Limit(3).ToList();
                }
                else if (dateLs.Count == 1)
                {
                    filter = Builders<Article>.Filter.Regex("publishedLastTimeAt", dateLs.First().ToString("yyyy-MM-dd"));

                    var req = articlesCollection.Find(filter);
                    var count = req.Count();
                    articles = req.Skip(ran.Next((int)count)).Limit(3).ToList();
                }
                else
                {
                    filter = Builders<Article>.Filter.Regex("publishedLastTimeAt", "bidon");
                    foreach (var item in dateLs)
                    {
                        Console.WriteLine(item.ToString("yyyy-MM-dd"));
                        filter = Builders<Article>.Filter.Or(filter,
                            Builders<Article>.Filter.Regex("publishedLastTimeAt", item.ToString("yyyy-MM-dd")));
                    }
                    var req = articlesCollection.Find(filter);
                    var count = req.Count();
                    articles = req.Skip(ran.Next((int)count)).Limit(3).ToList();
                }
            }
            else
            {
                if (dateLs.Count == 0)
                {
                    Console.WriteLine(catLs.First().First().ToString().ToUpper() + catLs.First().Substring(1).ToLower());
                    filter = Builders<Article>.Filter.Regex("themeTag.name",
                        "Sport");
                    var req = articlesCollection.Find(filter);
                    var count = req.Count();
                    articles = req.Skip(ran.Next((int)count)).Limit(3).ToList();
                }
                else if (dateLs.Count == 1)
                {
                    filter = Builders<Article>.Filter.Regex("publishedLastTimeAt", dateLs.First().ToString("yyyy-MM-dd"));
                    filter = Builders<Article>.Filter.Or(filter, Builders<Article>.Filter.Regex("themeTag.name",
                        catLs.First().First().ToString().ToUpper() + catLs.First().Substring(1).ToLower()));

                    var req = articlesCollection.Find(filter);
                    var count = req.Count();
                    articles = req.Skip(ran.Next((int)count)).Limit(3).ToList();
                }
                else
                {
                    filter = Builders<Article>.Filter.Regex("publishedLastTimeAt", "bidon");
                    foreach (var item in dateLs)
                    {
                        Console.WriteLine(item.ToString("yyyy-MM-dd"));
                        filter = Builders<Article>.Filter.Or(filter,
                            Builders<Article>.Filter.Regex("publishedLastTimeAt", item.ToString("yyyy-MM-dd")));
                    }
                    filter = Builders<Article>.Filter.Or(filter, Builders<Article>.Filter.Regex("themeTag.name",
                        catLs.First().First().ToString().ToUpper() + catLs.First().Substring(1).ToLower()));

                    var req = articlesCollection.Find(filter);
                    var count = req.Count();
                    articles = req.Skip(ran.Next((int)count)).Limit(3).ToList();
                }
            }

            return articles;
        }
    }
}