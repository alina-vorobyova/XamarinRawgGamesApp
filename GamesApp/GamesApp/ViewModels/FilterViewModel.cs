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
    public class FilterViewModel : ViewModelBase
    {
        private readonly IGameApiClient _gameApiClient;

        private ObservableCollection<GenreResult> _genres = new ObservableCollection<GenreResult>();
        private ObservableCollection<PlatformResult> _platforms = new ObservableCollection<PlatformResult>();
        private Dictionary<string, string> FiltersDictionary = new Dictionary<string, string>();

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

        private string _yearParam;
        public string YearParam
        {
            get => _yearParam;
            set => Set(ref _yearParam, value);
        }

        private PlatformResult _platform;
        public PlatformResult Platform
        {
            get => _platform;
            set => Set(ref _platform, value);
        }

        private GenreResult _genre;

        public GenreResult Genre
        {
            get => _genre;
            set => Set(ref _genre, value);
        }

        public Command AddSearchFiltersCommand { get; set; }
        public Command CancelSearchFiltersCommand { get; set; }
        public Command ClearSearchFiltersCommand { get; set; }

        public FilterViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            LoadAllGenresAndPlatforms();
            AddSearchFiltersCommand = new Command(AddFilters);
            CancelSearchFiltersCommand = new Command(CancelFilters);
            ClearSearchFiltersCommand = new Command(ClearFilters);
        }

        private void ClearFilters()
        {
            MessagingCenter.Send(this, "clear_search_filters");
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void CancelFilters()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void AddFilters()
        {
            FiltersDictionary["year"] = YearParam;
            FiltersDictionary["genres"] = Genre?.slug;
            FiltersDictionary["platforms"] = Platform?.id.ToString();
            MessagingCenter.Send(this, "add_search_filters", FiltersDictionary);
            Application.Current.MainPage.Navigation.PopModalAsync();
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
