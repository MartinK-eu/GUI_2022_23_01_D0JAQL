using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.Media3D;

namespace RayCaster.Logic
{
    internal static class RayCalculator
    {
        public static double CalculateRayLength(double playerX,double playerY, double playerLookXRad, double playerLookYRad,int[,] map,out int face)
        {
            double xDeltaDist = Math.Abs(1 / playerLookXRad);
            double yDeltaDist = Math.Abs(1 / playerLookYRad);

            double xRayDist = 0;
            double yRayDist = 0;

            int mapX = (int)playerX;
            int mapY = (int)playerY;
            int stepX, stepY;

            if (playerLookXRad > 0)
            {
                stepX = 1;
                xRayDist = Math.Abs((mapX + 1 - playerX)) * xDeltaDist;
            }
            else
            {
                stepX = -1;
                xRayDist = Math.Abs((playerX - mapX)) * xDeltaDist;
            }

            if (playerLookYRad > 0)
            {
                stepY = 1;
                yRayDist = Math.Abs((mapY + 1 - playerY)) * yDeltaDist;
            }
            else
            {
                stepY = -1;
                yRayDist = Math.Abs((playerY - mapY)) * yDeltaDist;
            }

            if (playerLookXRad == 0)
                xRayDist = double.PositiveInfinity;

            if (playerLookYRad == 0)
                yRayDist = double.PositiveInfinity;

            double rayDistance = 0;
            bool hit = false;
            face = 0;

            while (!hit)
            {
                if (xRayDist < yRayDist)
                {
                    xRayDist += xDeltaDist;
                    rayDistance = xRayDist;
                    mapX += stepX;

                    face = 0;
                }
                else
                {
                    yRayDist += yDeltaDist;
                    rayDistance = yRayDist;
                    mapY += stepY;

                    face = 1;
                }

                try
                {
                    hit = map[mapY, mapX] == 1;

                }
                catch (Exception)
                {
                    Debug.WriteLine(mapY + ", " + mapX);
                    throw;
                }
            }
            if (face == 0) rayDistance -= xDeltaDist;
            else rayDistance -= yDeltaDist;

            return rayDistance;
        }
    }
}
