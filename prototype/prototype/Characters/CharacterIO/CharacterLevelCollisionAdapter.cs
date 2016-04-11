using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.TileEngine.Collisions;
using Microsoft.Xna.Framework;

namespace Prototype.Characters
{
    public class CharacterLevelCollisionAdapter
    {
        public String animation;
        public List<float> level;
        public List<Collidable> collidables;

        public CharacterLevelCollisionAdapter()
        {
            level = new List<float>();
            collidables = new List<Collidable>();

           /* List<Circle> level3Circles = new List<Circle>();
            level3Circles.Add(new Circle(new Vector2(0, 0), 10));
            level.Add(3);
            collidables.Add(new Collidable(level3Circles));

            List<Vector2> level0Points = new List<Vector2>();
            level0Points.Add(new Vector2(0, 0));
            level.Add(0);
            collidables.Add(new Collidable(level0Points));*/
        }

        public CharacterLevelCollisionAdapter(String animation)
            : this()
        {
            this.animation = animation;
        }
    }
}
