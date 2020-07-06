using System;
using System.Collections.Generic;
using System.Text;

namespace GamesApp.ViewModels
{
    public class TitleViewModel : ViewModelBase
    {

        private string _searchGame;
        public string SearchGame
        {
            get => _searchGame;
            set => Set(ref _searchGame, value);
        }
    }
}
