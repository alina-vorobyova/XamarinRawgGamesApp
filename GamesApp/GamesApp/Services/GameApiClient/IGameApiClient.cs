using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;

namespace GamesApp.Services.GameApiClient
{
    interface IGameApiClient
    {
        Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(int page);

        Task<GameApiResponse> GetGamesByFilter(string year, string platform, string genre);

        Task<GameDetailedResponse> GetGameByIdAsync(int id);

        Task<GameApiResponse> GetGamesByNameAsync(string gameName, int page);
    }
}
