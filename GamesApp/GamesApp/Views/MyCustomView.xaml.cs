using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCustomView : ContentView
    {
        public MyCustomView()
        {
            InitializeComponent();
        }

        private void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is NewGamesViewModel newGamesViewModel)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    newGamesViewModel.LoadMoreGames();
            }

            if (BindingContext is SearchViewModel searchViewModel)
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    searchViewModel.LoadMoreGames();
            }
        }
    }
}