using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Utils;

namespace Prototype.Characters.Strategies
{
    public static class StrategyConstants
    {
        public const int NUMDIR = 8;
        public const int NORTH = 0;
        public const int NORTHEAST = 1;
        public const int EAST = 2;
        public const int SOUTHEAST = 3;
        public const int SOUTH = 4;
        public const int SOUTHWEST = 5;
        public const int WEST = 6;
        public const int NORTHWEST = 7;
        public const int MAXCLOSEDLIST = 100;
        public const int INITVAL = 16;
        public static List<Pair<int, int>> directions;
        public static void Initialize()
        {
            directions = new List<Pair<int, int>>(NUMDIR);
            for (int i = 0; i < NUMDIR; ++i)
            {
                directions.Add(new Pair<int, int>(0, 0));
            }
            directions[NORTH] = new Pair<int, int>(-1, -1);
            directions[NORTHEAST] = new Pair<int, int>(-1, 0);
            directions[EAST] = new Pair<int, int>(-1, 1);
            directions[SOUTHEAST] = new Pair<int, int>(0, 1);
            directions[SOUTH] = new Pair<int, int>(1, 1);
            directions[SOUTHWEST] = new Pair<int, int>(1, 0);
            directions[WEST] = new Pair<int, int>(1, -1);
            directions[NORTHWEST] = new Pair<int, int>(0, -1);
        }
    }
}
