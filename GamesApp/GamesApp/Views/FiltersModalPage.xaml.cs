using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesApp.Dtos;
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
            BindingContext = new FilterViewModel();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            //if(BindingContext is GamesViewModel gamesViewModel)
            //{
            //    if (!string.IsNullOrWhiteSpace(gamesViewModel.YearParam) ||
            //        !string.IsNullOrWhiteSpace(gamesViewModel.Genre))
            //    {
            //        var filters = new SearchFiltersDto()
            //        {
            //            Year = gamesViewModel.YearParam,
            //            Genre = gamesViewModel.Genre
            //        };
            //        MessagingCenter.Send(this, "search_filters", filters);
            //    }
            //}
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}