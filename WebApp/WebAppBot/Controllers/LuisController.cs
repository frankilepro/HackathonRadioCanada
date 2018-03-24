using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppBot.Controllers
{
    [Route("api/messages")]
    public class LuisController : Controller
    {
        [HttpGet("{message}")]
        public async Task<string> GetAsync(string message)
        {
            HttpClient hClient = new HttpClient();
            var res = await hClient.GetStringAsync(
                "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/4b27fd30-c27c-4f48-8e7a-db3fd54a4059?subscription-key=e11e7ae44c214a6b8cf28199afa0cdd0&verbose=true&timezoneOffset=0&q="
                + message);
            return res;
        }
    }
}
