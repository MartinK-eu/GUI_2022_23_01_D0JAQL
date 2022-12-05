using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCaster.MainMenu
{
    public class LeaderboardItem : ObservableObject
    {
		private string name;

		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}

		private TimeSpan time;

		public TimeSpan Time
		{
			get { return time; }
			set { SetProperty(ref time, value); }
		}

		public LeaderboardItem()
		{
			Time = TimeSpan.Zero;
		}
	}
}
