using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Prototype.TileEngine.IsometricView;



namespace Prototype.TileEngine.Map
{
    static class Slope
    {
        // Reference elevated vertices' coordinates
        static Vector3 ELEVATED_NORTH = new Vector3(0, 0, IsoInfo.TileElevation);
        static Vector3 ELEVATED_EAST = new Vector3(0, IsoInfo.TileSide, IsoInfo.TileElevation);
        static Vector3 ELEVATED_SOUTH = new Vector3(IsoInfo.TileSide, IsoInfo.TileSide, IsoInfo.TileElevation);
        static Vector3 ELEVATED_WEST = new Vector3(IsoInfo.TileSide, 0, IsoInfo.TileElevation);

        static Vector3 LOW_NORTH = new Vector3(0, 0, 0);
        static Vector3 LOW_EAST = new Vector3(0, IsoInfo.TileSide, 0);
        static Vector3 LOW_SOUTH = new Vector3(IsoInfo.TileSide, IsoInfo.TileSide, 0);
        static Vector3 LOW_WEST = new Vector3(IsoInfo.TileSide, 0, 0);

        /// <summary>
        /// Calculates z component (elevation) correponding to the point (x,y) in the plane containing the slope
        /// Plane equations:
        /// x = slopePoint3.x + c1*v1.X + c1*v2.X
        /// y = slopePoint3.Y + c1*v1.Y + c1*v2.Y
        /// z = slopePoint3.Z + c1*v1.Z + c1*v2.Z
        /// </summary>
        /// <param name="point">Point in the horizontal plane for which we want to find the elevation on the slope</param>
        /// <param name="slopePoint1">A point within the slope</param>
        /// <param name="slopePoint2">A point within the slope</param>
        /// <param name="slopePoint3">A point within the slope</param>
        /// <returns>Elevation corresponding to point's commponents on the slope</returns>
        private static float calculateElevation(Vector2 point, Vector3 slopePoint1, Vector3 slopePoint2, Vector3 slopePoint3)
        {
            if (slopePoint1 == slopePoint2 || slopePoint1 == slopePoint3 || slopePoint2 == slopePoint3)
                return -1;
            // Calculate 2 vectors within the slope's plane
            Vector3 v1 = slopePoint1 - slopePoint3;
            Vector3 v2 = slopePoint2 - slopePoint3;
            // Use plane equations to calculate z component (elevation) correponding to the point (x,y) in that plane
            // Isolating c1 and c2 int the plane equations for x and y
            float c1, c2;
            if (v1.X != 0)
            {
                c2 = (point.Y - slopePoint3.Y - v1.Y * ((point.X - slopePoint3.X) / v1.X)) / (v2.Y - ((v1.Y * v2.X) / v1.X));
                c1 = (point.X - slopePoint3.X - c2 * v2.X) / (v1.X);
            }
            else
            {
                c1 = (point.X - slopePoint3.X - v1.X * ((point.Y - slopePoint3.Y) / v1.Y)) / (v2.X - ((v1.X * v2.Y) / v1.Y));
                c2 = (point.Y - slopePoint3.Y - c1 * v2.Y) / (v1.Y);            
            }
            
            // Calculate z
            float z = slopePoint3.Z + c1 * v1.Z + c2 * v2.Z;

            return z;
        }

        /// <summary>
        /// Returns the elevation (z) corresponding to the point (x,y) on the slopeTile
        /// </summary>
        /// <param name="slopeType">The slope tile type for which the elevation will be calculated</param>
        /// <param name="point">point the point's coordinates, relative to the tile's origin (upper top vertex)</param>
        /// <returns>Elevation corresponding to the given point on the given slope</returns>
        public static float slopeElevation(Tile slopeType, Vector2 point)
        {
            switch (slopeType)
            {
                // 1 elevated vertex
                case Tile.slopeHighSVertex:
                    // Slopeless part
                    if (point.Y <= IsoInfo.TileSide - point.X)
                        return 0;
                    else
                        return calculateElevation(point, LOW_WEST, LOW_EAST, ELEVATED_SOUTH);
                case Tile.slopeHighWVertex:
                    // Slopeless part
                    if (point.Y >= point.X)
                        return 0;
                    else
                        return calculateElevation(point, LOW_NORTH, LOW_SOUTH, ELEVATED_WEST);
                case Tile.slopeHighNVertex:
                    // Slopeless part
                    if (point.Y >= IsoInfo.TileSide - point.X)
                        return 0;
                    else
                        return calculateElevation(point, LOW_WEST, LOW_EAST, ELEVATED_NORTH);
                case Tile.slopeHighEVertex:
                    // Slopeless part
                    if (point.Y <= point.X)
                        return 0;
                    else
                        return calculateElevation(point, LOW_NORTH, LOW_SOUTH, ELEVATED_EAST);
                // 2 elevated vertices
                case Tile.slopeLowSWEdge:
                    return calculateElevation(point, ELEVATED_EAST, ELEVATED_NORTH, LOW_SOUTH);
                case Tile.slopeLowNWEdge:
                    return calculateElevation(point, ELEVATED_EAST, ELEVATED_SOUTH, LOW_WEST);
                case Tile.slopeLowNEEdge:
                    return calculateElevation(point, ELEVATED_WEST, ELEVATED_SOUTH, LOW_NORTH);
                case Tile.slopeLowSEEdge:
                    return calculateElevation(point, ELEVATED_WEST, ELEVATED_NORTH, LOW_EAST);
                // 3 elevated vertices
                case Tile.slopeLowSVertex:
                    if (point.Y <= IsoInfo.TileSide - point.X)
                        return IsoInfo.TileElevation;
                    else
                        return calculateElevation(point, ELEVATED_WEST, ELEVATED_EAST, LOW_SOUTH);
                case Tile.slopeLowWVertex:
                    if (point.Y >= point.X)
                        return IsoInfo.TileElevation;
                    else
                        return calculateElevation(point, ELEVATED_NORTH, ELEVATED_SOUTH, LOW_WEST);
                case Tile.slopeLowNVertex:
                    if (point.Y >= IsoInfo.TileSide - point.X)
                        return IsoInfo.TileElevation;
                    else
                        return calculateElevation(point, ELEVATED_WEST, ELEVATED_EAST, LOW_NORTH);
                case Tile.slopeLowEVertex:
                    if (point.Y <= point.X)
                        return IsoInfo.TileElevation;
                    else
                        return calculateElevation(point, ELEVATED_NORTH, ELEVATED_SOUTH, LOW_EAST);
                default:
                    return -1;

            }
        }
    }
}
