using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
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

        public Command LoadMoreGames { get; set; }

        public NewGamesViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            LoadGamesFromApi();
            LoadMoreGames = new Command(LoadMoreGamesFromApi);
        }

        private async void LoadGamesFromApi()
        {
            _page = 1;
            var games = await _gameApiClient.GetAllNewReleasedGamesForLast30Days(_page);
            NewReleasedGames = new ObservableCollection<Game>(games.results);
        }

        private async void LoadMoreGamesFromApi()
        {
            if (!_loading)
            {
                //try
                //{
                    _loading = true;
                    _page++;
                    var games = await _gameApiClient.GetAllNewReleasedGamesForLast30Days(_page);//.ConfigureAwait(true);
                    //NewReleasedGames = new ObservableCollection<Game>(NewReleasedGames.Concat(games.results));
                    foreach (var item in games.results)
                    {
                        //await Task.Delay(500);
                        NewReleasedGames.Add(item);
                    }
                    _loading = false;
                //}
                //catch (Exception e)
                //{
                //    _page--;
                //    _loading = false;
                //}
            }
        }

    }
}
