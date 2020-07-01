using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;
using GamesApp.Services.LikedGameService;
using GamesApp.Views;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    class FavoriteGamesViewModel : ViewModelBase
    {
        private ObservableCollection<GameDetailedResponse> _favoriteGames = new ObservableCollection<GameDetailedResponse>();
        private readonly IFavoriteGameService _favoriteGameService;

        public ObservableCollection<GameDetailedResponse> FavoriteGames
        {
            get => _favoriteGames;
            set => Set(ref _favoriteGames, value);
        }

        private bool _isFavExists;
        public bool IsFavExists
        {
            get => _isFavExists;
            set => Set(ref _isFavExists, value);
        }


        public Command GameDetailCommand { get; set; }
        public Command DislikeGameCommand { get; set; }
        public Command DislikeAllGamesCommand { get; set; }


        public FavoriteGamesViewModel()
        {
            _favoriteGameService = DependencyService.Get<IFavoriteGameService>();
            GameDetailCommand = new Command<GameDetailedResponse>(GameDetails);
            DislikeGameCommand = new Command<GameDetailedResponse>(DislikeGame);
            DislikeAllGamesCommand = new Command(DislikeAllGames);
            LoadAllFavoriteGames();

            MessagingCenter.Subscribe<GameDetailViewModel, GameDetailedResponse>(this, "game_disliked", async (sender, message) =>
            {
                if(message != null)
                     await RemovedFromFavorites(message.id);
            });

            MessagingCenter.Subscribe<GameDetailViewModel, GameDetailedResponse>(this, "game_liked", async (sender, message) =>
            {
                if (message != null)
                    await AddedToFavorites(message.id);
            });

            MessagingCenter.Subscribe<GamesViewModel, Game>(this, "game_liked", async (sender, message) =>
            {
                if (message != null)
                    await AddedToFavorites(message.id);
            });

            MessagingCenter.Subscribe<GamesViewModel, Game>(this, "game_disliked", async (sender, message) =>
            {
                if (message != null)
                    await RemovedFromFavorites(message.id);
            });
        }

        private async Task AddedToFavorites(int gameId)
        {
            var gameForLikeCheck = FavoriteGames.FirstOrDefault(x => x.id != gameId);
            FavoriteGames.Add(gameForLikeCheck);
            await LoadAllFavoriteGames();
        }

        private async Task RemovedFromFavorites(int gameId)
        {
            var gameForLikeCheck = FavoriteGames.FirstOrDefault(x => x.id == gameId);
            FavoriteGames.Remove(gameForLikeCheck);
            await LoadAllFavoriteGames();
        }

        private async void DislikeAllGames()
        {
            await _favoriteGameService.RemoveAllFavoriteGamesAsync();
            FavoriteGames.Clear();
            MessagingCenter.Send(this, "all_games_disliked");
            if (FavoriteGames.Count > 0)
                IsFavExists = true;
            else
                IsFavExists = false;
        }


        private async Task LoadAllFavoriteGames()
        {
            var favGames = await _favoriteGameService.GetAllFavoriteGamesAsync();
            FavoriteGames = new ObservableCollection<GameDetailedResponse>(favGames);
            if (FavoriteGames.Count > 0)
            {
                IsFavExists = true;
                FavoriteGames = new ObservableCollection<GameDetailedResponse>(FavoriteGames.Reverse());
            }
            else
                IsFavExists = false;
        }

        private async void DislikeGame(GameDetailedResponse game)
        {
            game.IsLiked = false;
            await _favoriteGameService.DislikeGameAsync(game.id);
            await Application.Current.MainPage.DisplayAlert("Dislike :(", $"{game.name} removed to Favorites! 🎮", "Close");
            FavoriteGames.Remove(game);
            await LoadAllFavoriteGames();
            MessagingCenter.Send(this, "game_disliked", game);
        }

        private async void GameDetails(GameDetailedResponse game)
        {
            var detailPage = (Application.Current.MainPage as MasterDetailPage).Detail;
            await detailPage.Navigation.PushAsync(new GameDetailPage());
            MessagingCenter.Send(this, "game_details", game);
        }
    }
}
