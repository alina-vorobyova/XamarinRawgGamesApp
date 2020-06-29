using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamesApp.Models;
using Newtonsoft.Json;

namespace GamesApp.Services.GameApiClient
{
    class GameApiClient : IGameApiClient

    {
        private const string url = "https://api.rawg.io/api";
        private readonly HttpClient _httpClient;
        
        public GameApiClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(int page)
        {
            GameApiResponse games = null;
            var currentDate = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
            var firstDayCurMonth = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-01";
            var requestUri = $"{url}/games?dates={firstDayCurMonth},{currentDate}&ordering=released&page_size=10&page={page}&ordering=-rating";
            var json =  await _httpClient.GetStringAsync(requestUri);
            if(json != null)
                 games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }

        public Task<GameApiResponse> GetGamesByFilter(string year, string platform, string genre)
        {
            throw new NotImplementedException();
        }

        public async Task<GameDetailedResponse> GetGameByIdAsync(int id)
        {
            GameDetailedResponse game = null;
            var requestUri = $"{url}/games/{id}";
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
            var requestUri = $"{url}/games?search={gameName}&ordering=released&page_size=10&page={page}&ordering=-rating";
            var json = await _httpClient.GetStringAsync(requestUri);
            if (json != null)
                games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }
    }
}
