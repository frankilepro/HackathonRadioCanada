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
        public async Task<string> GetText([FromRoute]string text)
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

        private string HandleGetNews(Entity[] entities)
        {
            if (entities.Length == 0) return "";

            string categoryType = "";
            foreach (var entity in entities)
            {
                if (entity.type.Contains("Categorie"))
                {
                    categoryType = entity.type.Replace("Categorie::", "");
                }
            }

            var suggestedArticles = MongoController.GetArticlesByCategory(categoryType);

            return JsonConvert.SerializeObject(suggestedArticles);
        }

        private string HandleComment(Entity[] entities)
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
            return string.IsNullOrEmpty(resp) ? "Tout ceci est bizarre" : resp;
        }

        private (List<string> catLs, List<DateTime> dateLs) GetLists(Entity[] entities)
        {
            var catLs = new List<string>();
            var dateLs = new List<DateTime>();
            foreach (var item in entities)
            {
                if (item.type.StartsWith("Categorie"))
                {
                    catLs.Add(item.entity);
                }
                else if (item.type.StartsWith("builtin"))
                {
                    var date = item.resolution.values.First().timex;
                    if (DateTime.TryParse(date, out var day))
                    {
                        dateLs.Add(day);
                    }
                }
            }
            return (catLs, dateLs);
        }

        private string HandleNone(Entity[] entities)
        {
            return "Je ne comprends pas votre intention ...";
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

        [HttpGet("load/")]
        public async Task<string> Load()
        {
            const int DefaultBufferSize = 4096;
            const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
            var debut = DateTime.Now;
            int count = 0;
            using (var stream = new FileStream("wiki.fr.vec", FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string line = await reader.ReadLineAsync();
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        ++count;
                        var splitted = line.Split(" ").Where(x => !string.IsNullOrEmpty(x));
                        var toSkip = splitted.Count() - 300;
                        var word = string.Join(" ", splitted.Take(toSkip));
                        Debug.WriteLine(word);
                        var vec = splitted.Skip(toSkip).
                                           Select(x => float.Parse(x)).ToArray();
                        Model.TryAdd(word, vec);
                    }
                }
            }
            //using (FileStream fs = System.IO.File.Open("wiki.fr.vec", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{
            //    using (BufferedStream bs = new BufferedStream(fs))
            //    {
            //        using (StreamReader sr = new StreamReader(bs))
            //        {
            //            string line = sr.ReadLine();
            //            while ((line = sr.ReadLine()) != null)
            //            {
            //                ++count;
            //                var splitted = line.Split(" ").Where(x => !string.IsNullOrEmpty(x));
            //                var toSkip = splitted.Count() - 300;
            //                var word = string.Join(" ", splitted.Take(toSkip));
            //                Debug.WriteLine(word);
            //                var vec = splitted.Skip(toSkip).
            //                                   Select(x => float.Parse(x)).ToArray();
            //                Model.Add(word, vec);
            //            }
            //        }
            //    }
            //}
            return (DateTime.Now - debut).TotalMilliseconds.ToString() + " " + Model.Count;
        }

        [HttpGet("{word}")]
        public string Word([FromRoute]string word)
        {
            return Model.ContainsKey(word).ToString() + " " + Model.Count;
        }
    }
}
