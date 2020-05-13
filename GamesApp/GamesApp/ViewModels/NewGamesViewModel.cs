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
using GamesApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class NewGamesViewModel : ViewModelBase
    {
        private readonly IGameApiClient _gameApiClient;

        private ObservableCollection<Game> _newReleasedGames = new ObservableCollection<Game>();

        private bool _loading = false;

        private int _page;

        public ObservableCollection<Game> NewReleasedGames
        {
            get => _newReleasedGames;
            set => Set(ref _newReleasedGames, value);
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
        public  Command GameDetailCommand { get; set; }

        public NewGamesViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            LoadGamesFromApi();
            LoadMoreGames = new Command(LoadMoreGamesFromApi);
            GameDetailCommand = new Command<Game>(GameDetails);
        }

        private async void GameDetails(Game game)
        {
            var detailPage = (Application.Current.MainPage as MasterDetailPage).Detail;
            await detailPage.Navigation.PushAsync(new GameDetailPage());
            MessagingCenter.Send(this, "game_details", game);
        }


        private async void LoadGamesFromApi()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                CheckConnection = "";
                IsConnected = false;
                try
                {
                    _page = 1;
                    var games = await _gameApiClient.GetAllNewReleasedGamesForLast30Days(_page);
                    if (games != null)
                        NewReleasedGames = new ObservableCollection<Game>(games.results);
                }
                catch (Exception e)
                {

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
                        var games = await _gameApiClient.GetAllNewReleasedGamesForLast30Days(_page);
                        if (games != null)
                        {
                            foreach (var item in games.results)
                            {
                                NewReleasedGames.Add(item);
                            }
                            _loading = false;
                        }
                    }
                }
                catch (Exception e)
                {
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
