using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
using GamesApp.Services.LikedGameService;
using MediaManager;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly IGameApiClient _gameApiClient;
        private ObservableCollection<Game> _favoriteGames = new ObservableCollection<Game>();
        private readonly IFavoriteGameService _favoriteGameService;

        private GameDetailedResponse _game;
        public GameDetailedResponse Game
        {
            get => _game;
            set => Set(ref _game, value);
        }


        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => Set(ref _isConnected, value);
        }

        private string _checkConnection;
        public string CheckConnection
        {
            get => _checkConnection;
            set => Set(ref _checkConnection, value);
        }

        private bool _isVideoExists;
        public bool IsVideoExists
        {
            get => _isVideoExists;
            set => Set(ref _isVideoExists, value);
        }

        public Command ShareGameCommand { get; set; }
        public Command LikeGameCommand { get; set; }
        public Command DislikeGameCommand { get; set; }


        public GameDetailViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            _favoriteGameService = DependencyService.Get<IFavoriteGameService>();

            MessagingCenter.Subscribe<GamesViewModel, Game>(this, "game_details", async (sender, message) =>
            {
                LoadGameFromApi(message.id);
                Title = $"Details about: {message.name}";
            });

            MessagingCenter.Subscribe<FavoriteGamesViewModel, GameDetailedResponse>(this, "game_details", async (sender, message) =>
            {
                LoadGameFromApi(message.id);
                Title = $"Details about: {message.name}";
            });


            ShareGameCommand = new Command(ShareGame);
            LikeGameCommand = new Command(LikeGame);
            DislikeGameCommand = new Command(DislikeGame);
        }

        private async void LikeGame()
        {
            Game.IsLiked = true;
            await _favoriteGameService.LikeGameAsync(Game.id);
            await Application.Current.MainPage.DisplayAlert("Like!", $"{Game.name} added to Favorites! 🎮", "Close");
            MessagingCenter.Send(this, "game_liked", Game);
        }

        private async void DislikeGame()
        {
            Game.IsLiked = false;
            await _favoriteGameService.DislikeGameAsync(Game.id);
            await Application.Current.MainPage.DisplayAlert("Dislike :(", $"{Game.name} removed to Favorites! 🎮", "Close");
            MessagingCenter.Send(this, "game_disliked", Game);

        }

        private async Task<bool> CheckIsGameAlreadyLikedOrNot(int id)
        {
            bool isFav;
            isFav = await _favoriteGameService.IsGameInFavorites(id);
            return isFav;
        }

      
        private async void LoadGameFromApi(int gameId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                CheckConnection = "";
                IsConnected = false;
                try
                {
                    var game = await _gameApiClient.GetGameByIdAsync(gameId);
                    if (game != null)
                    { 
                        game.IsLiked = await CheckIsGameAlreadyLikedOrNot(gameId);
                        Game = game;
                        if (Game.clip != null)
                            IsVideoExists = true;
                        else
                            IsVideoExists = false;
                    }
                    //else
                    //    await Application.Current.MainPage.DisplayAlert("Warning!", "Service is now unavailable. Please, try again later.", "Close");
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

        private void ShareGame()
        {
            Share.RequestAsync(new ShareTextRequest
            {
                Text = Game.description,
                Title = Game.name
            });
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
            {
                if (e.NetworkAccess != NetworkAccess.Internet)
                    Application.Current.MainPage.DisplayAlert("Warning!", "Check your Internet connection!", "Close");
                else
                {
                    Application.Current.MainPage.DisplayAlert("Hooray!", "Internet connection's back!", "Close");
                }
            }

        }
    }
