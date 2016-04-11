using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using Prototype.TileEngine.Collisions;

namespace Prototype.Characters.CharacterIO
{
    public class ModelReader
    {
        #region Fields
            /// <summary>
            /// Character's health points
            /// </summary>
            public int hp;
            /// <summary>
            /// Character's vision radio
            /// </summary>
            public float visionRadio;
            /// <summary>
            /// Character's damage per second
            /// </summary>
            public float dps;
            /// <summary>
            /// Character's movement speed
            /// </summary>
            public float speed;
            /// <summary>
            /// Character's life regeneration
            /// </summary>
            public float regeneration;
            /// <summary>
            /// Radio in which ranged enemies shoot
            /// </summary>
            public float shotRadio;
            /// <summary>
            /// Radio in which ranged enemies increase their distance with the player
            /// </summary>
            public float escapeRadio;
            /// <summary>
            /// Character's spriteSheet
            /// </summary>
            public String spriteSheet;
            /// <summary>
            /// Number of sprites in a row
            /// </summary>
            public int nSpritesRow;
            /// <summary>
            /// Animations
            /// </summary>
            public List<AnimationReader> animations;
            /// <summary>
            /// Frames in which the animation can be stopped
            /// </summary>
            public List<int> interrumptibleAttackFrames;
            /// <summary>
            /// Frames in which the dash animation can be stopped
            /// </summary>
            public List<int> interrumptibleDashFrames;
            /// <summary>
            /// Character's collision
            /// </summary>
            public CharacterCollisionAdapter collisions;
            /// <summary>
            /// Character's offset for drawing
            /// </summary>
            public Vector2 drawOffset;
            /// <summary>
            /// Character's attack collider
            /// </summary>
            public Collidable attackCollider;
            
        #endregion

        #region Methods
            /// <summary>
            /// Reads a character model from an XML file
            /// </summary>
            /// <param name="filename">Name of the file containing the model</param>
            /// <returns>The model</returns>
            public static CharModel readFromXML(String filename)
            {
                ModelReader mr;
                XmlSerializer serializer = new XmlSerializer(typeof(ModelReader));
                TextReader textReader = new StreamReader(filename);

                object aux = serializer.Deserialize(textReader);
                textReader.Close();
                mr = (ModelReader)aux;

                CharModel cm = new CharModel();
            
                // Load the colliders into the dictionary
           
                for(int i = 0; i < mr.collisions.animationCollision.Count; ++i)
                {
                    CharacterLevelCollisionAdapter clca = mr.collisions.animationCollision[i];
                    int intMin = Math.Min(clca.level.Count, clca.collidables.Count);
                    Dictionary<float, Collidable> col = new Dictionary<float,Collidable>();
                
                    for(int j = 0; j < intMin; ++j)
                    {
                        col.Add(clca.level[j], clca.collidables[j]);
                    }

                    cm.collidables.Add(clca.animation, col);
                }
                cm.hp = mr.hp;
                cm.drawOffset = mr.drawOffset;
                cm.spriteSheet = mr.spriteSheet;
                cm.attackCollider = mr.attackCollider;
                cm.visionRadio = mr.visionRadio;
                cm.dps = mr.dps;
                cm.speed = mr.speed;
                cm.regeneration = mr.regeneration;
                cm.shotRadio = mr.shotRadio;
                cm.escapeRadio = mr.escapeRadio;
                cm.animations = mr.animations;
                cm.interrumptibleAttackFrames = mr.interrumptibleAttackFrames;
                cm.interrumptibleDashFrames = mr.interrumptibleDashFrames;
                cm.nFramesRow = mr.nSpritesRow;
                return cm;
            }

            public static void writeToXML(String filename)
            {
                ModelReader mr = new ModelReader();
                mr.hp = 100;
                mr.spriteSheet = "Sprites/Characters/playerSpriteSheet";
                mr.collisions = new CharacterCollisionAdapter();
                mr.drawOffset = new Vector2(64, 108);
                XmlSerializer serializer = new XmlSerializer(typeof(ModelReader));
                TextWriter textWriter = new StreamWriter(filename);
                serializer.Serialize(textWriter, mr);
                textWriter.Close();
                }
            }
        #endregion
}
