using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RayCaster.Models
{
    public class Character
    {
        public double PozY { get; set; }
        public double PozX { get; set; }
        public double Speed { get; set; }
        public Vector MoveDirection { get; set; }
        public int LookAngle { get; set; }

        public Character(double pozY, double pozX, double speed, Vector moveDirection, int lookAngle)
        {
            PozY = pozY;
            PozX = pozX;
            Speed = speed;
            MoveDirection = moveDirection;
            LookAngle = lookAngle;
        }
    }
}
