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
    public partial class CustomTitleView : ContentView
    {
        public CustomTitleView()
        {
            InitializeComponent();
            BindingContext = new GamesViewModel();
        }

        private async void Entry_OnCompleted(object sender, EventArgs e)
        {
            if (BindingContext is GamesViewModel gameViewModel)
            {
                if (!string.IsNullOrWhiteSpace(gameViewModel.SearchGame))
                {
                    var detailPage = (Application.Current.MainPage as MasterDetailPage)?.Detail;
                    await detailPage.Navigation?.PushAsync(new SearchGamePage());
                    MessagingCenter.Send(this, "search_game", gameViewModel.SearchGame);
                    SearchEntry.Text = "";
                }
            }
        }
    }
}