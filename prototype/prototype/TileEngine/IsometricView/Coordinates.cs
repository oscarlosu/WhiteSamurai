/**
 * @file TileEngine/Coordinates.cs
 * @author Oscar Losada & Javier Sanz-Cruzado
 * @brief Class definition for Coordinates
 * @date 28/09/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Prototype.TileEngine.IsometricView
{

    /// <summary>
    /// Static class which provides methods for changing between several coordinate systems.
    /// </summary>
    static class Coordinates
    {

        /// <summary>
        /// Converts isometric coordinates to screen Coordinates.
        /// </summary>
        /// <param name="isoCoordinates">Isometric coordinates</param>
        /// <returns>Screen Coordinates. The result coordinates are integers</returns>
        public static Vector2 toScreen(Vector3 isoCoordinates)
        {
            float x = (isoCoordinates.Y - isoCoordinates.X) * IsoInfo.Cosine;
            float y = (isoCoordinates.Y + isoCoordinates.X) * IsoInfo.Sine - isoCoordinates.Z;

            return new Vector2(x,y);
        }

        /// <summary>
        /// Converts cellCoordinates to screen coordinates
        /// </summary>
        /// <param name="cellCoordinates">Cell coordinates</param>
        /// <returns>Screen Coordinates. The result coordinates are integers</returns>
        public static Vector2 toScreen(Vector2 cellCoordinates)
        {
            return toScreen(new Vector3(cellCoordinates.X * IsoInfo.TileSide, cellCoordinates.Y * IsoInfo.TileSide, 0));
        }
        
        /// <summary>
        /// Converts 2D isometric coordinates (without elevation) to standard 2D coordinates
        /// </summary>
        /// <param name="isoCoordinates">2D isometric coordinates (without elevation)</param>
        /// <returns>standard 2D coordinates</returns>
        public static Vector2 to2D(Vector2 isoCoordinates)
        {
            float x = (isoCoordinates.Y - isoCoordinates.X) * IsoInfo.Cosine;
            float y = - (isoCoordinates.Y + isoCoordinates.X) * IsoInfo.Sine;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Converts standard 2D coordinates to isometric coordinates assuming there is no elevation
        /// </summary>
        /// <param name="coordinates2D">Standard 2 dimensional coordinates</param>
        /// <returns>isometric coordinates with elevation 0</returns>
        public static Vector3 toIsoZ0(Vector2 coordinates2D)
        {
            Vector2 isoXAxis = new Vector2(IsoInfo.SouthWest.X, IsoInfo.SouthWest.Y);
            Vector2 isoYAxis = new Vector2(IsoInfo.SouthEast.X, IsoInfo.SouthEast.Y);
            // Calculate scalar projections on isoXAxis and isoYAxis
            float isoX = (Vector2.Dot(coordinates2D, isoXAxis) / coordinates2D.Length());
            float isoY = (Vector2.Dot(coordinates2D, isoYAxis) / coordinates2D.Length());

            return new Vector3(isoX, isoY, 0);
        }

        /// <summary>
        /// Converts isometric coordinates to cell coordinates
        /// </summary>
        /// <param name="isoCoordinates">Isometric coordinates</param>
        /// <returns>Cell coordinates. The result coordinates are integers</returns>
        public static Vector2 toCell(Vector3 isoCoordinates)
        {
            int x = (int)Math.Floor(isoCoordinates.X / IsoInfo.TileSide);
            int y = (int)Math.Floor(isoCoordinates.Y / IsoInfo.TileSide);

            return new Vector2(x, y);

        }
 
        /// <summary>
        /// Converts isometric coordinates to isometric coordinates within a cell,
        /// ie relative to the cell ("offset" within cell)
        /// </summary>
        /// <param name="isoCoordinates">Isometric coordinates</param>
        /// <returns>Isometric coordinates relative to the cell ("offset" within cell)</returns>
        public static Vector3 toInsideCell(Vector3 isoCoordinates)
        {
            // Get CellCoords
            Vector2 cellCoords = toCell(isoCoordinates);
            cellCoords.X = cellCoords.X * IsoInfo.TileSide;
            cellCoords.Y = cellCoords.Y * IsoInfo.TileSide;
            // Calculate offset inside cell
            int x = (int)round(isoCoordinates.X - cellCoords.X);
            int y = (int)round(isoCoordinates.Y - cellCoords.Y);

            return new Vector3(x, y, 0);

        }

        /// <summary>
        /// Rounds a double to integer
        /// </summary>
        /// <param name="x">Double to be rounded</param>
        /// <returns>the result of rounding x</returns>
        public static int round(double x)
        {
            return (int)Math.Truncate(x + 0.5);
        }

    }
}
