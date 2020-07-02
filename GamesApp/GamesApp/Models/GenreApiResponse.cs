using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Models
{
    public class GenreApiResponse
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public GenreResult[] results { get; set; }
        public string description { get; set; }
        public string seo_title { get; set; }
        public string seo_description { get; set; }
        public string seo_h1 { get; set; }
    }

    public class GenreResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
        public Game[] games { get; set; }
        public bool following { get; set; }
    }
}
