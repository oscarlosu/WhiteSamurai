using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using System.Xml.Serialization;
using System.IO;

namespace Prototype.Characters
{
    public class CharacterCollisionAdapter
    {
        
        public List<CharacterLevelCollisionAdapter> animationCollision;

        public CharacterCollisionAdapter()
        {
      /*      animationCollision = new List<CharacterLevelCollisionAdapter>();
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleSouth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleNorth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleSouthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleSouthEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleNorthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("idleNorthEast"));
            // Define moving animations
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingSouth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingNorth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingSouthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingSouthEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingNorthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("movingNorthEast"));
            // Define attack animations
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingSouth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingNorth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingSouthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingSouthEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingNorthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("attackingNorthEast"));
            // Define dash animations
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingSouth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingNorth"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingSouthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingSouthEast"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingNorthWest"));
            animationCollision.Add(new CharacterLevelCollisionAdapter("dashingNorthEast"));*/
        }
        public static void writeToXML(String file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CharacterCollisionAdapter));
            TextWriter textWriter = new StreamWriter(file);
            CharacterCollisionAdapter cca = new CharacterCollisionAdapter();
            serializer.Serialize(textWriter, cca);
            textWriter.Close();
            return;
        }
    }
}
