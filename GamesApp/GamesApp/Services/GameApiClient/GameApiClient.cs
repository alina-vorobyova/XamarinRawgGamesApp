using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using GamesApp.Dtos;
using GamesApp.Models;
using Newtonsoft.Json;

namespace GamesApp.Services.GameApiClient
{
    class GameApiClient : IGameApiClient

    {
        private const string urlApi = "https://api.rawg.io/api";
        private readonly HttpClient _httpClient;
        
        public GameApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(int page)
        {
            GameApiResponse games = null;
            var currentDate = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
            var thirtyDaysBeforeDateTime = DateTime.Today.AddDays(-30);
            var thirtyDaysBefore = $"{thirtyDaysBeforeDateTime.Year}-{thirtyDaysBeforeDateTime.Month:D2}-{thirtyDaysBeforeDateTime.Day:D2}";
            var requestUri = $"{urlApi}/games?dates={thirtyDaysBefore},{currentDate}&ordering=released&page_size=10&page={page}&ordering=-rating";
            var json =  await _httpClient.GetStringAsync(requestUri);
            if(json != null)
                 games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }

        public async Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(SearchFiltersDto searchFiltersDto, int page)
        {
            var filtersDictionary = new Dictionary<string, string>
            {
                { "year", searchFiltersDto.Year },
                { "platform", searchFiltersDto.Platform },
                { "genres", searchFiltersDto.Genre }
            };

            GameApiResponse games = null;
            var currentDate = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
            var thirtyDaysBeforeDateTime = DateTime.Today.AddDays(-30);
            var thirtyDaysBefore = $"{thirtyDaysBeforeDateTime.Year}-{thirtyDaysBeforeDateTime.Month:D2}-{thirtyDaysBeforeDateTime.Day:D2}";

            var requestUri = $"{urlApi}/games?dates={thirtyDaysBefore},{currentDate}&ordering=released&ordering=-rating&page_size=10&page={page}";

            StringBuilder requestUriWithFilters = new StringBuilder(requestUri);
            foreach (var queryItem in filtersDictionary)
            {
                if (!string.IsNullOrWhiteSpace(queryItem.Value))
                    requestUriWithFilters.Append($"&{queryItem.Key}={queryItem.Value}");
            }
           
            var json = await _httpClient.GetStringAsync(requestUriWithFilters.ToString());
            if (json != null)
                games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }

        public Task<GameApiResponse> GetGamesByFilter(string year, string platform, string genre, int page)
        {
            throw new NotImplementedException();
        }

        public async Task<GameDetailedResponse> GetGameByIdAsync(int id)
        {
            GameDetailedResponse game = null;
            var requestUri = $"{urlApi}/games/{id}";
            var json = await _httpClient.GetStringAsync(requestUri);
            if (json != null)
            {
                game = JsonConvert.DeserializeObject<GameDetailedResponse>(json);
                string pattern = @"<(.|\n)*?/>";
                game.description = Regex.Replace(game.description, pattern, string.Empty);
            }
            return game;
        }

        public async Task<GameApiResponse> GetGamesByNameAsync(string gameName, int page)
        {
            GameApiResponse games = null;
            var requestUri = $"{urlApi}/games?search={gameName}&ordering=released&page_size=10&page={page}&ordering=-rating";
            var json = await _httpClient.GetStringAsync(requestUri);
            if (json != null)
                games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }
    }
}
