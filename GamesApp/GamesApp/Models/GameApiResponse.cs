using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Models
{
    public class GameApiResponse
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public Game[] results { get; set; }
        public bool user_platforms { get; set; }
    }

    public class Game
    {
        public string slug { get; set; }
        public string name { get; set; }
        public int playtime { get; set; }
        public Platform[] platforms { get; set; }
        public Store[] stores { get; set; }
        public string released { get; set; }
        public bool tba { get; set; }
        public string background_image { get; set; }
        public double rating { get; set; }
        public double rating_top { get; set; }
        public object[] ratings { get; set; }
        public int ratings_count { get; set; }
        public int reviews_text_count { get; set; }
        public int added { get; set; }
        public object added_by_status { get; set; }
        public object metacritic { get; set; }
        public int suggestions_count { get; set; }
        public int id { get; set; }
        public object score { get; set; }
        public object clip { get; set; }
        public Tag[] tags { get; set; }
        public object user_game { get; set; }
        public int reviews_count { get; set; }
        public int community_rating { get; set; }
        public string saturated_color { get; set; }
        public string dominant_color { get; set; }
        public Short_Screenshots[] short_screenshots { get; set; }
        public Parent_Platforms[] parent_platforms { get; set; }
        public Genre[] genres { get; set; }
    }

    public class Platform
    {
        public Platform1 platform { get; set; }
    }

    public class Platform1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Store
    {
        public Store1 store { get; set; }
    }

    public class Store1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string language { get; set; }
        public int games_count { get; set; }
        public string image_background { get; set; }
    }

    public class Short_Screenshots
    {
        public int id { get; set; }
        public string image { get; set; }
    }

    public class Parent_Platforms
    {
        public Platform2 platform { get; set; }
    }

    public class Platform2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

}
