using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RayCaster.MainMenu.LeaderboardWindow
{
    /// <summary>
    /// Interaction logic for LeaderBoardWindow.xaml
    /// </summary>
    public partial class LeaderBoardWindow : Window
    {
        public LeaderBoardWindow(ObservableCollection<LeaderboardItem> leaderboardItems)
        {
            InitializeComponent();
            listbox.ItemsSource = leaderboardItems;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
