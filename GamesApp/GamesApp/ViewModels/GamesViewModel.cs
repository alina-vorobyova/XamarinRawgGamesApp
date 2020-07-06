using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;
using GamesApp.Services.GameApiClient;
using GamesApp.Services.LikedGameService;
using GamesApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private readonly IGameApiClient _gameApiClient;
        protected readonly IFavoriteGameService _favoriteGameService;

        protected ObservableCollection<Game> _newReleasedGames = new ObservableCollection<Game>();

        protected Dictionary<string, string> FiltersDictionary = new Dictionary<string, string>();


        protected bool _loading = false;

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


        private int _remainingItemsThresholdn;
        public int RemainingItemsThreshold
        {
            get => _remainingItemsThresholdn;
            set => Set(ref _remainingItemsThresholdn, value);
        }

        private string _displaySelectedKindOfFilter;
        public string DisplaySelectedKindOfFilter
        {
            get => _displaySelectedKindOfFilter;
            set => Set(ref _displaySelectedKindOfFilter, value);
        }


        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set => Set(ref _isConnected, value);
        }

        private string _searchGame;
        public string SearchGame
        {
            get => _searchGame;
            set => Set(ref _searchGame, value);
        }

        public Command LoadMoreGamesCommand { get; set; }
        public Command GameDetailCommand { get; set; }
        public Command LikeGameCommand { get; set; }
        public Command DislikeGameCommand { get; set; }
        public Command OpenFiltersModalPage { get; set; }


        public Guid Id { get; set; }

        public GamesViewModel()
        {
            _gameApiClient = DependencyService.Get<IGameApiClient>();
            _favoriteGameService = DependencyService.Get<IFavoriteGameService>();
            GameDetailCommand = new Command<Game>(GameDetails);
            LikeGameCommand = new Command<Game>(LikeGame);
            DislikeGameCommand = new Command<Game>(DislikeGame);
            OpenFiltersModalPage = new Command(OpenFiltersPage);


            MessagingCenter.Subscribe<FilterViewModel>(this, "clear_search_filters", (sender) =>
            {
                FiltersDictionary.Clear();
                AddFilters();
            });


            MessagingCenter.Subscribe<FilterViewModel, Dictionary<string,string>>(this, "add_search_filters", (sender, message) =>
            {
                FiltersDictionary = message;
                AddFilters();
            });

            MessagingCenter.Subscribe<CustomTitleView, string>(this, "search_game", (sender, message) =>
            {
                SearchGame = message;
            });

            MessagingCenter.Subscribe<GameDetailViewModel, GameDetailedResponse>(this, "game_liked", async (sender, message) =>
            {
                await GameFavoriteInfoChanged(message.id);
            });

            MessagingCenter.Subscribe<GameDetailViewModel, GameDetailedResponse>(this, "game_disliked", async (sender, message) =>
            {
                await GameFavoriteInfoChanged(message.id);
            });

            MessagingCenter.Subscribe<FavoriteGamesViewModel, GameDetailedResponse>(this, "game_disliked", async (sender, message) =>
            {
                await GameFavoriteInfoChanged(message.id);
            });

            MessagingCenter.Subscribe<FavoriteGamesViewModel>(this, "all_games_disliked", async (sender) =>
            {
                var likesCheckedGames = await CheckIsGameAlreadyLikedOrNot(NewReleasedGames.ToList());
                NewReleasedGames = new ObservableCollection<Game>(likesCheckedGames);
            });
        }

        protected void AddFilters()
        {
            DisplaySelectedKindOfFilter = String.Empty;
            if (FiltersDictionary.Count > 0)
            {
                DisplaySelectedKindOfFilter += FiltersDictionary["year"] is null ? String.Empty : "Year ";
                DisplaySelectedKindOfFilter += FiltersDictionary["genres"] is null ? String.Empty : "Genre ";
                DisplaySelectedKindOfFilter += FiltersDictionary["platforms"] is null ? String.Empty : "Platform ";
            }
            MessagingCenter.Send(this, "filters_added");
        }

        protected async void OpenFiltersPage()
        {
            var filtersPage = (Application.Current.MainPage as MasterDetailPage)?.Detail;
            await filtersPage.Navigation?.PushModalAsync(new FiltersModalPage());
        }

        protected async Task GameFavoriteInfoChanged(int gameId)
        {
            var gameForLikeCheck = NewReleasedGames.FirstOrDefault(x => x.id == gameId);
            if (gameForLikeCheck != null)
                gameForLikeCheck.IsLiked = await CheckIsGameAlreadyLikedOrNot(gameForLikeCheck.id);
        }

        protected async Task<IEnumerable<Game>> CheckIsGameAlreadyLikedOrNot(List<Game> games)
        {
            bool isFav;
            foreach (var gameFromApi in games)
            {
                isFav = await _favoriteGameService.IsGameInFavorites(gameFromApi.id);
                gameFromApi.IsLiked = isFav;
            }

            return games;
        }

        protected async Task<bool> CheckIsGameAlreadyLikedOrNot(int id)
        {
            bool isFav;
            isFav = await _favoriteGameService.IsGameInFavorites(id);
            return isFav;
        }

        protected async void LikeGame(Game game)
        {
            game.IsLiked = true;
            await _favoriteGameService.LikeGameAsync(game.id);
            await Application.Current.MainPage.DisplayAlert("Like!", $"{game.name} added to Favorites! 🎮", "Close");
            MessagingCenter.Send(this, "game_liked", game);
        }

        protected async void DislikeGame(Game game)
        {
            game.IsLiked = false;
            await _favoriteGameService.DislikeGameAsync(game.id);
            await Application.Current.MainPage.DisplayAlert("Dislike :(", $"{game.name} removed to Favorites! 🎮", "Close");
            MessagingCenter.Send(this, "game_disliked", game);
        }

        protected async void GameDetails(Game game)
        {
            var detailPage = (Application.Current.MainPage as MasterDetailPage)?.Detail;
            await detailPage.Navigation?.PushAsync(new GameDetailPage());
            MessagingCenter.Send(this, "game_details", game);
        }


        protected async Task LoadGamesFromApi(GameApiResponse games)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var remainingItemsThresholdShouldInvoke = 0;
                var remainingItemsThresholdShouldNotInvoke = -1;

                CheckConnection = "";
                IsConnected = false;
                try
                {
                    if (games != null)
                    {
                        var likesCheckedGames = await CheckIsGameAlreadyLikedOrNot(games.results);
                        NewReleasedGames = new ObservableCollection<Game>(likesCheckedGames);
                        if (games.next != null)
                            RemainingItemsThreshold = 0;
                        else
                            RemainingItemsThreshold = -1;
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

        protected internal async void LoadMoreGamesFromApi(GameApiResponse games)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    if (!_loading)
                    {
                        _loading = true;
                        if (games != null)
                        {
                            foreach (var item in games.results)
                            {
                                item.IsLiked = await CheckIsGameAlreadyLikedOrNot(item.id);
                                NewReleasedGames.Add(item);
                                if (games.next != null)
                                    RemainingItemsThreshold = 0;
                                else
                                    RemainingItemsThreshold = -1;
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
    }
}
