using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GamesApp.Services;
using GamesApp.Services.GameApiClient;
using GamesApp.Services.LikedGameService;
using GamesApp.ViewModels;
using GamesApp.Views;
using Xamarin.Essentials;

namespace GamesApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IGameApiClient, GameApiClient>();
            DependencyService.Register<IFavoriteGameService, FavoriteGameService>();
            DependencyService.Register<GamesViewModel>();
            DependencyService.Register<SearchViewModel>();
            DependencyService.Register<NewGamesViewModel>();
            DependencyService.Register<FavoriteGamesViewModel>();
            DependencyService.Register<FilterViewModel>();
            DependencyService.Register<GameDetailViewModel>();
            DependencyService.Register<TitleViewModel>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
