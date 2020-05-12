using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GamesApp.Services;
using GamesApp.Services.GameApiClient;
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
