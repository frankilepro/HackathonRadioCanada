using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppBot.Model
{
    public class LuisModel
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public Intent[] intents { get; set; }
        public Entity[] entities { get; set; }
    }

    public class TopScoringIntent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public float score { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public Resolution resolution { get; set; }
    }

    public class Resolution
    {
        public Value[] values { get; set; }
    }

    public class Value
    {
        public string timex { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
}
