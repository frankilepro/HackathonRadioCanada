using System.Threading.Tasks;
using MongoDB.Driver;

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

        // public static async Task GetDocument<T>(T model, string collName)
        // {
        //     var collection = Db.GetCollection<T>(collName);
        //     await collection.FindAsync(model.Id);
        // }
    }
}