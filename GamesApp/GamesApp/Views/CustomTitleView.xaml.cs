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
            BindingContext = new TitleViewModel();
            //BindingContext = DependencyService.Get<TitleViewModel>();
        }

        private async void Entry_OnCompleted(object sender, EventArgs e)
        {
            if (BindingContext is TitleViewModel titleViewModel)
            {
                if (!string.IsNullOrWhiteSpace(titleViewModel.SearchGame))
                {
                    MessagingCenter.Send(this, "search_game", titleViewModel.SearchGame);
                    var detailPage = (Application.Current.MainPage as MasterDetailPage)?.Detail;
                    await detailPage.Navigation?.PushAsync(new SearchGamePage());
                    SearchEntry.Text = "";
                }
            }
        }
    }
}