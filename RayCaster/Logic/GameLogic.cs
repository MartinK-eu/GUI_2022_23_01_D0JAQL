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
using System.Diagnostics;

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
            model = new GameModel(MapGenerator(), new Character(1, 1, 0.07, new Vector(0, 1), 0), true);
        }

        public int[,] MapGenerator()
        {
            string[] lines = File.ReadAllLines(@"..\..\..\Assets\map.txt");

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
            double oldPozX = model.Player.PozX;
            double oldPozY = model.Player.PozY;
            switch (control)
            {
                case Controls.Left:
                    model.Player.LookAngle -= 5;
                    break;
                case Controls.Right:
                    model.Player.LookAngle += 5;
                    break;
                case Controls.W:
                    model.Player.PozY -= Math.Sin(ToRadians(model.Player.LookAngle)) * model.Player.Speed;
                    model.Player.PozX += Math.Cos(ToRadians(model.Player.LookAngle)) * model.Player.Speed;
                    break;
                case Controls.A:
                    model.Player.PozY += Math.Sin(ToRadians(model.Player.LookAngle + 90)) * model.Player.Speed;
                    model.Player.PozX -= Math.Cos(ToRadians(model.Player.LookAngle + 90)) * model.Player.Speed;
                    break;
                case Controls.S:
                    model.Player.PozY += Math.Sin(ToRadians(model.Player.LookAngle)) * model.Player.Speed;
                    model.Player.PozX -= Math.Cos(ToRadians(model.Player.LookAngle)) * model.Player.Speed;
                    break;
                case Controls.D:
                    model.Player.PozY -= Math.Sin(ToRadians(model.Player.LookAngle + 90)) * model.Player.Speed;
                    model.Player.PozX += Math.Cos(ToRadians(model.Player.LookAngle + 90)) * model.Player.Speed;
                    break;
                case Controls.Map:
                    model.InMapMode = !model.InMapMode;
                    break;
                default:
                    break;
            }

            if (PlayerHitsWall(model.Player.PozX, model.Player.PozY, model.MapMatrix))
            {
                model.Player.PozX = oldPozX;
                model.Player.PozY = oldPozY;
            }
            Debug.WriteLine("X: " + model.Player.PozX);
            Debug.WriteLine("Y: " + model.Player.PozY);
        }

        private bool PlayerHitsWall(double pozX, double pozY, int[,] matrix)
        {
            Debug.WriteLine("roudned down x: " + (int)Math.Floor(pozX));
            Debug.WriteLine("roudned down y: " + (int)Math.Floor(pozY));

            Debug.WriteLine(matrix[(int)Math.Floor(pozX), (int)Math.Floor(pozY)]);
            if (matrix[(int)Math.Floor(pozY), (int)Math.Floor(pozX)] > 0)
                return true;
            else
                return false;
        }
        public static double ToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }
    }
}
