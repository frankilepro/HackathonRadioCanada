using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebAppBot.Data;
using WebAppBot.Model;

namespace WebAppBot.Controllers
{
    [Route("api/preferences")]
    public class PreferencesController : Controller
    {
        [HttpGet("{id}/{isPositive}")]
        public void Get(string id, bool isPositive)
        {
            MongoController.UpdatePreferences(1, id, isPositive);
        }
    }
}
