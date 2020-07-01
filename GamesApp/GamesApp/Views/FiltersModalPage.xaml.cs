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
    public partial class FiltersModalPage : ContentPage
    {
        public FiltersModalPage()
        {
            InitializeComponent();
            BindingContext = new GamesViewModel();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if(BindingContext is GamesViewModel gamesViewModel)
            {
                if(!string.IsNullOrWhiteSpace(gamesViewModel.YearParam))
                    MessagingCenter.Send(this, "search_filters", gamesViewModel.YearParam);
            }
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}