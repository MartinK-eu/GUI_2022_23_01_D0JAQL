using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCaster.MainMenu
{
    public class MainMenuLogic
    {
        public TimeSpan StartGame(TimeSpan currentLongest)
        {
            RayCaster.MainWindow rayCaster = new RayCaster.MainWindow();

            DateTime startTime = DateTime.Now;
            rayCaster.ShowDialog();
            return DateTime.Now.Subtract(startTime) > currentLongest ? DateTime.Now.Subtract(startTime) : currentLongest;
        }

        internal void ShowContext()
        {
            ContextWindow contextWindow = new ContextWindow();
            contextWindow.ShowDialog();
        }

        internal void ShowLeaderboard()
        {
            throw new NotImplementedException();
        }
    }
}
