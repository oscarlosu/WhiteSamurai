using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Prototype.ScreenManager.Screens;
using Prototype.TileEngine.IsometricView;

namespace Prototype.TileEngine.Map
{
    /// <summary>
    /// Class that represents a cell in a TileMap
    /// </summary>
    public class MapCell
    {
        #region TerrainTilesLayer
            #region Fields
            /// <summary>
            /// List of elevated and slope tile
            /// <seealso cref="Tile"/>
            /// </summary>
            private List<Tile> terrain;
            /// <summary>
            /// Number of elements in <see cref="terrain"/>
            /// </summary>
            private int terrainTileCount;
            /// <summary>
            /// Screen position of the terrain tile
            /// </summary>
            private List<Vector2> terrainScreenPosition;
            /// <summary>
            /// Depth of the terrain tile
            /// </summary>
            private List<float> terrainDepth;
            /// <summary>
            /// Color of the terrain tile
            /// </summary>
            private List<Color> terrainColor;
            #endregion

            #region Properties
            /// <summary>
            /// <see cref="terrainTileCount"/>
            /// </summary>
            public int TerrainTileCount { get { return terrainTileCount; } }

            /// <summary>
            /// <see cref="terrain"/>
            /// </summary>
            public List<Tile> Terrain { get { return terrain; } }
            
            /// <summary>
            /// <see cref="terrainColor"/>
            /// </summary>
            public List<Color> TerrainColor { get { return terrainColor; } }
            #endregion

            #region Methods
            /// <summary>
            /// Adds a terrain tile to the list
            /// </summary>
            /// <param name="tileID">Tile ID</param>
            public void addTerrainTile(Tile tileID)
            {
                if (tileID != Tile.empty && terrainTileCount + terrainObjectTileCount < MapManager.Instance.properties.maxTileStack)
                {
                    if (isTerrainEmpty() || isSlope(getTerrainTopTile()) == false)
                    {
                        terrain.Add(tileID);
                        ++terrainTileCount;
                        terrainDepth.Add(0);
                        terrainColor.Add(Color.White);
                        terrainScreenPosition.Add(Vector2.Zero);
                    }  
                }
            }

            /// <summary>
            /// Adds a list of terrain tiles to the list
            /// </summary>
            /// <param name="list">List of terrain tiles</param>
            public void addTerrainTileList(List<Tile> list)
            {
                foreach (Tile tileID in list)
                {
                    addTerrainTile(tileID);
                }
            }

            /// <summary>
            /// Gets a terrain tile from an index
            /// </summary>
            /// <param name="index">Index of the tile</param>
            /// <returns>The tile in that position</returns>
            public Tile getTerrainTile(int index)
            {
                return terrain[index];
            }
            /// <summary>
            /// Gets the top terrain tile
            /// </summary>
            /// <returns></returns>
            public Tile getTerrainTopTile()
            {
                return terrain[terrainTileCount - 1];
            }
            /// <summary>
            /// Checks if the terrain is empty
            /// </summary>
            /// <returns>True if the terrain is empty, false in other case</returns>
            public bool isTerrainEmpty()
            {
                return terrainTileCount == 0;
            }

            /// <summary>
            /// Gets the (flat) elevation of the cell
            /// </summary>
            /// <returns>The elevation</returns>
            public float getTerrainElevation()
            {
                // Check if top tile is slope
                Tile topTile = this.getTerrainTopTile();
                if (this.isSlope(topTile))
                    return (this.terrainTileCount - 1) * IsoInfo.TileElevation;
                else
                    return this.terrainTileCount * IsoInfo.TileElevation;
            }

            /// <summary>
            /// Gets the elevation of a point of the cell
            /// </summary>
            /// <param name="point">Point</param>
            /// <returns>The elevation</returns>
            public float getTerrainElevation(Vector3 point)
            {
                float elevation = this.terrainTileCount * IsoInfo.TileElevation;
            
                // Check if top tile is slope
                Tile topTile = this.getTerrainTopTile();
                if (this.isSlope(topTile))
                {
                    // Uncount slope
                    elevation -= IsoInfo.TileElevation;
                    // Calculate elevation at slope point
                    Vector3 offset = Coordinates.toInsideCell(point);
                    elevation += Slope.slopeElevation(topTile, new Vector2(offset.X, offset.Y));
                }
                return elevation;
            }

