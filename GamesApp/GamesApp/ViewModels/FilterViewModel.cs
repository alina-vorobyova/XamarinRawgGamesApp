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
        private ObservableCollection<PlatformResult> _platforms = new ObservableCollection<PlatformResult>();
        public ObservableCollection<GenreResult> Genres
        {
            get => _genres;
            set => Set(ref _genres, value);
        }

        public ObservableCollection<PlatformResult> Platforms
        {
            get => _platforms;
            set => Set(ref _platforms, value);
        }

        public FilterViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            LoadAllGenresAndPlatforms();
        }

        private async Task LoadAllGenresAndPlatforms()
        {
            var genres = await _gameApiClient.GetAllGenresAsync();
            var platforms = await _gameApiClient.GetAllPlatforms();
            Genres = new ObservableCollection<GenreResult>(genres.results);
            Platforms = new ObservableCollection<PlatformResult>(platforms.results);
        }
    }
}
