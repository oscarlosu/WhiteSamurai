using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using Microsoft.Xna.Framework;

namespace Prototype.Characters.CharacterIO
{
    public class CharModel
    {
        public int hp;
        public float visionRadio;
        public float dps;
        public float speed;
        public float regeneration;
        public String spriteSheet;
        public Dictionary<String, Dictionary<float, Collidable>> collidables;
        public Vector2 drawOffset;
        public Collidable attackCollider;
        public float shotRadio;
        public float escapeRadio;
        public int nFramesRow;
        public List<AnimationReader> animations;
        public List<int> interrumptibleAttackFrames;
        public List<int> interrumptibleDashFrames;
        public CharModel()
        {
            collidables = new Dictionary<string, Dictionary<float, Collidable>>();
        }
    }
}
