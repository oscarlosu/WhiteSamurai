using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Prototype.TileEngine.IsometricView;

namespace Prototype.TileEngine.Collisions
{
    [Serializable()]
    public class Collidable
    {

        #region Fields and Properties
            public List<Circle> RelativeCircles { get; set; }

            public List<Vector2> RelativePoints { get; set; }

            public bool IsPointCollidable { get; set; }
        #endregion

        #region Constructors
            public Collidable(List<Circle> relativeCircles)
            {
                this.RelativeCircles = relativeCircles;
                IsPointCollidable = false;
            }

            public Collidable(List<Vector2> relativePoints)
            {
                this.RelativePoints = relativePoints;
                IsPointCollidable = true;
            }

            public Collidable()
            {
                this.RelativeCircles = null;
                this.RelativePoints = null;
                this.IsPointCollidable = false;
            }
        #endregion

        #region Methods
            public bool collide(Vector2 position, Collidable other, Vector2 otherPosition)
            {
                if (IsPointCollidable && other.IsPointCollidable)
                {
                    foreach (Vector2 relativePoint in this.RelativePoints)
                    {
                        foreach (Vector2 otherRelativePoint in other.RelativePoints)
                        {
                            // Create point and other's point with position and other's position
                            Vector2 realPoint = position + relativePoint;
                            Vector2 otherRealPoint = otherPosition + otherRelativePoint;
                            if (realPoint == otherRealPoint) return true;
                        }
                    }
                }
                else if(!IsPointCollidable && other.IsPointCollidable)
                {
                    foreach (Circle relativeCircle in RelativeCircles)
                    {
                        foreach (Vector2 otherRelativePoint in other.RelativePoints)
                        {
                            // Create circle and other's point with position and other's position
                            Circle realCircle = new Circle(position + relativeCircle.Center, relativeCircle.Radius);
                            Vector2 otherRealPoint = otherPosition + otherRelativePoint;
                            if (realCircle.intersect(otherRealPoint)) return true;
                        }
                    } 
                }
                else if(IsPointCollidable && !other.IsPointCollidable)
                {
                    foreach (Vector2 relativePoint in this.RelativePoints)
                    {
                        foreach (Circle otherRelativeCircle in other.RelativeCircles)
                        {
                            // Create point and other's circle with other's position and position
                            Vector2 realPoint = position + relativePoint;
                            Circle otherRealCircle = new Circle(otherPosition + otherRelativeCircle.Center, otherRelativeCircle.Radius);
                            if (otherRealCircle.intersect(realPoint)) return true;
                        }              
                    }  
                }
                // Both are circleCollidable
                else
                {
                    foreach (Circle relativeCircle in this.RelativeCircles)
                    {
                        foreach (Circle otherRelativeCircle in other.RelativeCircles)
                        {
                            // Create circles with real center using position
                            Circle realCircle = new Circle(position + relativeCircle.Center, relativeCircle.Radius);
                            Circle otherRealCircle = new Circle(otherPosition + otherRelativeCircle.Center, otherRelativeCircle.Radius);
                            if (realCircle.intersect(otherRealCircle)) return true;
                        }
                    }
                }            
                return false;
            }
        
