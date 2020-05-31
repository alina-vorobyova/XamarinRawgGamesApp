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

        public Command GameDetailCommand { get; set; }
        public Command DislikeGameCommand { get; set; }

        public FavoriteGamesViewModel()
        {
            _favoriteGameService = DependencyService.Get<IFavoriteGameService>();
            GameDetailCommand = new Command<GameDetailedResponse>(GameDetails);
            DislikeGameCommand = new Command<GameDetailedResponse>(DislikeGame);
            LoadAllFavoriteGames();

            MessagingCenter.Subscribe<GameDetailViewModel, GameDetailedResponse>(this, "game_disliked", async (sender, message) =>
            {
                var gameForLikeCheck = FavoriteGames.FirstOrDefault(x => x.id == message.id);
                FavoriteGames.Remove(gameForLikeCheck);
                LoadAllFavoriteGames();
            });
        }

        private async Task LoadAllFavoriteGames()
        {
           var favGames = await _favoriteGameService.GetAllFavoriteGamesAsync();
           FavoriteGames = new ObservableCollection<GameDetailedResponse>(favGames);
        }

        private async void DislikeGame(GameDetailedResponse game)
        {
            game.IsLiked = false;
            await _favoriteGameService.DislikeGameAsync(game.id);
            await Application.Current.MainPage.DisplayAlert("Dislike :(", $"{game.name} removed to Favorites! 🎮", "Close");
            FavoriteGames.Remove(game);
            await LoadAllFavoriteGames();
        }

        private async void GameDetails(GameDetailedResponse game)
        {
            var detailPage = (Application.Current.MainPage as MasterDetailPage).Detail;
            await detailPage.Navigation.PushAsync(new GameDetailPage());
            MessagingCenter.Send(this, "game_details", game);
        }
    }
}
