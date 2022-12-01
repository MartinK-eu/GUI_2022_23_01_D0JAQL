using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RayCaster.Models
{
    public class GameModel : IGameModel
    {
        public int[,] MapMatrix { get; set; }
        public Character Player { get; set; }
        public bool InMapMode { get; set; }


        public GameModel(int[,] map, Character p, bool inMapMode)
        {
            MapMatrix = map;
            Player = p;
            InMapMode = inMapMode;
        }
    }
}
