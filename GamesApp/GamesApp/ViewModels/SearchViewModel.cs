﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Services.GameApiClient;
using GamesApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class SearchViewModel : GamesViewModel
    {
        private readonly IGameApiClient _gameApiClient;
        private int _page;
        public SearchViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            LoadNewGames();
            LoadMoreGamesCommand = new Command(LoadMoreGames);
            MessagingCenter.Subscribe<CustomTitleView, string>(this, "search_game", async (sender, message) =>
            {
                SearchGame = message;
                await LoadNewGames();
            });
        }

       

        public async Task LoadNewGames()
        {
            _page = 1;
            if (!string.IsNullOrWhiteSpace(SearchGame))
            {
                var games = await _gameApiClient.GetGamesByNameAsync(SearchGame, _page);
                await LoadGamesFromApi(games);
            }
        }

        public async void LoadMoreGames()
        {
            _page++;
            var games = await _gameApiClient.GetGamesByNameAsync(SearchGame, _page);
            LoadMoreGamesFromApi(games);
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet)
                Application.Current.MainPage.DisplayAlert("Warning!", "Check your Internet connection!", "Close");
            else
            {
                Application.Current.MainPage.DisplayAlert("Hooray!", "Internet connection is back!", "Close");
                if (NewReleasedGames.Count > 0)
                    LoadMoreGames();
                else
                    LoadNewGames();
            }
        }
    }
}
