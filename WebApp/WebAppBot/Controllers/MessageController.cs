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
        private object path;

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
            return Json(suggestedArticles);
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
            return string.IsNullOrEmpty(resp) ? Json("Tout ceci est bizarre") : Json(resp);
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
            return Json("Je ne comprends pas votre intention ...");
        }

        [HttpGet("like/{id}/{isPositive}")]
        public string GetLike([FromRoute]string articleId, [FromRoute]bool isPositive)
        {
            //MongoController.UpdatePreferences(1, articleId, isPositive);
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

        static CancellationTokenSource Cts;
        [HttpGet("load/")]
        public string Load()
        {
            if (Model.Count != 0) return "Le model est généré";
            Cts = new CancellationTokenSource();
            Cts.CancelAfter(5 * 60 * 1000);
            Task.Run(() => LongThread(), Cts.Token).
                ContinueWith((_) =>
                {
                    Ex = "fini";
                    Seconds = (DateTime.Now - Debut).TotalSeconds;
                });
            return "C'est partie";
        }

        static DateTime Debut;
        static string Ex = "";
        static double Seconds = 0;
        private void LongThread()
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
            return Model.ContainsKey(word).ToString() + " " + Model.Count + " " + Seconds + " " + Ex;
        }

        [HttpGet("UpdateArticles/")]
        public string UpdateArticles()
        {
            var Client = new MongoClient(MongoController.uri);
            var Db = Client.GetDatabase(MongoController.db);
            var collection = Db.GetCollection<Article>("articles");
            var filter = Builders<Article>.Filter.Empty;
            foreach (var item in collection.Find(filter).ToList())
            {
                var vecs = new List<float[]>();
                foreach (var key in item.KeyPhrases)
                {
                    if (Model.TryGetValue(key, out var vec))
                    {
                        vecs.Add(vec);
                    }
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
                    rep = new float[300];
                    for (int j = 0; j < 300; j++)
                    {
                        rep[j] = 0;
                    }
                }
                collection.UpdateOne(x => x.Id == item.Id, Builders<Article>.Update.Set("vector", rep));
            }
            return "nice";
        }
    }
}
