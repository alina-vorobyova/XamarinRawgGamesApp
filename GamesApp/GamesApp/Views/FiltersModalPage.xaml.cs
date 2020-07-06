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
            BindingContext = DependencyService.Get<FilterViewModel>();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}