using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

class MongoController
{
    public static MongoClient Client { get; set; }
    public static IMongoDatabase Db { get; set; }
    public static void Initialize()
    {
        string uri = "mongodb://hackrcdb:SPjTg21eZutZh3LhHE5hE1lMKB3oIrHT7aLOyqnihq30rqAifwy8Kbd5s2AYpkuk3gzzq3xevzZYF42uExh0SA==@hackrcdb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
        string db = "hackrcdb";
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