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
    public partial class SearchGamePage : ContentPage
    {
        public SearchGamePage()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel();
        }
    }
}