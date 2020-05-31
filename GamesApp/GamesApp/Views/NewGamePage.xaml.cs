using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Models;
using GamesApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewGamePage : ContentPage
    {
        public NewGamePage()
        {
            InitializeComponent();
            BindingContext = new NewGamesViewModel();
        }


        private void OnRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            if (BindingContext is NewGamesViewModel newGamesViewModel)
            {
                newGamesViewModel.LoadMoreGames.Execute(null);
            }
        }

       
    }
}