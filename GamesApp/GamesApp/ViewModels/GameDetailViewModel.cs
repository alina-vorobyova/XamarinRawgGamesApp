using System;
using System.Collections.Generic;
using System.Text;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
using MediaManager;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class GameDetailViewModel : ViewModelBase
    {
        private readonly IGameApiClient _gameApiClient;

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

        public GameDetailViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _gameApiClient = DependencyService.Get<IGameApiClient>();

            MessagingCenter.Subscribe<NewGamesViewModel, Game>(this, "game_details", async (sender, message) =>
            {
                LoadGameFromApi(message.id);
                Title = $"Details about: {message.name}";
            });

            ShareGameCommand = new Command(ShareGame);
        }

        private async void LoadGameFromApi(int gameId)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                CheckConnection = "";
                IsConnected = false;
                try
                {
                    var game = await _gameApiClient.GetGameById(gameId);
                    if (game != null)
                    {
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