            /// <summary>
            /// Checks if a tile is a slope
            /// </summary>
            /// <param name="tileID">Tile ID</param>
            /// <returns>True if it is a slope, false if not</returns>
            public bool isSlope(Tile tileID)
            {
                if ((int)tileID >= 1 && (int)tileID <= 12)
                    return true;
                else
                    return false;
            }
            #endregion
        #endregion

        #region TerrainObjectsLayer
            #region Fields
            /// <summary>
            /// List of trees and rocks
            /// <seealso cref="TerrainObjects"/>
            /// </summary>
            private List<TerrainObject> terrainObjects;
            /// <summary>
            /// Number of elements in <see cref="terrainObjects"/>
            /// </summary>
            private int terrainObjectTileCount;
            private List<Vector2> terrainObjectPosition;
            private List<float> terrainObjectDepth;
            private List<Color> terrainObjectColor;
            #endregion

            #region Properties
            /// <summary>
            /// <see cref="terrainObjects"/>
            /// </summary>
            public List<TerrainObject> TerrainObjects { get { return terrainObjects; } }
            /// <summary>
            /// <see cref="terrainObjectTileCount"/>
            /// </summary>
            public int TerrainObjectTileCount { get { return terrainObjectTileCount; } } /**< @see terrainObjectTileCount */
            public List<Color> TerrainObjectColor { get { return terrainObjectColor; } }
            #endregion

            #region Methods
            
            /// <summary>Adds a terrain object tile to the list</summary>
            /// <param name="objectID">Object ID</param>
            public void addTerrainObjectTile(TerrainObject objectID)
            {
                if (objectID != TerrainObject.empty && terrainTileCount + terrainObjectTileCount < MapManager.Instance.properties.maxTileStack)
                {
                    // Add only if top tile is not a slope
                    if (isTerrainEmpty() || this.isSlope(getTerrainTopTile()) == false)
                    {
                        terrainObjects.Add(objectID);
                        ++terrainObjectTileCount;
                        terrainObjectDepth.Add(0);
                        terrainObjectColor.Add(Color.White);
                        terrainObjectPosition.Add(Vector2.Zero);
                    }
                }

            }

            /// <summary>
            /// Adds a list of terrain object tiles to the list
            /// </summary>
            /// <param name="list">List of terrain object tiles</param>
            public void addTerrainObjectTileList(List<TerrainObject> list)
            {
                foreach (TerrainObject objectID in list)
                {
                    addTerrainObjectTile(objectID);
                }
            }


            /// <summary>
            /// Gets a terrain object tile of the list using an index
            /// </summary>
            /// <param name="index">Index of the tile</param>
            /// <returns>The tile</returns>
            public TerrainObject getTerrainObjectTile(int index)
            {
                return terrainObjects[index];
            }
            /// <summary>
            /// Gets the top terrain object tile of the list
            /// </summary>
            /// <returns>The tile</returns>
            public TerrainObject getTerrainObjectTopTile()
            {
                return terrainObjects[terrainObjectTileCount - 1];
            }
            /// <summary>
            /// Checks whether the terrain object list is empty or not
            /// </summary>
            /// <returns>True if it is empty, false if not</returns>
            public bool isTerrainObjectEmpty()
            {
                return terrainObjectTileCount == 0;
            }
            #endregion
            
        #endregion

        #region Constructor
            public MapCell()
            {
                terrain = new List<Tile>();
                terrainTileCount = 0;
                terrainObjects = new List<TerrainObject>();
                terrainObjectTileCount = 0;
                terrainObjectColor = new List<Color>();
                terrainScreenPosition = new List<Vector2>();
                terrainDepth = new List<float>();
                terrainColor = new List<Color>();
                terrainObjectPosition = new List<Vector2>();
                terrainObjectDepth = new List<float>();
            }
        #endregion

        #region GeneralMethods
            /// <summary>
        /// Given a TileID, this methods calculates the source rectangle in the tileSheet
        /// </summary>
        /// <param name="tileID">Tile ID</param>
        /// <param name="nCols">number of columns of the tileSheet</param>
        /// <param name="requiresPadding">True if requires padding, false if not</param>
        /// <returns>The rectangle</returns>
        private static Rectangle getSourceRectangle(int tileID, int nCols, bool requiresPadding)
        {
            int row = tileID / nCols;
            int col = tileID % nCols;
            if(requiresPadding)
                return new Rectangle(
                    (int)(col * (IsoInfo.TileWidth + IsoInfo.SpriteSheetRowPadding)), 
                    (int)(row * (IsoInfo.TileHeight + IsoInfo.SpriteSheetColPadding)), 
                    (int)IsoInfo.TileWidth, (int)IsoInfo.TileHeight);
            else
                return new Rectangle(
                    (int)(col * (IsoInfo.TileWidth)),
                    (int)(row * (IsoInfo.TileHeight)),
                    (int)IsoInfo.TileWidth, (int)IsoInfo.TileHeight);
        }

