using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Dtos;
using GamesApp.Models;

namespace GamesApp.Services.GameApiClient
{
    interface IGameApiClient
    {
        Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(int page);
        Task<GameApiResponse> GetAllNewReleasedGamesForLast30DaysAsync(Dictionary<string, string> searchFilters, int page);
        Task<GameDetailedResponse> GetGameByIdAsync(int id);
        Task<GameApiResponse> GetGamesByNameAsync(string gameName, int page);
        Task<GenreApiResponse> GetAllGenresAsync();

        Task<PlatformApiResponse> GetAllPlatforms();
    }
}
