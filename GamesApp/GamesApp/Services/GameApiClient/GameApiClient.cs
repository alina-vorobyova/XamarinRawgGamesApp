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

     
        public async Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(Dictionary<string, string> searchFilters, int page)
        {
            GameApiResponse games = null;
            var currentDate = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
            var thirtyDaysBefore = -30;
            var thirtyDaysBeforeDateTime = DateTime.Today.AddDays(thirtyDaysBefore);
            var thirtyDaysBeforeToday = $"{thirtyDaysBeforeDateTime.Year}-{thirtyDaysBeforeDateTime.Month:D2}-{thirtyDaysBeforeDateTime.Day:D2}";

            var requestUri = $"{urlApi}/games?dates={thirtyDaysBeforeToday},{currentDate}&ordering=released&ordering=-rating&page_size=10&page={page}";

            var requestUriWithFilters = AddFiltersToUri(searchFilters, requestUri);

            var json = await _httpClient.GetStringAsync(requestUriWithFilters);
            if (json != null)
                games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
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


        public async Task<GameApiResponse> GetGamesByNameAsync(Dictionary<string, string> searchFilters, string gameName, int page)
        {
            GameApiResponse games = null;
            var requestUri = $"{urlApi}/games?search={gameName}&page_size=10&page={page}&ordering=-rating";
            var requestUriWithFilters = AddFiltersToUri(searchFilters, requestUri);

            var json = await _httpClient.GetStringAsync(requestUriWithFilters);
            if (json != null)
                games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }

        public async Task<GenreApiResponse> GetAllGenresAsync()
        {
            GenreApiResponse genreApiResponse = null;
            var requestUri = $"{urlApi}/genres";
            var json = await _httpClient.GetStringAsync(requestUri);
            if (json != null)
                genreApiResponse = JsonConvert.DeserializeObject<GenreApiResponse>(json);
            return genreApiResponse;
        }

        public async Task<PlatformApiResponse> GetAllPlatforms()
        {
            PlatformApiResponse platformsApiResponse = null;
            var requestUri = $"{urlApi}/platforms";
            var json = await _httpClient.GetStringAsync(requestUri);
            if (json != null)
                platformsApiResponse = JsonConvert.DeserializeObject<PlatformApiResponse>(json);
            return platformsApiResponse;
        }

        private string AddFiltersToUri(Dictionary<string, string> searchFilters, string requestUri)
        {
            var requestUriWithFilters = new StringBuilder(requestUri);
            foreach (var queryItem in searchFilters)
            {
                if (!string.IsNullOrWhiteSpace(queryItem.Value))
                    requestUriWithFilters.Append($"&{queryItem.Key}={queryItem.Value}");
            }

            return requestUriWithFilters.ToString();
        }

    }
}