        public void Update(GameTime gameTime, Vector2 cellCoords)
        {
            for (int i = 0; i < this.terrainTileCount; ++i)
            {
                //System.Diagnostics.Debug.Print("Entered Terrain drawing loop!\n");
                Vector3 position = new Vector3(cellCoords.X * IsoInfo.TileSide, cellCoords.Y * IsoInfo.TileSide, i * IsoInfo.TileElevation);
                Vector3 cellPosition = new Vector3(cellCoords.X, cellCoords.Y, i);
                Vector2 screenPosition = Coordinates.toScreen(position) - IsoInfo.TileOrigin - Camera.Position;
                float depth = Camera.getDepth(cellPosition, DrawableType.TILE);

                this.terrainScreenPosition[i] = screenPosition;
                this.terrainDepth[i] = depth;
            }

          
            for (int j = 0; j < this.terrainObjectTileCount; ++j)
            {
                Vector3 position = new Vector3(cellCoords.X * IsoInfo.TileSide, cellCoords.Y * IsoInfo.TileSide, this.getTerrainElevation() + j * IsoInfo.TileHeight);
                Vector3 cellPosition = new Vector3(cellCoords.X, cellCoords.Y, j + this.getTerrainElevation() / IsoInfo.TileElevation);
                Vector2 screenPosition = Coordinates.toScreen(position) - IsoInfo.TileOrigin - Camera.Position;
                float depth = Camera.getDepth(cellPosition, DrawableType.OBJECT);

                if (this.terrainObjectPosition.Count > j)
                {
                    this.terrainObjectPosition[j] = screenPosition;
                }
                else
                {
                    this.terrainObjectPosition.Add(screenPosition);
                }

                if (this.terrainObjectDepth.Count > j)
                {
                    this.terrainObjectDepth[j] = depth;
                }
                else
                {
                    this.terrainObjectDepth.Add(depth);
                }
            }
        }

        /// <summary>
        /// Draws a map cell
        /// </summary>
        /// <param name="batch">sprite batch</param>
        /// <param name="cellCoords">coordenates of the cell to draw</param>
        public void Draw(SpriteBatch batch, Vector2 cellCoords)
        {
            Texture2D tileSheet = MapManager.Instance.tileSheet;
            Texture2D objectSheet = MapManager.Instance.objectSheet;
            
            //MapCell myCell = MapManager.Instance.CurrentMap.Cells[(int)cellCoords.Y][(int)cellCoords.X];

            // Draw Terrain tiles
            for (int i = 0; i < this.terrainTileCount; ++i)
            {
                batch.Draw(
                        tileSheet, terrainScreenPosition[i],
                        getSourceRectangle((int)this.getTerrainTile(i), MapManager.Instance.properties.tileSheetCols, true), terrainColor[i],
                        0f, Vector2.Zero, 1f, SpriteEffects.None, // default/unused
                        terrainDepth[i]  // Depth                       
                        );
                /*
                batch.DrawString(
                    Game1.font, "{" + cellCoords.X + "," + cellCoords.Y + "}", Coordinates.toScreen(position) - IsoInfo.TileOrigin - Camera.Position + new Vector2(-10 + 32, 8), Color.Red, 
                    0f, Vector2.Zero, 0.3f, SpriteEffects.None,
                    Camera.getDepth(Vector3.Zero, DrawableType.TILE, SpecialDepthPrecedence.WORLD_INFO));
                 * */
            }
            // Draw terrainObjects tiles
            for (int i = 0; i < this.terrainObjectTileCount; ++i)
            {
                batch.Draw(
                        objectSheet, terrainObjectPosition[i],
                        getSourceRectangle((int)this.getTerrainObjectTile(i), MapManager.Instance.properties.objectSheetCols, false), terrainObjectColor[i],
                        0f, Vector2.Zero, 1f, SpriteEffects.None, // default/unused
                        terrainObjectDepth[i]  // Depth                       
                        );
            }
        }
        #endregion
    }
}
