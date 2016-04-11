using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Utils;
using Prototype.TileEngine.Map;

namespace Prototype.TileEngine.MapIO
{
    [Serializable()]
    public class CellList
    {
        public List<Pair<int, int>> list;
        public List<Tile> tiles;
        public List<TerrainObject> objects;

        public CellList()
        {
            list = new List<Pair<int, int>>();
            tiles = new List<Tile>();
            objects = new List<TerrainObject>();
        }
    }
}
