using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame
{
    public static class Directions
    {
        public static Coordinate North { get { return new Coordinate(0, -1); } }
        public static Coordinate South { get { return new Coordinate(0, 1); } }
        public static Coordinate East { get { return new Coordinate(1, 0); } }
        public static Coordinate West { get { return new Coordinate(-1, 0); } }
        public static List<Coordinate> AllDirections { get { return new List<Coordinate>() { North, South, East, West }; } }

        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
