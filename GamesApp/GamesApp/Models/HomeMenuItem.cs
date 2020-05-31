using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.Models
{
    public enum MenuItemType
    {
        Home,
        Favorites
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
