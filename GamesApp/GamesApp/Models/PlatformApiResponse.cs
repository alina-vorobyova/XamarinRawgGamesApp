using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Models
{
    public class PlatformApiResponse
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public PlatformResult[] results { get; set; }
    }

    public class PlatformResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
        public object image { get; set; }
        public int? year_start { get; set; }
        public object year_end { get; set; }
        public Game[] games { get; set; }
    }


}
