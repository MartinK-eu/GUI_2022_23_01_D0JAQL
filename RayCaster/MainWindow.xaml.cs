using RayCaster.Logic;
using RayCaster.Models;
using RayCaster.Renderer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace RayCaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameLogic logic;
        public MainWindow()
        {
            InitializeComponent();
            logic = new GameLogic();
            display.SetupModel(logic.model);

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 16; //base = 1, 60fps = 16ms, 30 = 33ms, 24fps = 41ms
            timer.Start();


        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            display.InvalidateVisual();
            Input();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            */
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            display.Resize(new Size(grid.ActualWidth, grid.ActualHeight));
            display.InvalidateVisual();
        }

        private void Input()
        {
            if (Keyboard.IsKeyDown(Key.W))
                logic.Control(Controls.W);
            if (Keyboard.IsKeyDown(Key.A))
                logic.Control(Controls.A);
            if (Keyboard.IsKeyDown(Key.S))
                logic.Control(Controls.S);
            if (Keyboard.IsKeyDown(Key.D))
                logic.Control(Controls.D);
            if (Keyboard.IsKeyDown(Key.Right))
                logic.Control(Controls.Right);
            if (Keyboard.IsKeyDown(Key.Left))
                logic.Control(Controls.Left);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.M))
                logic.Control(Controls.Map);
            if (Keyboard.IsKeyDown(Key.Escape))
            {
                this.DialogResult = true;
            }

        }
    }
}
