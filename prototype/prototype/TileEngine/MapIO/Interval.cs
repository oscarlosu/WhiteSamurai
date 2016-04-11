using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Utils;
using Prototype.TileEngine.Map;

namespace Prototype.TileEngine.MapIO
{
    [Serializable()]
    public class Interval
    {
        public Pair<int, int> start;
        public Pair<int, int> end;
        public List<Tile> tiles;
        public List<TerrainObject> objects;

        public Interval()
        {
            tiles = new List<Tile>();
            objects = new List<TerrainObject>();
        }
    }
}
