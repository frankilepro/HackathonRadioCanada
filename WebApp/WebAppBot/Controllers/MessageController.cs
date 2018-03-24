using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAppBot.Data;
using WebAppBot.Model;

namespace WebAppBot.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        const string URL = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/4b27fd30-c27c-4f48-8e7a-db3fd54a4059?subscription-key=e11e7ae44c214a6b8cf28199afa0cdd0&verbose=true&timezoneOffset=0&q=";

        static int CurrentId = 0;

        [HttpGet("text/{text}")]
        public async Task<string> GetText([FromRoute]string text)
        {
            HttpClient hClient = new HttpClient();
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
            return "handle news";
        }

        private string HandleComment(Entity[] entities)
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

        private string HandleNone(Entity[] entities)
        {
            return "handle none";
        }

        [HttpGet("like/{id}/{isPositive}")]
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
    }
}