            /**
             * @brief Checks if there is a collision with a MapCell (parallelogram) 
             * 
             * 
             * 
             */
            public bool collide(Vector2 position, Vector2 cellCoords)
            {
                // PointCollidable
                if (IsPointCollidable)
                {
                    foreach (Vector2 relativePoint in this.RelativePoints)
                    {
                        // Create circle with real center using position
                        Vector2 realPoint = position + relativePoint;
                        // Calculate parallelogram vertices
                        Vector2 v1 = cellCoords;
                        Vector2 v2 = v1 + new Vector2(IsoInfo.TileWidth / 2, -IsoInfo.TileWidth / 4);
                        Vector2 v3 = v1 + new Vector2(0, -IsoInfo.TileWidth / 2);
                        Vector2 v4 = v1 + new Vector2(-IsoInfo.TileWidth / 2, -IsoInfo.TileWidth / 4);
                        // Check if point is inside the parallelogram
                        // Calculate the two parallelogram edges that intersect on v1 (could be any other vertex of the parallelogram)
                        Vector2 e1 = v2 - v1;
                        Vector2 e2 = v4 - v1;
                        // Calculate vector from v1 to the point
                        Vector2 v1c = realPoint - v1;
                        // If the point is inside the parallelogram, it must be a linear combination of e1 and e2
                        // with positive (scalar) c1, c2 (v1c = c1 * e1 + c2 * e2)
                        float determinante1e2 = e1.X * e2.Y - e2.X * e1.Y;
                        float determinantv1ce1 = v1c.X * e1.Y - e1.X * v1c.Y;
                        float determinantv1ce2 = v1c.X * e2.Y - e2.X * v1c.Y;
                        float c2 = -(determinantv1ce1 / determinante1e2);
                        float c1 = (determinantv1ce2 / determinante1e2);
                        if ((0 <= c1 && c1 <= 1) && (0 <= c2 && c2 <= 1))
                            return true;
                    }
                }
                // CircleCollidable
                else
                {
                    foreach (Circle relativeCircle in this.RelativeCircles)
                    {
                        // Create circle with real center using position
                        Circle realCircle = new Circle(position + relativeCircle.Center, relativeCircle.Radius);
                        // Calculate parallelogram vertices
                        Vector2 v1 = cellCoords;
                        Vector2 v2 = v1 + new Vector2(IsoInfo.TileWidth / 2, -IsoInfo.TileWidth / 4);
                        Vector2 v3 = v1 + new Vector2(0, -IsoInfo.TileWidth / 2);
                        Vector2 v4 = v1 + new Vector2(-IsoInfo.TileWidth / 2, -IsoInfo.TileWidth / 4);
                        // Check if any of the vertices are inside the circle
                        if (realCircle.intersect(v1) || realCircle.intersect(v2)
                            || realCircle.intersect(v3) || realCircle.intersect(v4))
                            return true;
                        // Check if any of the edges intersects the circle
                        if (realCircle.intersect(v1, v2) || realCircle.intersect(v2, v3) ||
                            realCircle.intersect(v3, v4) || realCircle.intersect(v4, v1))
                            return true;
                        // Check if circle center is inside the parallelogram
                        // Calculate the two parallelogram edges that intersect on v1 (could be any other vertex of the parallelogram)
                        Vector2 e1 = v2 - v1;
                        Vector2 e2 = v4 - v1;
                        // Calculate vector from v1 to the circle center
                        Vector2 v1c = realCircle.Center - v1;
                        // If the center is inside the parallelogram, it must be a linear combination of e1 and e2
                        // with positive (scalar) c1, c2 (v1c = c1 * e1 + c2 * e2)
                        float determinante1e2 = e1.X * e2.Y - e2.X * e1.Y;
                        float determinantv1ce1 = v1c.X * e1.Y - e1.X * v1c.Y;
                        float determinantv1ce2 = v1c.X * e2.Y - e2.X * v1c.Y;
                        float c1 = -(determinantv1ce1 / determinante1e2);
                        float c2 = (determinantv1ce2 / determinante1e2);
                        if ((0 <= c1 && c1 <= 1) && (0 <= c2 && c2 <= 1))
                            return true;
                    }
                }
                return false;
            }
        #endregion

        /*
         /**
         * @brief Returns a list of some of points where the collidable intersected the cell
         * 
         * 
         * 
         * 
         * /
        public List<Vector3> getCollisionPoints(Vector2 position, Vector2 cellCoords)
        {
            List<Vector3> collisionPoints = new List<Vector3>();

            // TODO: 

            return collisionPoints;
        }
         */
        
    }
}
