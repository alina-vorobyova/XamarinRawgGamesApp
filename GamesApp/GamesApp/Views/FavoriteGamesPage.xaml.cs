using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GamesApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteGamesPage : ContentPage
    {
        public FavoriteGamesPage()
        {
            InitializeComponent();
            //BindingContext = new FavoriteGamesViewModel();
            BindingContext = DependencyService.Get<FavoriteGamesViewModel>();
        }
    }
}