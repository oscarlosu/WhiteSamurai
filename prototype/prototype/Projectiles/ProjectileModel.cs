using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using System.Xml.Serialization;
using System.IO;

namespace Prototype.Projectiles
{
    public class ProjectileModel
    {
        public float speed;
        public float damage;

        public static ProjectileModel readFromXML(String file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProjectileModel));
            TextReader reader = new StreamReader(file);
            object obj = serializer.Deserialize(reader);
            ProjectileModel el = (ProjectileModel)obj;
            reader.Close();
            return el;
        }
    }
}
