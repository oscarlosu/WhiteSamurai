/**
 * @file TileEngine/Camera.cs
 * @author Oscar Losada & Javier Sanz-Cruzado
 * @brief Class definition for Camera
 * @date 28/09/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace Prototype.TileEngine.IsometricView
{
    enum DrawableType
    {
        TILE = 0,
        OBJECT,
        MOBILE_OBJECT
    };

    static class Camera
    {
        #region Fields
        /// <summary>
        /// Width of the screen
        /// </summary>
        private static int screenWidth;
        /// <summary>
        /// Height of the screen
        /// </summary>
        private static int screenHeight;
        /// <summary>
        /// Position of the camera
        /// </summary>
        private static Vector2 position;


        private static Vector3 firstCell;
        private static Vector3 lastCell;
        private static int mapSize;
        private static int maxTileStack;
        private static float tileDepth;
        #endregion

        #region Properties
        /// <summary>
        /// Width of the screen
        /// </summary>
        public static int ScreenWidth
        {
            get
            {
                return screenWidth;
            }
            set
            {
                screenWidth = value;
            }
        }

        /// <summary>
        /// Height of the screen
        /// </summary>
        public static int ScreenHeight
        {
            get
            {
                return screenHeight;
            }
            set
            {
                screenHeight = value;
            }
        }

        /// <summary>
        /// Position of the camera
        /// </summary>
        public static Vector2 Position
        {
            get
            {
                return position;
            }

        }

        public static int MapSize
        {
            get
            {
                return mapSize;
            }
            set
            {
                mapSize = value;
            }
        }
        #endregion

        #region Game Methods
                
        /// <summary>
        /// Stores the game window dimensions
        /// </summary>
        /// <param name="window">Current game window</param>
        public static void Initialize(GameWindow window)
        {
            screenWidth =  window.ClientBounds.Width;
            screenHeight = window.ClientBounds.Height;
            position = Vector2.Zero;
        }
                
        /// <summary>
        /// Calculates camera isometric coords with center position.
        /// Usually, the center will be the character's isometric coordinates.
        /// </summary>
        /// <param name="center">character position</param>
        public static void Update(Vector3 center)
        {
            Vector2 screenCenter = Coordinates.toScreen(new Vector3(center.X, center.Y, center.Z));
            position = new Vector2(
                screenCenter.X - screenWidth / 2,
                screenCenter.Y - screenHeight / 2);


            // Update depthSorting parameters
            firstCell = MapManager.Instance.FirstCell;
            lastCell = MapManager.Instance.LastCell;
            mapSize = (int)(lastCell.Y - firstCell.Y) + (int)(lastCell.X - firstCell.X);
            maxTileStack = MapManager.Instance.properties.maxTileStack;
            tileDepth = IsoInfo.TileSide + IsoInfo.TileSide;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the depth of an object in the isometric map
        /// </summary>
        /// <param name="worldPosition">Position of the object</param>
        /// <param name="type">Type of depth</param>
        /// <param name="special">Special depth precedence of the object</param>
        /// <returns>The depth</returns> 
        public static float getDepth(Vector3 worldPosition, DrawableType type)
        {
            // FrontToBack -> draw values closer to 0 behind values closer to 1            
            double insideCellRow;
            int row;

            int elevation;
            
            if (type == DrawableType.TILE)
            {
                // Make coordinates relative to first drawn cell intead of world origin
                worldPosition -= firstCell;
                row = (int)(worldPosition.X + worldPosition.Y);
                elevation = (int)worldPosition.Z;
                insideCellRow = 0;
            }
            else if (type == DrawableType.OBJECT)
            {
                // Make coordinates relative to first drawn cell intead of world origin
                worldPosition -= firstCell;
                row = (int)(worldPosition.X + worldPosition.Y);
                elevation = (int)worldPosition.Z;
                // Objects are always centered in their cells
                insideCellRow = IsoInfo.TileCenter.X + IsoInfo.TileCenter.Y;
            }
            else // MOBILE_OBJECT
            {
                // Make coordinates relative to first drawn cell intead of world origin
                worldPosition -= (IsoInfo.TileSide * firstCell);
                row = (int)Math.Floor((worldPosition.X / IsoInfo.TileSide) + (int)Math.Floor(worldPosition.Y / IsoInfo.TileSide));
                elevation = (int)Math.Ceiling((worldPosition.Z / IsoInfo.TileElevation));

                //Vector3 insideCellCoords = Coordinates.toInsideCell(worldPosition) + new Vector3(0, 0, worldPosition.Z);
                Vector3 insideCellCoords = Coordinates.toInsideCell(worldPosition);
                insideCellRow = insideCellCoords.X + insideCellCoords.Y;
            }

            float depth = 0;

            depth += (float)row / (float)mapSize;
            depth += (float)(insideCellRow / (mapSize * tileDepth));
            depth += (float)elevation / (float)(mapSize * maxTileStack * tileDepth);

            /* Precedence for elevation
            depth += (float)elevation / (float)(mapSize * maxTileStack);
            depth += (float)(insideCellRow / (mapSize * maxTileStack * tileDepth));
            */
            return depth;
        }
        public static float getDepthAbsolute(Vector3 worldPosition, DrawableType type)
        {
            // FrontToBack -> draw values closer to 0 behind values closer to 1
            int mapSize = MapManager.Instance.CurrentMap.NCols + MapManager.Instance.CurrentMap.NRows;
            int maxTileStack = MapManager.Instance.properties.maxTileStack;
            double tileDepth = IsoInfo.TileSide + IsoInfo.TileSide;


            double insideCellRow;
            int row;

            int elevation;

            if (type == DrawableType.TILE)
            {
                row = (int)(worldPosition.X + worldPosition.Y);
                elevation = (int)worldPosition.Z;
                insideCellRow = 0;
            }
            else if (type == DrawableType.OBJECT)
            {
                row = (int)(worldPosition.X + worldPosition.Y);
                elevation = (int)worldPosition.Z;
                // Objects are always centered in their cells
                insideCellRow = IsoInfo.TileCenter.X + IsoInfo.TileCenter.Y;
            }
            else // MOBILE_OBJECT
            {
                row = (int)Math.Floor((worldPosition.X / IsoInfo.TileSide) + (int)Math.Floor(worldPosition.Y / IsoInfo.TileSide));
                elevation = (int)Math.Ceiling((worldPosition.Z / IsoInfo.TileElevation));

                //Vector3 insideCellCoords = Coordinates.toInsideCell(worldPosition) + new Vector3(0, 0, worldPosition.Z);
                Vector3 insideCellCoords = Coordinates.toInsideCell(worldPosition);
                insideCellRow = insideCellCoords.X + insideCellCoords.Y;
            }

            float depth = 0;

            depth += (float)row / (float)mapSize;
            depth += (float)(insideCellRow / (mapSize * tileDepth));
            depth += (float)elevation / (float)(mapSize * maxTileStack * tileDepth);

            /* Precedence for elevation
            depth += (float)elevation / (float)(mapSize * maxTileStack);
            depth += (float)(insideCellRow / (mapSize * maxTileStack * tileDepth));
            */
            return depth;
        }
        #endregion
    }
}
