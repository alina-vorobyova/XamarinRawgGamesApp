using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;

namespace GamesApp.Services.GameApiClient
{
    interface IGameApiClient
    {
        Task<GameApiResponse> GetAllNewReleasedGamesForLast30Days(int page);

        Task<GameApiResponse> GetGamesByFilter(string year, string platform, string genre);

        Task<GameDetailedResponse> GetGameById(int id);
    }
}
