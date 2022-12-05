using Newtonsoft.Json;
using RayCaster.MainMenu.AskForName;
using RayCaster.MainMenu.ContextWindow;
using RayCaster.MainMenu.LeaderboardWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RayCaster.MainMenu
{
    public class MainMenuLogic
    {
        public LeaderboardItem StartGame()
        {
            LeaderboardItem lbItem = new LeaderboardItem() { Name = "Name", Time = TimeSpan.Zero };
            AskForNameWindow askWindow = new AskForNameWindow(lbItem);
            askWindow.ShowDialog();

            RayCaster.MainWindow rayCaster = new RayCaster.MainWindow();
            DateTime startTime = DateTime.Now;
            rayCaster.ShowDialog();

            lbItem.Time = DateTime.Now.Subtract(startTime);
            return lbItem;
        }

        internal ObservableCollection<LeaderboardItem> LoadLeaderBoard()
        {
            var json = File.ReadAllText("lbItems.json");
            ObservableCollection<LeaderboardItem> lbItems = JsonConvert.DeserializeObject<ObservableCollection<LeaderboardItem>>(json);

            return lbItems;
        }

        internal void SaveLEaderBoard(ObservableCollection<LeaderboardItem> lbItems)
        {
            File.WriteAllText("lbItems.json", JsonConvert.SerializeObject(lbItems));
        }

        internal void ShowContext()
        {
            RayCaster.MainMenu.ContextWindow.ContextWindow contextWindow = new RayCaster.MainMenu.ContextWindow.ContextWindow();
            contextWindow.ShowDialog();
        }

        internal void ShowLeaderboard(ObservableCollection<LeaderboardItem> lbItems)
        {
            LeaderBoardWindow lbWIndow = new LeaderBoardWindow(lbItems);
            lbWIndow.ShowDialog();
        }
    }
}
