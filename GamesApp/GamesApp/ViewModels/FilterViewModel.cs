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
    public class FilterViewModel : GamesViewModel
    {
        private readonly IGameApiClient _gameApiClient;

        private ObservableCollection<GenreResult> _genres = new ObservableCollection<GenreResult>();
        public ObservableCollection<GenreResult> Genres
        {
            get => _genres;
            set => Set(ref _genres, value);
        }

        public FilterViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            LoadAllGenres();
        }

        private async Task LoadAllGenres()
        {
            var genres = await _gameApiClient.GetAllGenresAsync();
            Genres = new ObservableCollection<GenreResult>(genres.results);
        }
    }
}
