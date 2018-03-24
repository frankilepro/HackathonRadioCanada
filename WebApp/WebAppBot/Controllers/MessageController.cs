using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAppBot.Controllers
{
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        [HttpGet("{text}")]
        public string GetText([FromRoute]string text)
        {
            return "text api called";
        }

        [HttpGet("{vocal}")]
        public string GetVocal([FromRoute]string vocal)
        {
            return "vocal api called";
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
