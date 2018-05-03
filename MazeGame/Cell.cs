using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public struct Cell
    {
        public Coordinate PriorCell { get; set; }
        public bool visited { get; set; }
        public bool North { get; set; }
        public bool South { get; set; }
        public bool East { get; set; }
        public bool West { get; set; }
    }
}
