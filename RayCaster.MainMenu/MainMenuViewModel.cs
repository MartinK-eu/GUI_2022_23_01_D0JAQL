using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RayCaster.MainMenu
{
    public class MainMenuViewModel : ObservableRecipient
    {
        public MainMenuLogic logic;
        private MainMenuModel model;

        public MainMenuModel Model
        {
            get { return model; }
            set { SetProperty(ref model, value); }
        }

        public MainMenuViewModel()
        {
            logic = new MainMenuLogic();
            model = new MainMenuModel();

            model.LongestTime = TimeSpan.Zero;

            StartGame = new RelayCommand
            (
                () => 
                { 
                    model.LongestTime = logic.StartGame(model.LongestTime); 
                }
            );
            ShowContext = new RelayCommand
            (
                () => { logic.ShowContext(); }
            );
            ShowLeaderboard = new RelayCommand
            (
                () => { logic.ShowLeaderboard(); }
            );

        }
        public ICommand StartGame { get; set; }
        public ICommand ShowContext { get; set; }
        public ICommand ShowLeaderboard { get; set; }
    }
}
