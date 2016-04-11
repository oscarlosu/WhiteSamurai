using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using Prototype.TileEngine.Collisions;
using Prototype.TileEngine.Map;

namespace Prototype.TileEngine.Collisions
{
    [Serializable()]
    public class CollidableTerrainObjectsAdapter
    {
        public List<TerrainObject> objects;
        public List<Collidable> collidables;

        public static CollidableTerrainObjects loadFromXML(String xmlName)
        {
            //Read from XML file
            XmlSerializer serializer = new XmlSerializer(typeof(CollidableTerrainObjectsAdapter));
            TextReader reader = new StreamReader(xmlName);
            

            CollidableTerrainObjectsAdapter adapter = new CollidableTerrainObjectsAdapter();
            CollidableTerrainObjects collidable = new CollidableTerrainObjects();
            object obj = serializer.Deserialize(reader);
            adapter = (CollidableTerrainObjectsAdapter)obj;
            reader.Close();
            
            
            int min = Math.Min(adapter.objects.Count, adapter.collidables.Count);
            for (int i = 0; i < min; ++i)
            {
                collidable.collidables.Add(adapter.objects.ElementAt(i), adapter.collidables.ElementAt(i));
            }
            
            return collidable;
        }
    }
}
