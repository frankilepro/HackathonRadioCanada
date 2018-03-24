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
        HttpClient hClient = new HttpClient();
        // GET api/messages
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/messages/:message
        [HttpGet("{message}")]
        public async Task<string> GetAsync(string message)
        {
            var res = await hClient.GetStringAsync("https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/2fe3ec12-071c-41c4-9e0c-948ca9c1198f?subscription-key=e11e7ae44c214a6b8cf28199afa0cdd0&verbose=true&timezoneOffset=0&q=" + message);
            Console.WriteLine(res);
            return res;
        }

        // POST api/messages
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/messages/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/messages/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
