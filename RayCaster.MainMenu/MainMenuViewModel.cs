using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RayCaster.MainMenu
{
    public class MainMenuViewModel : ObservableRecipient
    {
        public MainMenuLogic logic;

        private ObservableCollection<LeaderboardItem> leaderboardItems;

        public ObservableCollection<LeaderboardItem> LeaderboardItmes
        {
            get { return leaderboardItems; }
            set { SetProperty(ref leaderboardItems, value); }
        }


        public MainMenuViewModel()
        {
            logic = new MainMenuLogic();
            LeaderboardItmes = logic.LoadLeaderBoard();

            StartGame = new RelayCommand
            (
                () => 
                { 
                    LeaderboardItmes.Add(logic.StartGame()); 
                }
            );
            ShowContext = new RelayCommand
            (
                () => { logic.ShowContext(); }
            );
            ShowLeaderboard = new RelayCommand
            (
                () => { logic.ShowLeaderboard(leaderboardItems); }
            );

        }
        public ICommand StartGame { get; set; }
        public ICommand ShowContext { get; set; }
        public ICommand ShowLeaderboard { get; set; }

        internal void SaveLeaderBoard()
        {
            logic.SaveLEaderBoard(LeaderboardItmes);
        }
    }
}
