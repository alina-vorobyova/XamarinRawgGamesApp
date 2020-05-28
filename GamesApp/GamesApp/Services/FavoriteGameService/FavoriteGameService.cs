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
            if (File.Exists(FileName))
            {
                var file = File.ReadAllText(FileName);
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

        public Task DislikeGameAsync(int gameId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllFavoriteGamesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GameDetailedResponse>> GetAllFavoriteGamesAsync()
        {
            var filename = Path.Combine(FileSystem.AppDataDirectory, FileName);

            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                using (var stream = new StreamReader(fs))
                {
                    var file = await stream.ReadToEndAsync();
                    var favoriteGamesDictionary = JsonConvert.DeserializeObject<Dictionary<int, GameDetailedResponse>>(file);
                    return favoriteGamesDictionary.Select(x => x.Value);
                }
            }
        }

        public async Task<bool> IsGameInFavorites(int id)
        {
            var favoriteGame = new GameDetailedResponse();
            if (FavoriteGames.Count == 0)
            {
                try
                {
                    FavoriteGames = (await GetAllFavoriteGamesAsync()).ToDictionary(x => x.id, x => x);
                    if (FavoriteGames.ContainsKey(id))
                        favoriteGame = FavoriteGames[id];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
               
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
            var json = JsonConvert.SerializeObject(FavoriteGames);
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
