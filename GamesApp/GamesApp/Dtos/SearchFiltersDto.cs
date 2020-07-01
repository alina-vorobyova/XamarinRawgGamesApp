using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GamesApp.Dtos
{
    public class SearchFiltersDto
    {
        public string Year { get; set; } = String.Empty;
        public string Platform { get; set; } = String.Empty;
        public string Genre { get; set; } = String.Empty;
    }
}
