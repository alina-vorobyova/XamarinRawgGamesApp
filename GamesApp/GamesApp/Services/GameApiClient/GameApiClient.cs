using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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

        public async Task<GameApiResponse> GetAllNewReleasedGamesForLast30Days(int page)
        {
            var currentDate = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
            var firstDayCurMonth = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}-01";
            var requestUri = $"{url}/games?dates={firstDayCurMonth},{currentDate}&ordering=released&page_size=10&page={page}";
            var json =  await _httpClient.GetStringAsync(requestUri);
            var games = JsonConvert.DeserializeObject<GameApiResponse>(json);
            return games;
        }

        public Task<GameApiResponse> GetGamesByFilter(string year, string platform, string genre)
        {
            throw new NotImplementedException();
        }
    }
}
