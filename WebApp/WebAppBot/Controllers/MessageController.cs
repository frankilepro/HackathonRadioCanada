using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Microsoft.WindowsAzure.Storage;
using MongoDB.Driver;
using Newtonsoft.Json;
using WebAppBot.Data;
using WebAppBot.Model;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAppBot.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        const string CONN = "DefaultEndpointsProtocol=https;AccountName=hackrc;AccountKey=u9HV3MqCpl+W9EuSgE9n7qVa/CN3DcMP0L1+P3nkJomfiElOEe7N0Fd9HeZpn5F6gPYsSzuXvp1uW+sYx1jHVA==;EndpointSuffix=core.windows.net";
        const string URL = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/4b27fd30-c27c-4f48-8e7a-db3fd54a4059?subscription-key=e11e7ae44c214a6b8cf28199afa0cdd0&verbose=true&timezoneOffset=0&q=";

        static ConcurrentDictionary<string, float[]> Model = new ConcurrentDictionary<string, float[]>();
        public static List<Article> Articles;
        public static Preference User;

        [HttpGet("text/{text}")]
        public async Task<JsonResult> GetText([FromRoute]string text)
        {
            var hClient = new HttpClient();
            var res = await hClient.GetStringAsync(URL + text);
            var luis = JsonConvert.DeserializeObject<LuisModel>(res);
            switch (luis.topScoringIntent.intent)
            {
                case "GetNews":
                    return HandleGetNews(luis.entities); ;
                case "Comment":
                    return HandleComment(luis.entities); ;
                case "None":
                default:
                    return HandleNone(luis.entities);
            }
        }

        private JsonResult HandleGetNews(Entity[] entities)
        {
            var lists = GetLists(entities);
            var suggestedArticles = MongoController.GetArticlesByEntities(lists.catLs, lists.dateLs);
            return Json(new ChatResponse { Type = 0, Value = suggestedArticles });
        }

        private JsonResult HandleComment(Entity[] entities)
        {
            (var catLs, var dateLs) = GetLists(entities);

            var builder = new StringBuilder();
            if (dateLs.Count != 0)
            {
                if (dateLs.Count == 1)
                {
                    builder.Append($"[ {dateLs[0].ToString("yyyy-MM-dd")} ] ");
                }
                else
                {
                    builder.Append($"[ {dateLs[0].ToString("yyyy-MM-dd")}, " +
                                     $"{dateLs[1].ToString("yyyy-MM-dd")} ] ");
                }
            }
            if (catLs.Count != 0)
            {
                builder.Append($"Je vois que tu aimes bien: {string.Join(" , ", catLs)}");
            }
            var resp = builder.ToString();
            return string.IsNullOrEmpty(resp) ?
                Json(new ChatResponse { Type = 1, Value = "Tout ceci est bizarre" }) :
                Json(new ChatResponse { Type = 1, Value = resp });
        }

        private (List<string> catLs, List<DateTime> dateLs) GetLists(Entity[] entities)
        {
            var categList = new List<string>();
            var dateList = new List<DateTime>();
            foreach (var item in entities)
            {
                if (item.type.StartsWith("Categorie"))
                {
                    categList.Add(item.entity.ToLower());
                }
                else if (item.type.EndsWith("date"))
                {
                    var date = item.resolution.values.First().value;
                    if (DateTime.TryParse(date, out var day))
                    {
                        dateList.Add(day);
                    }
                }
                else if (item.type.EndsWith("daterange"))
                {
                    var dateBegin = item.resolution.values.First().start;
                    var dateEnd = item.resolution.values.First().end;
                    if (DateTime.TryParse(dateBegin, out var day) && DateTime.TryParse(dateEnd, out var lDay))
                    {
                        while (day != lDay)
                        {
                            dateList.Add(day);
                            day = day.AddDays(1);
                        }
                    }
                }
            }
            return (categList, dateList);
        }

        private JsonResult HandleNone(Entity[] entities)
        {
            return Json(new ChatResponse { Type = 1, Value = "Je ne comprends pas votre intention ..." });
        }

        [HttpGet("like/{articleId}/{isPositive}")]
        public string GetLike([FromRoute]string articleId, [FromRoute]bool isPositive)
        {
            MongoController.UpdatePreferences(1, articleId, isPositive);
            if (isPositive)
            {
                return "Merci :)";
            }
            else
            {
                return "Nous allons mettre à jour vos préférences, désolé.";
            }
        }

        [HttpGet("download/")]
        public string Download()
        {
            if (CloudStorageAccount.TryParse(CONN, out var storageAccount))
            {
                var cloudContainerRef = storageAccount.CreateCloudBlobClient().
                        GetContainerReference("hackrccontainer");

                var cloudBlockBlob = cloudContainerRef.GetBlockBlobReference("wiki.fr.vec");
                cloudBlockBlob.DownloadToFileAsync("wiki.fr.vec", FileMode.Create).Wait();
            }
            else
            {
                return "bad";
            }
            return "good";
        }

        [HttpGet("load/")]
        public string Load()
        {
            if (Model.Count != 0) return "Le model est généré";
            Task.Run(() => LongLoadThread()).
                ContinueWith((_) =>
                {
                    Ex = "fini";
                    Seconds = (DateTime.Now - Debut).TotalSeconds;
                });
            Task.Run(() => Init());
            return "C'est partie";
        }

        private void Init()
        {
            var Client = new MongoClient(MongoController.uri);
            var Db = Client.GetDatabase(MongoController.db);
            var artCollection = Db.GetCollection<Article>("articles");
            var filter = Builders<Article>.Filter.Empty;
            Articles = artCollection.Find(filter).ToList();

            var userCollection = Db.GetCollection<Preference>("user");
            User = userCollection.Find(x => x.Id == 1).First();
        }

        static DateTime Debut;
        static string Ex = "";
        static double Seconds = 0;
        static double Counter = 0;

        private void LongLoadThread()
        {
            Debut = DateTime.Now;
            using (FileStream fs = System.IO.File.Open("wiki.fr.vec", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bs = new BufferedStream(fs))
                {
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line = sr.ReadLine();
                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                var splitted = line.Split(" ").Where(x => !string.IsNullOrEmpty(x) && x.Length > 2);
                                var toSkip = splitted.Count() - 300;
                                var word = string.Join(" ", splitted.Take(toSkip));
                                var vec = splitted.Skip(toSkip).
                                                   Select(x => float.Parse(x)).ToArray();
                                Model.TryAdd(word, vec);
                                Counter = Model.Count;
                            }
                            catch (Exception ex)
                            {
                                Ex = ex.Message;
                            }
                        }
                    }
                }
            }
            Seconds = (DateTime.Now - Debut).TotalSeconds;
        }

        [HttpGet("word/{word}")]
        public string Word([FromRoute]string word)
        {
            return Model.ContainsKey(word).ToString() + " " + Counter + " " + Seconds + " " + Ex;
        }

        [HttpGet("test/{test}")]
        public string Test([FromRoute]string test)
        {
            if (Articles == null) return "null";
            if (!int.TryParse(test, out int _) && Model.TryGetValue(test, out var vec))
            {
                var ls = Articles.OrderBy(x => DistanceBetweenVecs(x.Vector, vec));
                return string.Join(Environment.NewLine, ls.Select(x => x.Title));
            }
            var art = Articles.FirstOrDefault(x => x.Id == test);
            if (art != null)
            {
                var ls = Articles.OrderBy(x => DistanceBetweenVecs(x.Vector, art.Vector));
                return string.Join(Environment.NewLine, ls.Select(x => x.Title));
            }
            return "not in model";
        }

        [HttpGet("UpdateArticles/")]
        public string UpdateArticles()
        {
            Task.Run(() => LongUpdateThread()).
                ContinueWith((_) =>
                {
                    Ex = "fini";
                    Seconds = (DateTime.Now - Debut).TotalSeconds;
                });
            return "nice";
        }

        private void LongUpdateThread()
        {
            Counter = 0;
            Debut = DateTime.Now;
            var Client = new MongoClient(MongoController.uri);
            var Db = Client.GetDatabase(MongoController.db);
            var collection = Db.GetCollection<Article>("articles");
            if (Articles == null) return;
            foreach (var item in Articles.Reverse<Article>())
            {
                ++Counter;
                var vecs = new List<float[]>();
                foreach (var key in item.KeyPhrases)
                {
                    var subVecs = new List<float[]>();
                    foreach (var word in key.Split(" "))
                    {
                        if (Model.TryGetValue(word, out var vec))
                        {
                            subVecs.Add(vec);
                        }
                    }
                    float[] subRep;
                    if (subVecs.Count > 0)
                    {
                        subRep = subVecs[0];
                        for (int i = 1; i < subVecs.Count; i++)
                        {
                            for (int j = 0; j < 300; j++)
                            {
                                subRep[j] += subVecs[i][j];
                            }
                        }
                        for (int j = 0; j < 300; j++)
                        {
                            subRep[j] /= subVecs.Count;
                        }
                    }
                    else
                    {
                        subRep = Enumerable.Repeat(0.0f, 300).ToArray();
                    }
                    vecs.Add(subRep);
                }
                float[] rep;
                if (vecs.Count > 0)
                {
                    rep = vecs[0];
                    for (int i = 1; i < vecs.Count; i++)
                    {
                        for (int j = 0; j < 300; j++)
                        {
                            rep[j] += vecs[i][j];
                        }
                    }
                    for (int j = 0; j < 300; j++)
                    {
                        rep[j] /= vecs.Count;
                    }
                }
                else
                {
                    rep = Enumerable.Repeat(0.0f, 300).ToArray();
                }
                collection.UpdateOne(x => x.Id == item.Id, Builders<Article>.Update.Set("vector", rep));
            }
        }

        private double DistanceBetweenVecs(float[] vec1, float[] vec2)
        {
            double result = 0;
            for (int i = 0; i < 300; i++)
            {
                result += Math.Pow(vec1[i] - vec2[i], 2);
            }
            return Math.Sqrt(result);
        }
    }

    class ChatResponse
    {
        public int Type { get; set; }
        public object Value { get; set; }
    }
}
