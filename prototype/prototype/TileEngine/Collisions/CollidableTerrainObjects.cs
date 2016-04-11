using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using Prototype.TileEngine.Map;

namespace Prototype.TileEngine.Collisions
{
    public class CollidableTerrainObjects
    {
        public Dictionary<TerrainObject, Collidable> collidables;

        public static CollidableTerrainObjects loadFromXML(string p)
        {
            return CollidableTerrainObjectsAdapter.loadFromXML(p);
        }

        public CollidableTerrainObjects()
        {
            collidables = new Dictionary<TerrainObject, Collidable>();
        }
    }
}
