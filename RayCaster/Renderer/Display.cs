using RayCaster.Logic;
using RayCaster.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace RayCaster.Renderer
{
    public class Display : FrameworkElement
    {
        public IGameModel model;
        Size size;
        int fov = 60;
        static SolidColorBrush wallColor = new SolidColorBrush(Color.FromRgb(148,142,68));
        static SolidColorBrush sideWallColor = new SolidColorBrush(Color.FromRgb(114, 109, 44));
        static SolidColorBrush floorColor = new SolidColorBrush(Color.FromRgb(76, 58, 11));
        static SolidColorBrush ceilingColor = Brushes.DarkKhaki;

        public void Resize(Size size)
        {
            this.size = size;
        }

        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            Random rng = new Random();
            if (model.InMapMode)
            {
                double rectSize = size.Height / model.MapMatrix.GetLength(0);
                double horizontalOffset = size.Width / 4;

                drawingContext.DrawRectangle(floorColor, new Pen(Brushes.Black, 0),
                                    new Rect(0, 0, size.Width, size.Height));

                for (int i = 0; i < model.MapMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < model.MapMatrix.GetLength(1); j++)
                    {
                        switch (model.MapMatrix[i, j])
                        {
                            case 0:
                                break;
                            case 1:
                                drawingContext.DrawRectangle(wallColor, new Pen(Brushes.Black, 0),
                                    new Rect(horizontalOffset + j * rectSize, i* rectSize, rectSize, rectSize));
                                break;
                            default:
                                break;
                        }
                    }
                }

                double ellipseRange = 5;
                var playerCenter = new Point(horizontalOffset + model.Player.PozX * rectSize - ellipseRange / 2, model.Player.PozY * rectSize - ellipseRange / 2);
                drawingContext.DrawEllipse(Brushes.Red, new Pen(Brushes.Black, 1),playerCenter,ellipseRange,ellipseRange);  

                DrawRays(drawingContext, playerCenter, rectSize);
            }
            else
            {
                drawingContext.DrawRectangle(floorColor, new Pen(Brushes.Black, 0),
                                    new Rect(0, size.Height / 2, size.Width, size.Height / 2));
                drawingContext.DrawRectangle(ceilingColor, new Pen(Brushes.Black, 0),
                                    new Rect(0, 0, size.Width, size.Height / 2));
                DrawWalls( drawingContext);
            }
        }
        private void DrawRays(DrawingContext drawingContext,Point playerCenter,double rectSize)
        {
            double rectWidth = 10;
            for (double rayDir = model.Player.LookAngle - fov / 2; rayDir <= model.Player.LookAngle + fov / 2; rayDir += rectWidth / size.Width * fov)
            {
                double lineLength = RayCalculator.CalculateRayLength(model.Player.PozX, model.Player.PozY, Math.Cos(GameLogic.ToRadians(rayDir)), -Math.Sin(GameLogic.ToRadians(rayDir)), model.MapMatrix, out int blockFace) * rectSize;

                var lineEnd = new Point(playerCenter.X + Math.Cos(GameLogic.ToRadians(rayDir)) * lineLength, playerCenter.Y - Math.Sin(GameLogic.ToRadians(rayDir)) * lineLength);
                drawingContext.DrawLine(new Pen(blockFace == 1 ? Brushes.Blue : Brushes.DarkBlue, 1), playerCenter, lineEnd);
            }
        }
        private void DrawWalls(DrawingContext drawingContext)
        {
            double rectWidth = 2;

            double rectX = 0;
            for (double rayDir = model.Player.LookAngle - fov / 2; rayDir <= model.Player.LookAngle + fov / 2; rayDir += rectWidth / size.Width * fov)
            {
                double rayLength = RayCalculator.CalculateRayLength(model.Player.PozX, model.Player.PozY, Math.Cos(GameLogic.ToRadians(rayDir)), -Math.Sin(GameLogic.ToRadians(rayDir)), model.MapMatrix, out int blockFace);

                double rectHeight = size.Height / rayLength;
                if(rectHeight > size.Height) rectHeight = size.Height;

                drawingContext.DrawRectangle(blockFace == 1 ? wallColor : sideWallColor, new Pen(Brushes.Black, 0), new Rect(rectX, size.Height / 2 - rectHeight / 2, rectWidth, rectHeight));

                rectX += rectWidth;
            }
        }
    }
}
