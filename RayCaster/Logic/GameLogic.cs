using RayCaster.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Automation.Text;

namespace RayCaster.Logic
{
    public enum Controls
    {
        Left, Right, W, A, S, D, Map
    }
    public class GameLogic
    {
        public GameModel model;

        public GameLogic()
        {
            this.model = new GameModel(MapGenerator(), new Character(1,1,0.1,new Vector(0, 9), 0), false);
            model.Player.MoveDirection.Normalize();

        }

        public int[,] MapGenerator()
        {
            string[] lines = File.ReadAllLines("map1.txt");

            int[,] matrix = new int[lines.Length, lines.Length];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = ConvertToMapInt(lines[i][j]);
                }
            }

            return matrix;
        }

        private int ConvertToMapInt(char v)
        {
            switch (v)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                default:
                    return 0;
            }
        }

        public void Control(Controls control)
        {
            switch (control)
            {
                case Controls.Left:
                    model.Player.LookAngle += 5;
                    break;
                case Controls.Right:
                    model.Player.LookAngle -= 5;
                    break;
                case Controls.W:
                    model.Player.PozY -= model.Player.Speed;
                    break;
                case Controls.A:
                    model.Player.PozX -= model.Player.Speed;
                    break;
                case Controls.S:
                    model.Player.PozY += model.Player.Speed;
                    break;
                case Controls.D:
                    model.Player.PozX += model.Player.Speed;
                    break;
                case Controls.Map:
                    model.InMapMode = !model.InMapMode;
                    break;
                default:
                    break;
            }
        }
    }
}
