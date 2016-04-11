using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Prototype.TileEngine.Collisions
{
    [Serializable()]
    public class Circle
    {
        
        public Vector2 Center { get; set; }

        
        public float Radius { get; set; }

        public Circle(Vector2 center, float radius)
        {
            this.Center = center;
            this.Radius = radius;
        }

        public Circle()
        {
            this.Center = Vector2.Zero;
            this.Radius = 1.0f;
        }

        public bool intersect(Vector2 point)
        {
            float distance = (this.Center - point).Length();
            if (distance <= this.Radius)
                return true;
            else
                return false;
        }

        public bool intersect(Circle circle)
        {
            float distance = (this.Center - circle.Center).Length();
            if (distance <= this.Radius + circle.Radius)
                return true;
            else
                return false;
        }
        /**
         * @brief Checks if the segment determined by v1 and v2
         * intersects the circle
         * 
         * @param v1 First segment vertex
         * @param v2 Second segment vertex
         * 
         * @return true if segment intersects circle
         */
        public bool intersect(Vector2 v1, Vector2 v2)
        {
            // Preliminary check
            // If segment vertices are inside the circle skip the rest
            if (intersect(v1) || intersect(v2)) return true;
            // Calculate vector from v1 to v2
            Vector2 v1v2 = v2 - v1;
            // Calculate distance from center to line containing segment
            float distance = Math.Abs((v2.X - v1.X) * (v1.Y - Center.Y) - (v1.X - Center.X) * (v2.Y - v1.Y)) / v1v2.Length();
            
            // Check if distance is smaller than radius
            if(distance <= Radius)
            {
                // Verify intersection point is within segment (as opposed to only within the infinite line containing the segment)
                // Calculate orthogonal vector to segment
                Vector2 orthogonal =
                    new Vector2(v2.Y - v1.Y, -(v2.X - v1.X));
                orthogonal.Normalize();
                Vector2 p = Center + orthogonal * distance;
                if(((v1.X <= p.X && p.X <= v2.X) || (v2.X <= p.X && p.X <= v1.X)) && // v1.X <= center.X <= v2.X or viceversa
                    ((v1.Y <= p.Y && p.Y <= v2.Y) || (v2.Y <= p.Y && p.Y <= v1.Y))) // v1.Y <= center.Y <= v2.Y or viceversa
                    return true;
            }
            return false;

        }

    }
}
