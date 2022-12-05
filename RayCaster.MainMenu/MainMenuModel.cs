using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCaster.MainMenu
{
    public class MainMenuModel : ObservableObject
    {
        private TimeSpan longestTime;

        public TimeSpan LongestTime
        {
            get { return longestTime; }
            set
            {
                SetProperty(ref longestTime, value);
            }
        }
        public MainMenuModel()
        {
        }
    }
}
