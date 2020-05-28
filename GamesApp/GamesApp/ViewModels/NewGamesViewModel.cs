using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
using GamesApp.Services.LikedGameService;
using GamesApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class NewGamesViewModel : ViewModelBase
    {
        private readonly IGameApiClient _gameApiClient;
        private readonly IFavoriteGameService _favoriteGameService;

        private ObservableCollection<Game> _newReleasedGames = new ObservableCollection<Game>();
        private ObservableCollection<Game> _favoriteGames = new ObservableCollection<Game>();

        private bool _loading = false;

        private int _page;

        public ObservableCollection<Game> NewReleasedGames
        {
            get => _newReleasedGames;
            set => Set(ref _newReleasedGames, value);
        }

        public ObservableCollection<Game> FavoriteGames
        {
            get => _favoriteGames;
            set => Set(ref _favoriteGames, value);
        }

        private string _checkConnection;
        public string CheckConnection
        {
            get => _checkConnection;
            set => Set(ref _checkConnection, value);
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => Set(ref _isConnected, value);
        }
        
        public Command LoadMoreGames { get; set; }
        public Command GameDetailCommand { get; set; }
        public Command LikeGameCommand { get; set; }

        public NewGamesViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            _favoriteGameService = DependencyService.Get<IFavoriteGameService>();
            LoadGamesFromApi();
            LoadMoreGames = new Command(LoadMoreGamesFromApi);
            GameDetailCommand = new Command<Game>(GameDetails);
            LikeGameCommand = new Command<Game>(LikeGame);
        }


        private async Task<IEnumerable<Game>> CheckIsGameAlreadyLikedOrNot(List<Game> games)
        {
            bool isFav;
            foreach (var gameFromApi in games)
            {
                isFav = await _favoriteGameService.IsGameInFavorites(gameFromApi.id);
                gameFromApi.IsLiked = isFav;
            }

            return games;
        }

        private async Task<bool> CheckIsGameAlreadyLikedOrNot(int id)
        {
            bool isFav;
            isFav = await _favoriteGameService.IsGameInFavorites(id);
            return isFav;
        }

        private async void LikeGame(Game game)
        {
            game.IsLiked = true;
            await _favoriteGameService.LikeGameAsync(game.id);
            await Application.Current.MainPage.DisplayAlert("Like!", $"{game.name} added to Favorites! 🎮", "Close");
        }

        private async void GameDetails(Game game)
        {
            var detailPage = (Application.Current.MainPage as MasterDetailPage).Detail;
            await detailPage.Navigation.PushAsync(new GameDetailPage());
            MessagingCenter.Send(this, "game_details", game);
        }


        private async Task LoadGamesFromApi()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                CheckConnection = "";
                IsConnected = false;
                try
                {
                    _page = 1;
                    var games = await _gameApiClient.GetAllNewReleasedGamesForLast30DaysAsync(_page);
                    if (games != null)
                    {
                        var likesCheckedGames = await CheckIsGameAlreadyLikedOrNot(games.results);
                        NewReleasedGames = new ObservableCollection<Game>(likesCheckedGames);
                    }
                    else
                        await Application.Current.MainPage.DisplayAlert("Warning!", "Service is now unavailable. Please, try again later.", "Close");
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "Service is now unavailable. Please, try again later.", "Close");
                }
            }
            else
            {
                CheckConnection = "Check your connection";
                IsConnected = true;
            }
        }

        private async void LoadMoreGamesFromApi()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (!_loading)
                    {
                        _loading = true;
                        _page++;
                        var games = await _gameApiClient.GetAllNewReleasedGamesForLast30DaysAsync(_page);
                        if (games != null)
                        {
                            foreach (var item in games.results)
                            {
                                item.IsLiked = await CheckIsGameAlreadyLikedOrNot(item.id);
                                NewReleasedGames.Add(item);
                            }
                            _loading = false;
                        }
                        else
                            await Application.Current.MainPage.DisplayAlert("Warning!", "Service is now unavailable. Please, try again later.", "Close");
                    }
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Warning!", "Service is now unavailable. Please, try again later.", "Close");
                }
            }
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
                Application.Current.MainPage.DisplayAlert("Warning!", "Check your Internet connection!", "Close");
            else
            {
                Application.Current.MainPage.DisplayAlert("Hooray!", "Internet connection's back!", "Close");
                if (NewReleasedGames.Count > 0)
                    LoadMoreGamesFromApi();
                else
                    LoadGamesFromApi();
            }
        }

    }
}
