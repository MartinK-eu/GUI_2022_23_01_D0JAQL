using RayCaster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RayCaster.Renderer
{
    public class Display : FrameworkElement
    {
        public IGameModel model;
        Size size;

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
            if (size.Width > 50 && size.Height > 50)
            {
                Random rng = new Random();
                if (model.InMapMode)
                {
                    double rectWidth = size.Width / model.MapMatrix.GetLength(1);
                    double rectHeight = size.Height / model.MapMatrix.GetLength(0);

                    for (int i = 0; i < model.MapMatrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < model.MapMatrix.GetLength(1); j++)
                        {
                            switch (model.MapMatrix[i, j])
                            {
                                case 0:
                                    break;
                                case 1:
                                    drawingContext.DrawRectangle(Brushes.Blue, new Pen(Brushes.Black, 0),
                                        new Rect(j * rectWidth, i*rectHeight, rectWidth, rectHeight));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }



                    drawingContext.PushTransform(new RotateTransform(model.Player.LookAngle, 
                        model.Player.PozX * (size.Width / model.MapMatrix.GetLength(0)) + 2.5, model.Player.PozY * (size.Height / model.MapMatrix.GetLength(1)) + 2.5));
                    drawingContext.DrawRectangle(Brushes.Red, new Pen(Brushes.Black, 0),
                                        new Rect(model.Player.PozX * (size.Width / model.MapMatrix.GetLength(0)), model.Player.PozY * (size.Height / model.MapMatrix.GetLength(1)), 5, 5));
                    drawingContext.Pop();

                    drawingContext.PushTransform(new RotateTransform(model.Player.LookAngle, model.Player.PozX * (size.Width / model.MapMatrix.GetLength(0)) + 2.5, model.Player.PozY * (size.Height / model.MapMatrix.GetLength(0)) + 2.5));
                    drawingContext.DrawLine(new Pen(Brushes.Black, 2.5), new Point(model.Player.PozX * (size.Width / model.MapMatrix.GetLength(0)) + 2.5, model.Player.PozY * (size.Height / model.MapMatrix.GetLength(0)) + 2.5), new Point(model.Player.PozX * (size.Width / model.MapMatrix.GetLength(0)) + 12.5, model.Player.PozY * (size.Height / model.MapMatrix.GetLength(0)) + 12.5));
                    drawingContext.Pop();
                }
                else
                {
                    double rectWidth = size.Width / 5;
                    double rectHeight = size.Height;


                    for (int i = 0; i < size.Width / 5; i++)
                    {
                        drawingContext.DrawRectangle(Brushes.Blue, new Pen(Brushes.Black, 0),
                            new Rect(i * 5, 0, rectWidth, rectHeight));
                    }

                    /*drawingContext.DrawRectangle(new SolidColorBrush(Color.FromRgb((byte)rng.Next(1, 254), (byte)rng.Next(1, 254), (byte)rng.Next(1, 254))), new Pen(Brushes.Black, 0),
                            new Rect(1, 0, rectWidth, rectHeight));*/
                }

            }
        }
    }
}
