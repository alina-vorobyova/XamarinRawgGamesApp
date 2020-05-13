using System;
using System.Collections.Generic;
using System.Text;
using GamesApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GamesApp.ViewModels
{
    public class GameDetailViewModel : ViewModelBase
    {
        private Game _game;
        public Game Game
        {
            get => _game;
            set => Set(ref _game, value);
        }
        public GameDetailViewModel()
        {
            MessagingCenter.Subscribe<NewGamesViewModel, Game>(this, "game_details", async (sender, message) =>
            {
                Game = message;
                Title = $"Details about: {message.name}";
            });
        }
    }
}
