using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GamesApp.Dtos;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
using GamesApp.Services.LikedGameService;
using GamesApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class NewGamesViewModel : GamesViewModel
    {
        private readonly IGameApiClient _gameApiClient;
        private int _page;
        public NewGamesViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            LoadNewGames();
            LoadMoreGamesCommand = new Command(LoadMoreGames);

            MessagingCenter.Subscribe<GamesViewModel>(this, "filters_added", (sender) =>
            {
                LoadNewGames();
            });

            //MessagingCenter.Subscribe<GamesViewModel, string>(this, "search_filters_info_for_display", async (sender, message) =>
            //{
            //    DisplaySelectedKindOfFilter = message;
            //    //LoadNewGames();
            //});
        }

        public async void LoadNewGames()
        {
            _page = 1;
             var games = await _gameApiClient.GetAllNewReleasedGamesForLast30DaysAsync(FiltersDictionary, _page);
            await LoadGamesFromApi(games);
        }

        public async void LoadMoreGames()
        {
            _page++;
            var games = await _gameApiClient.GetAllNewReleasedGamesForLast30DaysAsync(FiltersDictionary, _page);
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
