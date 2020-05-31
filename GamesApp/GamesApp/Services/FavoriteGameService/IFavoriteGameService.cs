using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;

namespace GamesApp.Services.LikedGameService
{
    public interface IFavoriteGameService
    {
        Task LikeGameAsync(int gameId);

        Task DislikeGameAsync(int gameId);

        Task RemoveAllFavoriteGamesAsync();

        Task<IEnumerable<GameDetailedResponse>> GetAllFavoriteGamesAsync();

        Task<bool> IsGameInFavorites(int id);
    }
}
