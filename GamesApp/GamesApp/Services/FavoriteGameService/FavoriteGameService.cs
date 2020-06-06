using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace GamesApp.Services.LikedGameService
{
    class FavoriteGameService : IFavoriteGameService
    {
        private readonly IGameApiClient _gameApiClient;

        private Dictionary<int, GameDetailedResponse> FavoriteGames = new Dictionary<int, GameDetailedResponse>();
        private readonly string FileName = "savedGames.json";


        public FavoriteGameService()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
        }

        public async Task LikeGameAsync(int gameId)
        {
            var gameDetails = new GameDetailedResponse();
            string path = Path.Combine(FileSystem.AppDataDirectory, FileName);
            if (File.Exists(path))
            {
                var file = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(file))
                    FavoriteGames = JsonConvert.DeserializeObject<Dictionary<int, GameDetailedResponse>>(file);

                if (!FavoriteGames.ContainsKey(gameId))
                {
                    gameDetails = await _gameApiClient.GetGameByIdAsync(gameId);
                    gameDetails.IsLiked = true;
                    await SaveFavoriteGameToFileAsync(gameDetails);
                }
            }
            else
            {
                gameDetails = await _gameApiClient.GetGameByIdAsync(gameId);
                gameDetails.IsLiked = true;
                await SaveFavoriteGameToFileAsync(gameDetails);
            }
        }

        public async Task DislikeGameAsync(int gameId)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, FileName);
            if (File.Exists(path))
            {
                var file = File.ReadAllText(path);
                if(!string.IsNullOrEmpty(file))
                    FavoriteGames = JsonConvert.DeserializeObject<Dictionary<int, GameDetailedResponse>>(file);
                if (FavoriteGames.ContainsKey(gameId))
                    await DeleteFavoriteGameFromFileAsync(gameId);
            }
        }

        public async Task RemoveAllFavoriteGamesAsync()
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, FileName);
            if (File.Exists(path))
            {
                var file = File.ReadAllText(path);
                if(string.IsNullOrEmpty(file))
                    FavoriteGames = JsonConvert.DeserializeObject<Dictionary<int, GameDetailedResponse>>(file);
                if (FavoriteGames.Count > 0)
                {
                    FavoriteGames.Clear();
                    await SaveFileAsync(FavoriteGames);
                }
            }
        }

        public async Task<IEnumerable<GameDetailedResponse>> GetAllFavoriteGamesAsync()
        {
            var filename = Path.Combine(FileSystem.AppDataDirectory, FileName);
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (var stream = new StreamReader(fs))
                {
                    var file = await stream.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(file))
                    {
                        var favoriteGamesDictionary = JsonConvert.DeserializeObject<Dictionary<int, GameDetailedResponse>>(file);
                        return favoriteGamesDictionary.Select(x => x.Value);
                    }

                    return Enumerable.Empty<GameDetailedResponse>();
                }
            }
        }

        public async Task<bool> IsGameInFavorites(int id)
        {
            var favoriteGame = new GameDetailedResponse();
            if (FavoriteGames.Count == 0)
            {
                FavoriteGames = (await GetAllFavoriteGamesAsync()).ToDictionary(x => x.id, x => x);
                if (FavoriteGames.ContainsKey(id))
                    favoriteGame = FavoriteGames[id];
            }
            else
            {
                if (FavoriteGames.ContainsKey(id))
                    favoriteGame = FavoriteGames[id];
            }
            return favoriteGame.IsLiked;
        }

        private async Task SaveFavoriteGameToFileAsync(GameDetailedResponse game)
        {
            FavoriteGames.Add(game.id, game);
            await SaveFileAsync(FavoriteGames);
        }

        private async Task DeleteFavoriteGameFromFileAsync(int gameId)
        {
            FavoriteGames.Remove(gameId);
            await SaveFileAsync(FavoriteGames);

        }

        private async Task SaveFileAsync(Dictionary<int, GameDetailedResponse> favGames)
        {
            var json = JsonConvert.SerializeObject(favGames);
            var filename = Path.Combine(FileSystem.AppDataDirectory, FileName);

            using (var fs = new FileStream(filename, FileMode.Create))
            {
                using (var stream = new StreamWriter(fs))
                {
                    await stream.WriteAsync(json);
                }
            }
        }
    }
}
